using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserAttachmentsTests
    {
        [Test]
        public void Converting_Scenario_Attachments()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "attachments", "attachments.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(8));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(8));

            var scenario = livingDocProject.Features[0].Scenarios[6];

            Assert.That(scenario.Examples[0].Attachments[0], Is.EqualTo("https://cucumber.io"));
        }
    }
}
