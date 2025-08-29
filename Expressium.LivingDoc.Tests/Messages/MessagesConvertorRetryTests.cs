using Expressium.LivingDoc.Messages;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorRetryTests
    {
        [Test]
        public void Converting_Retry_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "retry.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(5));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Uri, Is.EqualTo("samples/retry/retry.feature"));

            var scenario = livingDocProject.Features[0].Scenarios[3];

            Assert.That(scenario.Examples[0].Steps[0].Message, Is.EqualTo("Exception in step\nsamples/retry/retry.feature:18"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo("Error"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("Exception in step"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Is.EqualTo(null));

            Assert.That(feature.Scenarios[0].Order, Is.EqualTo(1));
            Assert.That(feature.Scenarios[1].Order, Is.EqualTo(2));
            Assert.That(feature.Scenarios[2].Order, Is.EqualTo(3));
            Assert.That(feature.Scenarios[3].Order, Is.EqualTo(4));
            Assert.That(feature.Scenarios[4].Order, Is.EqualTo(5));
        }
    }
}
