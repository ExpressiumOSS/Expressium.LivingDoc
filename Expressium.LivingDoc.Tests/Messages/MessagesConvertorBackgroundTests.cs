using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorBackgroundTests
    {
        [Test]
        public void Converting_Feature_Background()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "background.feature.ndjson");

            var messagesConvertor = new MessagesConvertor();
            var livingDocProject = messagesConvertor.ConvertToLivingDoc(inputFilePath);

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

            var messagesConvertor = new MessagesConvertor();
            var livingDocProject = messagesConvertor.ConvertToLivingDoc(inputFilePath);

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
    }
}
