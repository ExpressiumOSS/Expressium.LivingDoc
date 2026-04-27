using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataAnalyticsGeneratorTests
    {
        private LivingDocProject CreateProjectWithTwoHistories()
        {
            var project = new LivingDocProject();

            var historyOneFeatures = new LivingDocProjectHistoryResults { Date = DateTime.UtcNow.AddDays(-1), Passed = 3, Incomplete = 1, Failed = 1, Skipped = 0 };
            var historyOneScenarios = new LivingDocProjectHistoryResults { Date = DateTime.UtcNow.AddDays(-1), Passed = 2, Incomplete = 0, Failed = 1, Skipped = 0 };
            var historyOneSteps = new LivingDocProjectHistoryResults { Date = DateTime.UtcNow.AddDays(-1), Passed = 2, Incomplete = 0, Failed = 1, Skipped = 0 };

            var historyTwoFeatures = new LivingDocProjectHistoryResults { Date = DateTime.UtcNow, Passed = 6, Incomplete = 2, Failed = 1, Skipped = 1 };
            var historyTwoScenarios = new LivingDocProjectHistoryResults { Date = DateTime.UtcNow, Passed = 3, Incomplete = 0, Failed = 0, Skipped = 0 };
            var historyTwoSteps = new LivingDocProjectHistoryResults { Date = DateTime.UtcNow, Passed = 3, Incomplete = 0, Failed = 0, Skipped = 0 };

            project.History.Features.Add(historyOneFeatures);
            project.History.Features.Add(historyTwoFeatures);
            project.History.Scenarios.Add(historyOneScenarios);
            project.History.Scenarios.Add(historyTwoScenarios);
            project.History.Steps.Add(historyOneSteps);
            project.History.Steps.Add(historyTwoSteps);

            return project;
        }

        private LivingDocScenario CreateScenarioWithHealth(string name, string health)
        {
            var scenario = new LivingDocScenario { Name = name, Health = health };
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            scenario.Examples.Add(example);
            return scenario;
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsTitle
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTitle()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataAnalyticsTitle();

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<div class='section'>"));
            Assert.That(listOfLines[1], Is.EqualTo("<span class='page-name' data-testid='page-title'>Analytics</span>"));
            Assert.That(listOfLines[2], Is.EqualTo("</div>"));
            Assert.That(listOfLines[3], Is.EqualTo("<hr>"));
            Assert.That(listOfLines[4], Is.EqualTo("<hr>"));
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsDuration
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsDuration()
        {
            var project = new LivingDocProject();
            project.Duration = new TimeSpan(0, 4, 12, 30, 0);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsDuration();

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[1], Is.EqualTo("<!-- Data Analytics Duration -->"));
            Assert.That(listOfLines[6], Is.EqualTo("<span data-testid='project-duration'>4h 12min</span>"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsDuration_Zero()
        {
            var project = new LivingDocProject();
            project.Duration = TimeSpan.Zero;

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsDuration();

            Assert.That(listOfLines[6], Is.EqualTo("<span data-testid='project-duration'>0s 000ms</span>"));
        }

        // -------------------------------------------------------
        // CalculatePercentage
        // -------------------------------------------------------

        [TestCase(0, 10, ExpectedResult = 0)]
        [TestCase(5, 10, ExpectedResult = 50)]
        [TestCase(1, 100, ExpectedResult = 1)]
        [TestCase(99, 100, ExpectedResult = 99)]
        [TestCase(1, 200, ExpectedResult = 1)]
        [TestCase(2, 3, ExpectedResult = 67)]
        [TestCase(1, 3, ExpectedResult = 33)]
        [TestCase(3, 3, ExpectedResult = 100)]
        [TestCase(0, 1, ExpectedResult = 0)]
        [TestCase(1, 1, ExpectedResult = 100)]
        [TestCase(6, 11, ExpectedResult = 55)]
        [TestCase(2, 11, ExpectedResult = 18)]
        [TestCase(1, 11, ExpectedResult = 9)]
        [TestCase(11, 11, ExpectedResult = 100)]
        [TestCase(0, 0, ExpectedResult = 0)]
        public int LivingDocDataAnalyticsGenerator_CalculatePercentage(int numberOfStatuses, int numberOfTests)
        {
            return LivingDocDataAnalyticsGenerator.CalculatePercentage(numberOfStatuses, numberOfTests);
        }

        // -------------------------------------------------------
        // AdjustPercentagesDiscrepancy
        // -------------------------------------------------------

        [TestCase(100, 0, 0, 0, 100, 0, 0, 0)]
        [TestCase(0, 0, 0, 0, 0, 0, 0, 0)]
        [TestCase(10, 20, 30, 55, 10, 20, 30, 40)]
        [TestCase(10, 20, 30, 45, 10, 20, 30, 40)]
        [TestCase(11, 21, 31, 41, 11, 21, 31, 37)]
        [TestCase(11, 19, 30, 40, 11, 19, 30, 40)]
        public void LivingDocDataAnalyticsGenerator_AdjustPercentagesDiscrepancy(
            int passed, int incomplete, int failed, int skipped,
            int passedOut, int incompleteOut, int failedOut, int skippedOut)
        {
            LivingDocDataAnalyticsGenerator.AdjustPercentagesDiscrepancy(ref passed, ref incomplete, ref failed, ref skipped);

            Assert.That(passed, Is.EqualTo(passedOut));
            Assert.That(incomplete, Is.EqualTo(incompleteOut));
            Assert.That(failed, Is.EqualTo(failedOut));
            Assert.That(skipped, Is.EqualTo(skippedOut));
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsStatusChart
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsStatusChart_Zero_Totals()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataAnalyticsStatusChart("Title", 0, 0, 0, 0, 0);

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Status Chart -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<span class='chart-name' data-testid='title-chart-title'>Title</span>"));
            Assert.That(listOfLines.Any(l => l.Trim().StartsWith("<circle")), Is.False);
            Assert.That(listOfLines.Any(l => l.Contains("data-testid='title-chart-passed'>0%")), Is.True);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsStatusChart_Passed_Only()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataAnalyticsStatusChart("Title", 10, 0, 0, 0, 10);

            Assert.That(listOfLines[1], Is.EqualTo("<span class='chart-name' data-testid='title-chart-title'>Title</span>"));
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-passed")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-incomplete")), Is.False);
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-failed")), Is.False);
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-skipped")), Is.False);
            Assert.That(listOfLines.Any(l => l.Contains("data-testid='title-chart-passed'>100%")), Is.True);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsStatusChart_All_Statuses()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataAnalyticsStatusChart("Title", 5, 4, 3, 2, 14);

            Assert.That(listOfLines[1], Is.EqualTo("<span class='chart-name' data-testid='title-chart-title'>Title</span>"));
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-passed")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-incomplete")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-failed")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("donut-segment-skipped")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("data-testid='title-chart-passed'>36%")), Is.True);
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsFeaturesView
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsFeaturesView()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataAnalyticsFeaturesView();

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Analytics -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='analytics-features'>"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Status Chart -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Duration -->"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsScenariosView
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsScenariosView()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataAnalyticsScenariosView();

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Analytics -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='analytics-scenarios'>"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Status Chart -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Duration -->"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsStepsView
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsStepsView()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataAnalyticsStepsView();

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Analytics -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='analytics-steps'>"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Status Chart -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Duration -->"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsTrends
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_Returns_Empty_With_No_History()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());

            Assert.That(generator.GenerateDataAnalyticsTrends("Features").Count, Is.EqualTo(0));
            Assert.That(generator.GenerateDataAnalyticsTrends("Scenarios").Count, Is.EqualTo(0));
            Assert.That(generator.GenerateDataAnalyticsTrends("Steps").Count, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_Returns_Empty_With_Single_History()
        {
            var project = new LivingDocProject();
            project.History.Features.Add(new LivingDocProjectHistoryResults { Date = DateTime.UtcNow, Passed = 1 });
            project.History.Scenarios.Add(new LivingDocProjectHistoryResults { Date = DateTime.UtcNow, Passed = 1 });
            project.History.Steps.Add(new LivingDocProjectHistoryResults { Date = DateTime.UtcNow, Passed = 1 });

            var generator = new LivingDocDataAnalyticsGenerator(project);

            Assert.That(generator.GenerateDataAnalyticsTrends("Features").Count, Is.EqualTo(0));
            Assert.That(generator.GenerateDataAnalyticsTrends("Scenarios").Count, Is.EqualTo(0));
            Assert.That(generator.GenerateDataAnalyticsTrends("Steps").Count, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_Features()
        {
            var generator = new LivingDocDataAnalyticsGenerator(CreateProjectWithTwoHistories());
            var listOfLines = generator.GenerateDataAnalyticsTrends("Features");

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Trend Chart -->"));
            Assert.That(listOfLines, Does.Contain("<div class='section analytics-trends'>"));
            Assert.That(listOfLines.Any(l => l.Contains("bgcolor-passed")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("bgcolor-failed")), Is.True);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_Scenarios()
        {
            var generator = new LivingDocDataAnalyticsGenerator(CreateProjectWithTwoHistories());
            var listOfLines = generator.GenerateDataAnalyticsTrends("Scenarios");

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Trend Chart -->"));
            Assert.That(listOfLines.Any(l => l.Contains("bgcolor-passed")), Is.True);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_Steps()
        {
            var generator = new LivingDocDataAnalyticsGenerator(CreateProjectWithTwoHistories());
            var listOfLines = generator.GenerateDataAnalyticsTrends("Steps");

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Trend Chart -->"));
            Assert.That(listOfLines.Any(l => l.Contains("bgcolor-passed")), Is.True);
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsHealths
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsHealths_Returns_Empty_With_No_Health()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Login" };
            feature.Scenarios.Add(new LivingDocScenario { Name = "Scenario A" });
            project.Features.Add(feature);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsHealths();

            Assert.That(listOfLines.Count, Is.GreaterThan(0));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsHealths_Fixed()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Login" };
            feature.Scenarios.Add(CreateScenarioWithHealth("Login Scenario", LivingDocHealths.Fixed.ToString()));
            project.Features.Add(feature);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsHealths();

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Healths Chart -->"));
            Assert.That(listOfLines.Any(l => l.Contains("history-passed") && l.Contains("Fixed")), Is.True);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsHealths_Broken()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Login" };
            feature.Scenarios.Add(CreateScenarioWithHealth("Login Scenario", LivingDocHealths.Broken.ToString()));
            project.Features.Add(feature);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsHealths();

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Healths Chart -->"));
            Assert.That(listOfLines.Any(l => l.Contains("history-failed") && l.Contains("Broken")), Is.True);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsHealths_Flaky()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Login" };
            feature.Scenarios.Add(CreateScenarioWithHealth("Login Scenario", LivingDocHealths.Flaky.ToString()));
            project.Features.Add(feature);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsHealths();

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Healths Chart -->"));
            Assert.That(listOfLines.Any(l => l.Contains("history-failed") && l.Contains("Flaky")), Is.True);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsHealths_Multiple_Scenarios()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Login" };
            feature.Scenarios.Add(CreateScenarioWithHealth("Scenario A", LivingDocHealths.Fixed.ToString()));
            feature.Scenarios.Add(CreateScenarioWithHealth("Scenario B", LivingDocHealths.Broken.ToString()));
            feature.Scenarios.Add(CreateScenarioWithHealth("Scenario C", LivingDocHealths.Flaky.ToString()));
            project.Features.Add(feature);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsHealths();

            Assert.That(listOfLines.Any(l => l.Contains("Scenario A")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("Scenario B")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("Scenario C")), Is.True);
        }

        // -------------------------------------------------------
        // GenerateDataAnalyticsFailures
        // -------------------------------------------------------

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsFailures_Returns_Empty_With_No_Health()
        {
            var project = new LivingDocProject();
            var feature = new LivingDocFeature { Name = "Login" };
            feature.Scenarios.Add(new LivingDocScenario { Name = "Scenario A" });
            project.Features.Add(feature);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsFailures();

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Failures Chart -->"));
            Assert.That(listOfLines.Any(l => l.Contains("<td>")), Is.False);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsFailures_With_Health_And_History()
        {
            var project = new LivingDocProject();

            project.History.Scenarios.Add(new LivingDocProjectHistoryResults { Date = DateTime.UtcNow.AddDays(-1), Failed = 1 });
            project.History.Scenarios.Add(new LivingDocProjectHistoryResults { Date = DateTime.UtcNow, Passed = 1 });

            var feature = new LivingDocFeature { Name = "Login" };
            var scenario = CreateScenarioWithHealth("Login Scenario", LivingDocHealths.Broken.ToString());

            scenario.Examples[0].History.Add(new LivingDocExampleHistoryResults
            {
                Date = DateTime.UtcNow.AddDays(-1),
                Status = LivingDocStatuses.Failed.ToString()
            });
            scenario.Examples[0].History.Add(new LivingDocExampleHistoryResults
            {
                Date = DateTime.UtcNow,
                Status = LivingDocStatuses.Passed.ToString()
            });

            feature.Scenarios.Add(scenario);
            project.Features.Add(feature);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsFailures();

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Failures Chart -->"));
            Assert.That(listOfLines.Any(l => l.Contains("Login Scenario")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("history-failed")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("history-passed")), Is.True);
            Assert.That(listOfLines.Any(l => l.Contains("history-skipped") && l.Contains("Broken")), Is.True);
        }
    }
}
