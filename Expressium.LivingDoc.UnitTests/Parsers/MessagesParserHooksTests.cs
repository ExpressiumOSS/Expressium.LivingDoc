using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserHooksTests
    {
        [Test]
        public void Converting_Hooks_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "hooks", "hooks.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Hooks"));
        }

        [Test]
        public void Converting_Hooks_PassedScenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "hooks", "hooks.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("No tags and a passed step"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void Converting_Hooks_FailedScenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "hooks", "hooks.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Name, Is.EqualTo("No tags and a failed step"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
        }
    }
}
