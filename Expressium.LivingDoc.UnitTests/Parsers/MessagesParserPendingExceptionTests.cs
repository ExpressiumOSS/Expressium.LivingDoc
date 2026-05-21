using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserPendingExceptionTests
    {
        [Test]
        public void Converting_PendingException_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "pending-exception", "pending-exception.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Pending steps via exception"));
        }

        [Test]
        public void Converting_PendingException_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "pending-exception", "pending-exception.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Pending via an exception"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(scenario.Examples[0].Steps[0].Name, Is.EqualTo("an unimplemented pending step"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
        }
    }
}
