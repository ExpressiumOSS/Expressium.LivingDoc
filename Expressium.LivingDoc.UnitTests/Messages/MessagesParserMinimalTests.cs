using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesParserMinimalTests
    {
        [Test]
        public void Converting_Minimal_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("cukes"));
            Assert.That(scenario.GetStatus, Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(scenario.Examples[0].Steps[0].Name, Is.EqualTo("I have 42 cukes in my belly"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }
    }
}
