using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserUndefinedTests
    {
        [Test]
        public void Converting_Undefined_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "undefined", "undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(4));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(6));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Undefined steps"));
        }

        [Test]
        public void Converting_Undefined_Step_Causes_Failure()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "undefined", "undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("An undefined step causes a failure"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
        }

        [Test]
        public void Converting_Undefined_Steps_Before_Are_Executed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "undefined", "undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Name, Is.EqualTo("Steps before undefined steps are executed"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
        }

        [Test]
        public void Converting_Undefined_Steps_After_Are_Skipped()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "undefined", "undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[2];

            Assert.That(scenario.Name, Is.EqualTo("Steps after undefined steps are skipped"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }
    }
}
