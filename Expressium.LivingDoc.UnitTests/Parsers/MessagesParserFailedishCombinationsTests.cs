using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserFailedishCombinationsTests
    {
        [Test]
        public void Converting_FailedishCombinations_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "failedish-combinations", "failedish-combinations.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(27));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(3));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Failed-ish combinations"));
        }

        [Test]
        public void Converting_FailedishCombinations_Rules()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "failedish-combinations", "failedish-combinations.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Rules[0].Name, Is.EqualTo("Undefined and ambiguous steps can follow a failed-ish step"));
            Assert.That(feature.Rules[1].Name, Is.EqualTo("Failed and pending steps do not follow a failed-ish step"));
            Assert.That(feature.Rules[2].Name, Is.EqualTo("No pickle steps follow a skipped step"));
        }

        [Test]
        public void Converting_FailedishCombinations_PendingFirstStep()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "failedish-combinations", "failedish-combinations.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Pending as the first failed-ish step"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Ambiguous.ToString()));
        }

        [Test]
        public void Converting_FailedishCombinations_SkippedStep()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "failedish-combinations", "failedish-combinations.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[8];

            Assert.That(scenario.Name, Is.EqualTo("Step marks itself skipped"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }
    }
}
