using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserRetryVariantsTests
    {
        [Test]
        public void Converting_RetryAmbiguous_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "retry-ambiguous", "retry-ambiguous.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Retry - With Ambiguous Steps"));
        }

        [Test]
        public void Converting_RetryAmbiguous_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "retry-ambiguous", "retry-ambiguous.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Test cases won't retry when the status is AMBIGUOUS"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Ambiguous.ToString()));
        }

        [Test]
        public void Converting_RetryPending_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "retry-pending", "retry-pending.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Retry - With Pending Steps"));
        }

        [Test]
        public void Converting_RetryPending_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "retry-pending", "retry-pending.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Test cases won't retry when the status is PENDING"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
        }

        [Test]
        public void Converting_RetryUndefined_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "retry-undefined", "retry-undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Retry - With Undefined Steps"));
        }

        [Test]
        public void Converting_RetryUndefined_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "retry-undefined", "retry-undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Test cases won't retry when the status is UNDEFINED"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
        }
    }
}
