using Expressium.LivingDoc.Messages;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorBackgroundTests
    {
        [Test]
        public void Converting_Feature_Background()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "background.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(13));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Background.Steps[0].Name, Is.EqualTo("I have $500 in my checking account"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have $500 in my checking account"));
        }
    }
}
