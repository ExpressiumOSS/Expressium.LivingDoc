using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserSkippedTests
    {
        [Test]
        public void Converting_Skipped_Step()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "skipped", "skipped.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(4));

            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Examples[0].Steps[1].Keyword, Is.EqualTo("And"));
            Assert.That(scenario.Examples[0].Steps[1].Name, Is.EqualTo("a step that is skipped"));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }
    }
}
