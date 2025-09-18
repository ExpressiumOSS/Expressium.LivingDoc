using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserDataTablesTests
    {
        [Test]
        public void Converting_Step_DataTables()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "data-tables.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));

            var dataTable = livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].DataTable;

            Assert.That(dataTable.Rows.Count, Is.EqualTo(2));

            Assert.That(dataTable.Rows[0].Cells.Count, Is.EqualTo(2));
            Assert.That(dataTable.Rows[0].Cells[0], Is.EqualTo("a"));
            Assert.That(dataTable.Rows[0].Cells[1], Is.EqualTo("b"));

            Assert.That(dataTable.Rows[1].Cells.Count, Is.EqualTo(2));
            Assert.That(dataTable.Rows[1].Cells[0], Is.EqualTo("1"));
            Assert.That(dataTable.Rows[1].Cells[1], Is.EqualTo("2"));
        }
    }
}
