using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesParserExampleTablesTests
    {
        [Test]
        public void Converting_Scenario_ExampleTables()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(27));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].DataTable.Rows.Count, Is.EqualTo(2));
            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells.Count, Is.EqualTo(3));

            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells[0], Is.EqualTo("start"));
            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells[1], Is.EqualTo("eat"));
            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells[2], Is.EqualTo("left"));

            Assert.That(scenario.Examples[0].DataTable.Rows[1].Cells[0], Is.EqualTo("12"));
            Assert.That(scenario.Examples[0].DataTable.Rows[1].Cells[1], Is.EqualTo("5"));
            Assert.That(scenario.Examples[0].DataTable.Rows[1].Cells[2], Is.EqualTo("7"));

            Assert.That(scenario.Examples[2].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(scenario.Examples[2].Steps[2].ExceptionType, Is.EqualTo("AssertionError"));
            Assert.That(scenario.Examples[2].Steps[2].ExceptionMessage, Does.Contain("Expected values to be strictly equal"));
        }
    }
}
