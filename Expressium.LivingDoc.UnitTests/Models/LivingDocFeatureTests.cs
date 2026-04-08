using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocFeatureTests
    {
        [Test]
        public void LivingDocFeature_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "stack-traces.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0001"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0000"));
            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocFeature.IsFailed(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 003ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:003"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\stack-traces"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "pending.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0000"));
            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocFeature.IsIncomplete(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 013ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:013"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\pending"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0001"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(100));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0100"));
            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocFeature.IsPassed(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 003ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:003"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\minimal"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Skipped()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "empty.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0001"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0000"));
            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocFeature.IsSkipped(), Is.True);
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("0s 000ms"));
            Assert.That(livingDocFeature.GetDurationSortId(), Is.EqualTo("00:00:000"));
            Assert.That(livingDocFeature.GetFolder(), Is.EqualTo("samples\\empty"));
        }

        [Test]
        public void LivingDocFeature_GetNumberOfStatuses()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Contact Us"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(33));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0033"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(1));

            livingDocFeature = livingDocProject.Features[1];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Login"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0002"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0000"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(0));

            livingDocFeature = livingDocProject.Features[2];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Registration"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(67));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0067"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(2));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(0));

            livingDocFeature = livingDocProject.Features[3];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Orders"));
            Assert.That(livingDocFeature.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfScenariosSortId(), Is.EqualTo("0003"));
            Assert.That(livingDocFeature.GetPercentageOfPassed(), Is.EqualTo(100));
            Assert.That(livingDocFeature.GetPercentageOfPassedSortId(), Is.EqualTo("0100"));
            Assert.That(livingDocFeature.GetNumberOfPassedScenarios(), Is.EqualTo(3));
            Assert.That(livingDocFeature.GetNumberOfIncompleteScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfFailedScenarios(), Is.EqualTo(0));
            Assert.That(livingDocFeature.GetNumberOfSkippedScenarios(), Is.EqualTo(0));
        }
    }
}
