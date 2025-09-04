using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesParserSkippedTests
    {
        [Test]
        public void Converting_Skipped_Step()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "skipped.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(5));

            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Examples[0].Steps[1].Keyword, Is.EqualTo("And"));
            Assert.That(scenario.Examples[0].Steps[1].Name, Is.EqualTo("I skip a step"));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }
    }
}
