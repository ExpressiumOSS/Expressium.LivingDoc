using Expressium.LivingDoc.Messages;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorEmptyTests
    {
        [Test]
        public void Converting_Empty_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "empty.feature.ndjson");

            var messagesConvertor = new MessagesConvertor();
            var livingDocProject = messagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(0));
        }
    }
}
