using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocFeatureTests
    {
        // ---------------------------------------------------------------------------
        // Helper factory methods
        // ---------------------------------------------------------------------------

        private static LivingDocStep CreateStep(LivingDocStatuses status)
        {
            return new LivingDocStep { Status = status.ToString() };
        }

        private static LivingDocExample CreateExample(params LivingDocStatuses[] stepStatuses)
        {
            var example = new LivingDocExample();
            foreach (var status in stepStatuses)
                example.Steps.Add(CreateStep(status));
            return example;
        }

        private static LivingDocScenario CreateScenario(params LivingDocExample[] examples)
        {
            var scenario = new LivingDocScenario();
            foreach (var example in examples)
                scenario.Examples.Add(example);
            return scenario;
        }

        private static LivingDocFeature CreateFeatureWithScenario(params LivingDocStatuses[] stepStatuses)
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(CreateExample(stepStatuses)));
            return feature;
        }

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_Constructor_DefaultsAreCorrect()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.Id, Is.Not.Null);
            Assert.That(feature.Id, Is.Not.Empty);
            Assert.That(feature.Tags, Is.Not.Null);
            Assert.That(feature.Tags, Is.Empty);
            Assert.That(feature.Rules, Is.Not.Null);
            Assert.That(feature.Rules, Is.Empty);
            Assert.That(feature.Scenarios, Is.Not.Null);
            Assert.That(feature.Scenarios, Is.Empty);
            Assert.That(feature.Background, Is.Null);
        }

        [Test]
        public void LivingDocFeature_Constructor_IdIsUniquePerInstance()
        {
            var feature1 = new LivingDocFeature();
            var feature2 = new LivingDocFeature();

            Assert.That(feature1.Id, Is.Not.EqualTo(feature2.Id));
        }

        // ---------------------------------------------------------------------------
        // GetDataTags / GetDataStatus
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetDataTags_WithNoTags_ReturnsEmptyString()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetDataTags(), Is.EqualTo(""));
        }

        [Test]
        public void LivingDocFeature_GetDataTags_WithMultipleTags_ReturnsSpaceSeparated()
        {
            var feature = new LivingDocFeature();
            feature.Tags.Add("@smoke");
            feature.Tags.Add("@regression");
            feature.Tags.Add("@login");

            Assert.That(feature.GetDataTags(), Is.EqualTo("@smoke @regression @login"));
        }

        [Test]
        public void LivingDocFeature_GetDataStatus_ReturnsPrefixedStatus()
        {
            var feature = CreateFeatureWithScenario(LivingDocStatuses.Passed);

            Assert.That(feature.GetDataStatus(), Is.EqualTo("@" + LivingDocStatuses.Passed.ToString()));
        }

        // ---------------------------------------------------------------------------
        // GetStatus / IsPassed / IsIncomplete / IsFailed / IsSkipped
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetStatus_ReturnsSkipped_WhenNoScenarios()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void LivingDocFeature_GetStatus_ReturnsPassed()
        {
            var feature = CreateFeatureWithScenario(LivingDocStatuses.Passed);

            Assert.That(feature.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(feature.IsPassed(), Is.True);
            Assert.That(feature.IsFailed(), Is.False);
            Assert.That(feature.IsIncomplete(), Is.False);
            Assert.That(feature.IsSkipped(), Is.False);
        }

        [Test]
        public void LivingDocFeature_GetStatus_ReturnsFailed()
        {
            var feature = CreateFeatureWithScenario(LivingDocStatuses.Failed);

            Assert.That(feature.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(feature.IsFailed(), Is.True);
            Assert.That(feature.IsPassed(), Is.False);
            Assert.That(feature.IsIncomplete(), Is.False);
            Assert.That(feature.IsSkipped(), Is.False);
        }

        [Test]
        public void LivingDocFeature_GetStatus_ReturnsIncomplete()
        {
            var feature = CreateFeatureWithScenario(LivingDocStatuses.Incomplete);

            Assert.That(feature.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(feature.IsIncomplete(), Is.True);
            Assert.That(feature.IsPassed(), Is.False);
            Assert.That(feature.IsFailed(), Is.False);
            Assert.That(feature.IsSkipped(), Is.False);
        }

        [Test]
        public void LivingDocFeature_GetStatus_ReturnsSkipped()
        {
            var feature = CreateFeatureWithScenario(LivingDocStatuses.Skipped);

            Assert.That(feature.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(feature.IsSkipped(), Is.True);
            Assert.That(feature.IsPassed(), Is.False);
            Assert.That(feature.IsFailed(), Is.False);
            Assert.That(feature.IsIncomplete(), Is.False);
        }

        [Test]
        public void LivingDocFeature_GetStatus_FailedTakesPriorityOverIncomplete()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(
                CreateExample(LivingDocStatuses.Failed),
                CreateExample(LivingDocStatuses.Incomplete)));

            Assert.That(feature.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
        }

        [Test]
        public void LivingDocFeature_GetStatus_IncompleteBeforeSkipped()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(
                CreateExample(LivingDocStatuses.Incomplete),
                CreateExample(LivingDocStatuses.Skipped)));

            Assert.That(feature.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
        }

        // ---------------------------------------------------------------------------
        // GetStatus (integration — file-based)
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetStatus_Failed_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "stack-traces", "stack-traces.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0001"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("000"));
            Assert.That(livingDocFeature.GetCoverage(), Is.EqualTo(100));
            Assert.That(livingDocFeature.GetCoverageSortId(), Is.EqualTo("100"));
            Assert.That(livingDocFeature.GetDataTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocFeature.IsFailed(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 003ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:003"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\stack-traces"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Incomplete_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "pending", "pending.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("000"));
            Assert.That(livingDocFeature.GetDataTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocFeature.IsIncomplete(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 013ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:013"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\pending"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Passed_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "minimal", "minimal.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0001"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(100));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("100"));
            Assert.That(livingDocFeature.GetDataTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocFeature.IsPassed(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 003ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:003"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\minimal"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Skipped_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "empty", "empty.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0001"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("000"));
            Assert.That(livingDocFeature.GetDataTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocFeature.IsSkipped(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 000ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:000"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\empty"));
        }

        // ---------------------------------------------------------------------------
        // GetNumberOfScenarios / scenario counts by status
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetNumberOfScenarios_ReturnsZero_WhenNoScenarios()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetNumberOfScenarios(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfScenarios_CountsExamplesAcrossScenarios()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(
                CreateExample(LivingDocStatuses.Passed),
                CreateExample(LivingDocStatuses.Passed)));
            feature.Scenarios.Add(CreateScenario(
                CreateExample(LivingDocStatuses.Failed)));

            Assert.That(feature.GetNumberOfScenarios(), Is.EqualTo(3));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfStatuses_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Contact Us"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(100));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("100"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(1));

            livingDocFeature = livingDocProject.Features[1];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Login"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0002"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("000"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(0));

            livingDocFeature = livingDocProject.Features[2];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Registration"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(67));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("067"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(2));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(0));

            livingDocFeature = livingDocProject.Features[3];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Orders"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPassRate(), Is.EqualTo(100));
            Assert.That(livingDocFeature.GetPassRateSortId(), Is.EqualTo("100"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(0));
        }

        // ---------------------------------------------------------------------------
        // GetNumberOfSteps / step counts by status
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetNumberOfSteps_ReturnsZero_WhenNoScenarios()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetNumberOfSteps(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfSteps_CountsStepsAcrossAllExamples()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(
                CreateExample(LivingDocStatuses.Passed, LivingDocStatuses.Passed),
                CreateExample(LivingDocStatuses.Failed, LivingDocStatuses.Skipped, LivingDocStatuses.Skipped)));

            Assert.That(feature.GetNumberOfSteps(), Is.EqualTo(5));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfSteps_ByStatus_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[3];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Orders"));
            Assert.That(livingDocFeature.GetNumberOfSteps(), Is.GreaterThan(0));
            Assert.That(livingDocFeature.GetNumberOfPassedSteps(), Is.EqualTo(livingDocFeature.GetNumberOfSteps()));
            Assert.That(livingDocFeature.GetNumberOfFailedSteps(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfIncompleteSteps(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfSkippedSteps(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfSteps_MixedStatuses_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var totalSteps = 0;
            var totalPassed = 0;
            var totalFailed = 0;
            var totalIncomplete = 0;
            var totalSkipped = 0;

            foreach (var feature in livingDocProject.Features)
            {
                totalSteps += feature.GetNumberOfSteps();
                totalPassed += feature.GetNumberOfPassedSteps();
                totalFailed += feature.GetNumberOfFailedSteps();
                totalIncomplete += feature.GetNumberOfIncompleteSteps();
                totalSkipped += feature.GetNumberOfSkippedSteps();
            }

            Assert.That(totalSteps, Is.EqualTo(30));
            Assert.That(totalPassed + totalFailed + totalIncomplete + totalSkipped, Is.EqualTo(totalSteps));
            Assert.That(totalPassed, Is.EqualTo(21));
            Assert.That(totalFailed, Is.EqualTo(2));
            Assert.That(totalIncomplete, Is.EqualTo(2));
            Assert.That(totalSkipped, Is.EqualTo(5));
        }

        // ---------------------------------------------------------------------------
        // GetNumberOfRules
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetNumberOfRules_ReturnsZero_WhenNoRules()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(new LivingDocScenario { Name = "Scenario without a rule" });

            Assert.That(feature.GetNumberOfRules(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfRules_CountsDistinctRuleIds()
        {
            var ruleId = System.Guid.NewGuid().ToString();
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(new LivingDocScenario { RuleId = ruleId });
            feature.Scenarios.Add(new LivingDocScenario { RuleId = ruleId });  // same rule, should not double-count
            feature.Scenarios.Add(new LivingDocScenario { RuleId = null });    // no rule

            Assert.That(feature.GetNumberOfRules(), Is.EqualTo(1));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfRules_ReturnsDistinctRuleCount_FromFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfRules(), Is.EqualTo(1));
        }

        // ---------------------------------------------------------------------------
        // GetPassRate / GetPassRateSortId
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetPassRate_ReturnsZero_WhenNoScenarios()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetPassRate(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocFeature_GetPassRate_Returns100_WhenAllPassed()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Passed)));
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Passed)));

            Assert.That(feature.GetPassRate(), Is.EqualTo(100));
            Assert.That(feature.GetPassRateSortId(), Is.EqualTo("100"));
        }

        [Test]
        public void LivingDocFeature_GetPassRate_Returns0_WhenAllFailed()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Failed)));

            Assert.That(feature.GetPassRate(), Is.EqualTo(0));
            Assert.That(feature.GetPassRateSortId(), Is.EqualTo("000"));
        }

        [Test]
        public void LivingDocFeature_GetPassRate_IsRounded_WhenMixed()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Passed)));
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Passed)));
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Failed)));

            // 2 passed out of 3 = 66.67 % → rounds to 67
            Assert.That(feature.GetPassRate(), Is.EqualTo(67));
            Assert.That(feature.GetPassRateSortId(), Is.EqualTo("067"));
        }

        // ---------------------------------------------------------------------------
        // GetCoverage / GetCoverageSortId
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetCoverage_ReturnsZero_WhenNoScenarios()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetCoverage(), Is.EqualTo(0));
            Assert.That(feature.GetCoverageSortId(), Is.EqualTo("000"));
        }

        [Test]
        public void LivingDocFeature_GetCoverage_Returns100_WhenAllExecuted()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Passed)));
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Failed)));

            Assert.That(feature.GetCoverage(), Is.EqualTo(100));
            Assert.That(feature.GetCoverageSortId(), Is.EqualTo("100"));
        }

        [Test]
        public void LivingDocFeature_GetCoverage_ExcludesSkippedScenarios()
        {
            var feature = new LivingDocFeature();
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Passed)));
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Skipped)));
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Skipped)));
            feature.Scenarios.Add(CreateScenario(CreateExample(LivingDocStatuses.Skipped)));

            // 1 of 4 executed = 25 %
            Assert.That(feature.GetCoverage(), Is.EqualTo(25));
        }

        // ---------------------------------------------------------------------------
        // GetFlakyRate / GetFlakyRateSortId
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetFlakyRate_ReturnsZero_WhenNoScenarios()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetFlakyRate(), Is.EqualTo(0));
            Assert.That(feature.GetFlakyRateSortId(), Is.EqualTo("000"));
        }

        [Test]
        public void LivingDocFeature_GetFlakyRate_ReturnsZero_WhenNoFlakyScenarios()
        {
            var feature = CreateFeatureWithScenario(LivingDocStatuses.Passed);

            Assert.That(feature.GetFlakyRate(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocFeature_GetFlakyRate_Returns100_WhenAllScenariosAreFlaky()
        {
            var feature = new LivingDocFeature();
            var scenario = new LivingDocScenario { Health = LivingDocHealths.Flaky.ToString() };
            scenario.Examples.Add(CreateExample(LivingDocStatuses.Passed));
            feature.Scenarios.Add(scenario);

            Assert.That(feature.GetFlakyRate(), Is.EqualTo(100));
            Assert.That(feature.GetFlakyRateSortId(), Is.EqualTo("100"));
        }

        // ---------------------------------------------------------------------------
        // GetDuration / GetDurationSortId
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetDuration_ReturnsZero_WhenNoScenarios()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetDuration(), Is.EqualTo("0s 000ms"));
            Assert.That(feature.GetDurationSortId(), Is.EqualTo("00:00:000"));
        }

        // ---------------------------------------------------------------------------
        // GetFolder
        // ---------------------------------------------------------------------------

        [Test]
        public void LivingDocFeature_GetFolder_ReturnsNull_WhenUriIsNull()
        {
            var feature = new LivingDocFeature();

            Assert.That(feature.GetFolder(), Is.Null);
        }

        [Test]
        public void LivingDocFeature_GetFolder_ReturnsNull_WhenUriIsWhitespace()
        {
            var feature = new LivingDocFeature { Uri = "   " };

            Assert.That(feature.GetFolder(), Is.Null);
        }

        [Test]
        public void LivingDocFeature_GetFolder_ReturnsNull_WhenUriHasNoDirectory()
        {
            var feature = new LivingDocFeature { Uri = "Login.feature" };

            Assert.That(feature.GetFolder(), Is.Null);
        }

        [Test]
        public void LivingDocFeature_GetFolder_ReturnsDirectory_WhenUriHasForwardSlashes()
        {
            var feature = new LivingDocFeature { Uri = "samples/login/Login.feature" };

            Assert.That(feature.GetFolder(), Is.EqualTo("samples\\login"));
        }

        [Test]
        public void LivingDocFeature_GetFolder_ReturnsDirectory_WhenUriHasSingleFolder()
        {
            var feature = new LivingDocFeature { Uri = "samples/Login.feature" };

            Assert.That(feature.GetFolder(), Is.EqualTo("samples"));
        }
    }
}
