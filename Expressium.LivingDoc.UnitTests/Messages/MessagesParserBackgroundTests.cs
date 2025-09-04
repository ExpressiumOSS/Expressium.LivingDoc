using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesParserBackgroundTests
    {
        [Test]
        public void Converting_Feature_Background()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "background.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(13));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Background.Steps[0].Name, Is.EqualTo("I have $500 in my checking account"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have $500 in my checking account"));
        }

        [Test]
        public void Converting_Feature_Backgrounds()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "backgrounds.features.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(4));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Background.Steps[0].Name, Is.EqualTo("I have an initial step running"));

            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have an initial step running"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(feature.Scenarios[1].Examples[0].Steps[0].Name, Is.EqualTo("I have an initial step running"));
            Assert.That(feature.Scenarios[1].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
        }

        [Test]
        public void Converting_Feature_Backgrounds_Examples()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "backgrounds-examples.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(4));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Background.Steps[0].Name, Is.EqualTo("I have an initial background step running"));

            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have an initial background step running"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(feature.Scenarios[0].Examples[1].Steps[0].Name, Is.EqualTo("I have an initial background step running"));
            Assert.That(feature.Scenarios[0].Examples[1].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
        }

        [Test]
        public void Converting_Feature_Backgrounds_Mixed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "backgrounds-mixed.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(15));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Background.Steps[0].Name, Is.EqualTo("I have an initial background step running"));

            Assert.That(feature.Scenarios[0].Name, Is.EqualTo("Scenario without Examples and with Background Step"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have an initial background step running"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(feature.Scenarios[1].Name, Is.EqualTo("Scenario with Examples both Passing and Failing Background Steps"));
            Assert.That(feature.Scenarios[1].Examples[0].Steps[0].Name, Is.EqualTo("I have an initial background step running"));
            Assert.That(feature.Scenarios[1].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(feature.Scenarios[1].Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(feature.Scenarios[1].Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));

            Assert.That(feature.Scenarios[1].Examples[1].Steps[0].Name, Is.EqualTo("I have an initial background step running"));
            Assert.That(feature.Scenarios[1].Examples[1].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
            Assert.That(feature.Scenarios[1].Examples[1].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(feature.Scenarios[1].Examples[1].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));

            Assert.That(feature.Scenarios[1].Examples[2].Steps[0].Name, Is.EqualTo("I have an initial background step running"));
            Assert.That(feature.Scenarios[1].Examples[2].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(feature.Scenarios[1].Examples[2].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(feature.Scenarios[1].Examples[2].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(feature.Scenarios[1].Examples[3].Steps[0].Name, Is.EqualTo("I have an initial background step running"));
            Assert.That(feature.Scenarios[1].Examples[3].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(feature.Scenarios[1].Examples[3].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(feature.Scenarios[1].Examples[3].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }
    }
}
