using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserExampleTablesUndefinedTests
    {
        [Test]
        public void Converting_ExampleTablesUndefined_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables-undefined", "examples-tables-undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(9));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Examples Tables - With Undefined Steps"));
        }

        [Test]
        public void Converting_ExampleTablesUndefined_FirstExample_UndefinedAtFirstStep()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables-undefined", "examples-tables-undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Eating cucumbers"));

            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(scenario.Examples[0].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void Converting_ExampleTablesUndefined_SecondExample_UndefinedAtSecondStep()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables-undefined", "examples-tables-undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[1].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[1].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
            Assert.That(scenario.Examples[1].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void Converting_ExampleTablesUndefined_ThirdExample_UndefinedAtThirdStep()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables-undefined", "examples-tables-undefined.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[2].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[2].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[2].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Undefined.ToString()));
        }
    }
}
