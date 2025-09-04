using Expressium.LivingDoc.Messages;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesParserStackTracesTests
    {
        [Test]
        public void Converting_Step_StackTraces()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "stack-traces.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[0].Message, Is.EqualTo("BOOM\nsamples/stack-traces/stack-traces.feature:9"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo("Error"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("BOOM"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Is.EqualTo(null));
        }
    }
}
