using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorHooksErrorsTests
    {
        [Test]
        public void Converting_Scenario_HooksErrors()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "hooks-errors.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(3));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Uri, Is.EqualTo("samples/hooks-conditional/hooks-conditional.feature"));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            // TODO - Missing implementation and validation of hook errors...
            //Assert.That(scenario.ExceptionType, Is.EqualTo("Error"));
            //Assert.That(scenario.ExceptionMessage, Is.EqualTo("BOOM"));
            //Assert.That(scenario..ExceptionStackTrace, Is.EqualTo("xxx"));
        }
    }
}
