using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserSkippedExceptionTests
    {
        [Test]
        public void Converting_SkippedException_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "skipped-exception", "skipped-exception.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Skipping scenarios via exception"));
        }

        [Test]
        public void Converting_SkippedException_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "skipped-exception", "skipped-exception.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Skipping via an exception"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(scenario.Examples[0].Steps[0].Name, Is.EqualTo("I skip a step"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }
    }
}
