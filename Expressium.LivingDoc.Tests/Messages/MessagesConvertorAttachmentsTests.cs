using Expressium.LivingDoc.Messages;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorAttachmentsTests
    {
        [Test]
        public void Converting_Scenario_Attachments()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "attachments.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(9));

            var scenario = livingDocProject.Features[0].Scenarios[8];

            Assert.That(scenario.Examples[0].Attachments[0], Is.EqualTo("https://cucumber.io"));
        }
    }
}
