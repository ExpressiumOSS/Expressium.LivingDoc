using Expressium.LivingDoc.Models;
using System;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocProjectHistoryHealthTests
    {
        private static readonly DateTime BaseDate = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        private static LivingDocExample CreateExample(string status)
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = status });
            return example;
        }

        private static LivingDocExample CreateExampleWithHistory(params string[] statuses)
        {
            var example = new LivingDocExample();
            for (int i = 0; i < statuses.Length; i++)
                example.History.Add(new LivingDocExampleHistoryResults { Date = BaseDate.AddDays(i), Status = statuses[i] });
            return example;
        }

        private static LivingDocProject CreateRun(DateTime date, params (string Feature, string Scenario, string[] ExampleStatuses)[] scenarios)
        {
            var project = new LivingDocProject { Date = date };

            foreach (var group in scenarios.GroupBy(s => s.Feature))
            {
                var feature = new LivingDocFeature { Name = group.Key };
                foreach (var item in group)
                {
                    var scenario = new LivingDocScenario { Name = item.Scenario };
                    foreach (var status in item.ExampleStatuses)
                        scenario.Examples.Add(CreateExample(status));
                    feature.Scenarios.Add(scenario);
                }
                project.Features.Add(feature);
            }

            return project;
        }

        private static LivingDocScenario GetScenario(LivingDocProject project, string featureName, string scenarioName)
        {
            return project.Features.Single(f => f.Name == featureName).Scenarios.Single(s => s.Name == scenarioName);
        }

        // HasHistory...

        [Test]
        public void LivingDocProject_HasHistory_ReturnsFalse_WhenEmpty()
        {
            Assert.That(new LivingDocProject().HasHistory(), Is.False);
        }

        [Test]
        public void LivingDocProject_HasHistory_ReturnsFalse_WithSingleRun()
        {
            var project = new LivingDocProject();
            project.History.Features.Add(new LivingDocProjectHistoryResults { Date = BaseDate });
            project.History.Scenarios.Add(new LivingDocProjectHistoryResults { Date = BaseDate });
            project.History.Steps.Add(new LivingDocProjectHistoryResults { Date = BaseDate });

            Assert.That(project.HasHistory(), Is.False);
        }

        [TestCase("Features")]
        [TestCase("Scenarios")]
        [TestCase("Steps")]
        public void LivingDocProject_HasHistory_ReturnsTrue_WhenAnyListHasTwoRuns(string list)
        {
            var project = new LivingDocProject();
            var target = list == "Features" ? project.History.Features : list == "Scenarios" ? project.History.Scenarios : project.History.Steps;
            target.Add(new LivingDocProjectHistoryResults { Date = BaseDate });
            target.Add(new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1) });

            Assert.That(project.HasHistory(), Is.True);
        }

        // MergeHistory...

        [Test]
        public void LivingDocProject_MergeHistory_MultipleRuns_AreAddedInMergeOrder()
        {
            var master = CreateRun(BaseDate.AddDays(3), ("Login", "Valid", new[] { "Passed" }));

            master.MergeHistory(CreateRun(BaseDate, ("Login", "Valid", new[] { "Failed" })));
            master.MergeHistory(CreateRun(BaseDate.AddDays(1), ("Login", "Valid", new[] { "Passed" })));
            master.MergeHistory(CreateRun(BaseDate.AddDays(2), ("Login", "Valid", new[] { "Passed" })));

            Assert.That(master.History.Scenarios.Select(h => h.Date), Is.EqualTo(new[] { BaseDate, BaseDate.AddDays(1), BaseDate.AddDays(2) }));
            Assert.That(master.History.Scenarios.Select(h => h.Failed), Is.EqualTo(new[] { 1, 0, 0 }));
            Assert.That(master.History.Scenarios.Select(h => h.Passed), Is.EqualTo(new[] { 0, 1, 1 }));
            Assert.That(master.HasHistory(), Is.True);

            var example = GetScenario(master, "Login", "Valid").Examples[0];
            Assert.That(example.History.Select(h => h.Status), Is.EqualTo(new[] { "Failed", "Passed", "Passed" }));
        }

        [Test]
        public void LivingDocProject_MergeHistory_SameDate_IsMergedOnlyOnce()
        {
            var master = CreateRun(BaseDate.AddDays(1), ("Login", "Valid", new[] { "Passed" }));
            var run = CreateRun(BaseDate, ("Login", "Valid", new[] { "Failed" }));

            master.MergeHistory(run);
            master.MergeHistory(run);
            master.MergeHistory(CreateRun(BaseDate, ("Login", "Valid", new[] { "Passed" })));

            Assert.That(master.History.Features.Count, Is.EqualTo(1));
            Assert.That(master.History.Scenarios.Count, Is.EqualTo(1));
            Assert.That(master.History.Steps.Count, Is.EqualTo(1));
            Assert.That(GetScenario(master, "Login", "Valid").Examples[0].History.Count, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocProject_MergeHistory_CountsStepsPerStatus()
        {
            var master = new LivingDocProject();

            var run = new LivingDocProject { Date = BaseDate };
            var feature = new LivingDocFeature { Name = "Checkout" };
            var scenario = new LivingDocScenario { Name = "Pay" };
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = "Passed" });
            example.Steps.Add(new LivingDocStep { Status = "Failed" });
            example.Steps.Add(new LivingDocStep { Status = "Skipped" });
            example.Steps.Add(new LivingDocStep { Status = "Undefined" });
            scenario.Examples.Add(example);
            feature.Scenarios.Add(scenario);
            run.Features.Add(feature);

            master.MergeHistory(run);

            var steps = master.History.Steps[0];
            Assert.That(steps.Passed, Is.EqualTo(1));
            Assert.That(steps.Failed, Is.EqualTo(1));
            Assert.That(steps.Skipped, Is.EqualTo(1));
            Assert.That(steps.Incomplete, Is.EqualTo(1));
            Assert.That(steps.GetNumberOfTests(), Is.EqualTo(4));
        }

        [Test]
        public void LivingDocProject_MergeHistory_ScenarioOutline_MapsExamplesByIndex()
        {
            var master = CreateRun(BaseDate.AddDays(1), ("Orders", "Outline", new[] { "Passed", "Passed", "Passed" }));
            master.MergeHistory(CreateRun(BaseDate, ("Orders", "Outline", new[] { "Passed", "Failed", "Skipped" })));

            var scenario = GetScenario(master, "Orders", "Outline");
            Assert.That(scenario.Examples[0].History.Single().Status, Is.EqualTo("Passed"));
            Assert.That(scenario.Examples[1].History.Single().Status, Is.EqualTo("Failed"));
            Assert.That(scenario.Examples[2].History.Single().Status, Is.EqualTo("Skipped"));
        }

        [Test]
        public void LivingDocProject_MergeHistory_ScenarioOutline_ExtraMasterExamples_GetNoHistory()
        {
            // The history run had fewer examples than the current run...
            var master = CreateRun(BaseDate.AddDays(1), ("Orders", "Outline", new[] { "Passed", "Passed", "Passed" }));
            master.MergeHistory(CreateRun(BaseDate, ("Orders", "Outline", new[] { "Failed" })));

            var scenario = GetScenario(master, "Orders", "Outline");
            Assert.That(scenario.Examples[0].History.Count, Is.EqualTo(1));
            Assert.That(scenario.Examples[1].History, Is.Empty);
            Assert.That(scenario.Examples[2].History, Is.Empty);
        }

        [Test]
        public void LivingDocProject_MergeHistory_RemovedScenario_IsIgnored()
        {
            // A scenario that only exists in the history run is not added to the master...
            var master = CreateRun(BaseDate.AddDays(1), ("Login", "Valid", new[] { "Passed" }));
            master.MergeHistory(CreateRun(BaseDate, ("Login", "Valid", new[] { "Passed" }), ("Login", "Removed", new[] { "Failed" })));

            Assert.That(master.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(master.History.Scenarios[0].Failed, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocProject_MergeHistory_CalculatesHealth_AfterEachMerge()
        {
            // Current run results are expected to be merged as the latest history entry as well...
            var master = CreateRun(BaseDate.AddDays(2), ("Login", "Valid", new[] { "Failed" }));

            master.MergeHistory(CreateRun(BaseDate, ("Login", "Valid", new[] { "Passed" })));
            Assert.That(GetScenario(master, "Login", "Valid").Health, Is.Null);

            master.MergeHistory(CreateRun(BaseDate.AddDays(1), ("Login", "Valid", new[] { "Failed" })));
            Assert.That(GetScenario(master, "Login", "Valid").Health, Is.EqualTo(LivingDocHealths.Regressed.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeHistory_NewScenario_IsMarkedNew()
        {
            var master = CreateRun(BaseDate.AddDays(2), ("Login", "Valid", new[] { "Passed" }), ("Login", "Added", new[] { "Passed" }));

            master.MergeHistory(CreateRun(BaseDate, ("Login", "Valid", new[] { "Passed" })));
            master.MergeHistory(CreateRun(BaseDate.AddDays(1), ("Login", "Valid", new[] { "Passed" }), ("Login", "Added", new[] { "Passed" })));

            Assert.That(GetScenario(master, "Login", "Added").Health, Is.EqualTo(LivingDocHealths.New.ToString()));
            Assert.That(GetScenario(master, "Login", "Valid").Health, Is.Null);
        }

        // MergeScenarioHistoryHealth - Scenario Outlines with multiple examples...

        private static LivingDocScenario EvaluateHealth(params LivingDocExample[] examples)
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Feature" };
            var scenario = new LivingDocScenario { Name = "Outline" };
            scenario.Examples.AddRange(examples);
            feature.Scenarios.Add(scenario);
            project.Features.Add(feature);

            project.History.Scenarios.Add(new LivingDocProjectHistoryResults { Date = BaseDate });
            project.History.Scenarios.Add(new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1) });

            project.MergeScenarioHistoryHealth();
            return scenario;
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MultipleExamples_AllStable_HealthIsNull()
        {
            var scenario = EvaluateHealth(
                CreateExampleWithHistory("Passed", "Passed"),
                CreateExampleWithHistory("Passed", "Passed"));

            Assert.That(scenario.Health, Is.Null);
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MultipleExamples_FirstIssueWins()
        {
            var scenario = EvaluateHealth(
                CreateExampleWithHistory("Failed", "Failed"),
                CreateExampleWithHistory("Passed", "Failed"));

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.Broken.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MultipleExamples_StableThenFlaky_IsFlaky()
        {
            var scenario = EvaluateHealth(
                CreateExampleWithHistory("Passed", "Passed"),
                CreateExampleWithHistory("Passed", "Failed", "Passed", "Failed"));

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.Flaky.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MultipleExamples_FixedIsOverriddenByLaterIssue()
        {
            var scenario = EvaluateHealth(
                CreateExampleWithHistory("Failed", "Passed"),
                CreateExampleWithHistory("Passed", "Failed"));

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.Regressed.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MultipleExamples_FixedThenStable_RemainsFixed()
        {
            var scenario = EvaluateHealth(
                CreateExampleWithHistory("Failed", "Passed"),
                CreateExampleWithHistory("Passed", "Passed"));

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.Fixed.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MultipleExamples_NewExampleWins()
        {
            // An example with a single history entry marks the scenario as New, later examples are not evaluated...
            var scenario = EvaluateHealth(
                CreateExampleWithHistory("Passed"),
                CreateExampleWithHistory("Passed", "Failed"));

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.New.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MultipleExamples_NewAfterIssue_IsNew()
        {
            var scenario = EvaluateHealth(
                CreateExampleWithHistory("Failed", "Failed"),
                CreateExampleWithHistory("Passed"));

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.New.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_ResetsPreviousHealth()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Feature" };
            var scenario = new LivingDocScenario { Name = "Scenario", Health = LivingDocHealths.Broken.ToString() };
            scenario.Examples.Add(CreateExampleWithHistory("Passed", "Passed", "Passed"));
            feature.Scenarios.Add(scenario);
            project.Features.Add(feature);

            project.MergeScenarioHistoryHealth();

            Assert.That(scenario.Health, Is.Null);
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_ScenarioWithoutExamples_HealthIsNull()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Feature" };
            var scenario = new LivingDocScenario { Name = "Scenario", Health = LivingDocHealths.Flaky.ToString() };
            feature.Scenarios.Add(scenario);
            project.Features.Add(feature);

            project.MergeScenarioHistoryHealth();

            Assert.That(scenario.Health, Is.Null);
            Assert.That(project.HasHealth(), Is.False);
        }

        [Test]
        public void LivingDocProject_MergeScenarioHistoryHealth_MoreThanFourRuns_UsesLatestFour()
        {
            // Older entries than the latest four are not considered...
            var scenario = EvaluateHealth(CreateExampleWithHistory("Passed", "Failed", "Failed", "Failed", "Failed", "Failed"));

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.Broken.ToString()));
        }
    }
}
