using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserPendingTests
    {
        [Test]
        public void Converting_Pending_Step_Definition()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "pending.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(5));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[0].Message, Is.EqualTo("TODO"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo(null));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo(null));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Is.EqualTo(null));
        }

        [Test]
        public void Converting_Pending_Ambiguous_Step_Definition()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "step-notifications.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(4));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[1].Message, Is.EqualTo(null));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].ExceptionType, Is.EqualTo("PendingStepException"));
            Assert.That(scenario.Examples[0].Steps[1].ExceptionMessage, Is.EqualTo("The step definition is not implemented."));
            Assert.That(scenario.Examples[0].Steps[1].ExceptionStackTrace, Does.Contain("Reqnroll.Bindings.BindingInvoker"));

            scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Examples[0].Steps[1].Message, Is.EqualTo(null));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Ambiguous.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].ExceptionType, Is.EqualTo("AmbiguousBindingException"));
            Assert.That(scenario.Examples[0].Steps[1].ExceptionMessage, Does.Contain("Ambiguous step definitions found"));
            Assert.That(scenario.Examples[0].Steps[1].ExceptionStackTrace, Does.Contain("Reqnroll.Infrastructure.TestExecutionEngine"));
        }
    }
}
