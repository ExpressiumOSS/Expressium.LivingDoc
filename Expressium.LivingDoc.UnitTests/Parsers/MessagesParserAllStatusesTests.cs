using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserAllStatusesTests
    {
        [Test]
        public void Converting_AllStatuses_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "all-statuses", "all-statuses.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(6));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(18));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("All statuses"));
        }

        [Test]
        public void Converting_AllStatuses_Passing_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "all-statuses", "all-statuses.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Passing"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void Converting_AllStatuses_Failing_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "all-statuses", "all-statuses.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Name, Is.EqualTo("Failing"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void Converting_AllStatuses_Pending_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "all-statuses", "all-statuses.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[2];

            Assert.That(scenario.Name, Is.EqualTo("Pending"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void Converting_AllStatuses_Skipped_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "all-statuses", "all-statuses.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[3];

            Assert.That(scenario.Name, Is.EqualTo("Skipped"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void Converting_AllStatuses_Undefined_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "all-statuses", "all-statuses.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[4];

            Assert.That(scenario.Name, Is.EqualTo("Undefined"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void Converting_AllStatuses_Ambiguous_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "all-statuses", "all-statuses.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[5];

            Assert.That(scenario.Name, Is.EqualTo("Ambiguous"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Ambiguous.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }
    }
}
