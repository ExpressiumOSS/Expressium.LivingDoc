using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesParserAmbiguousTests
    {
        [Test]
        public void Converting_Ambiguous_Step_Definition()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "ambiguous.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("Ambiguous Step Definition..."));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Ambiguous.ToString()));
        }
    }
}
