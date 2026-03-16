using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserHooksErrorsTests
    {
        [Test]
        public void Converting_HooksBeforeScenarioFailure()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "HookFailures.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var feature = livingDocProject.Features[0];
            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo("ApplicationException"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("Hook Before Scenario faling..."));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Does.Contain("BeforeScenario"));
        }

        [Test]
        public void Converting_HooksBeforeStepFailure()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "HookFailures.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var feature = livingDocProject.Features[0];
            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo("ApplicationException"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("Hook Before Step faling..."));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Does.Contain("BeforeStep"));
        }

        // Currently not inplemented correctly in LivingDoc...
        [Test]
        public void Converting_HooksAfterStepFailure()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "HookFailures.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var feature = livingDocProject.Features[0];
            var scenario = livingDocProject.Features[0].Scenarios[2];

            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo(null));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo(null));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Is.EqualTo(null));
        }

        [Test]
        public void Converting_HooksAfterScenarioFailure()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "HookFailures.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var feature = livingDocProject.Features[0];
            var scenario = livingDocProject.Features[0].Scenarios[3];

            Assert.That(scenario.Examples[0].Steps[2].ExceptionType, Is.EqualTo("ApplicationException"));
            Assert.That(scenario.Examples[0].Steps[2].ExceptionMessage, Is.EqualTo("Hook After Scenario faling..."));
            Assert.That(scenario.Examples[0].Steps[2].ExceptionStackTrace, Does.Contain("AfterScenario"));
        }
    }
}
