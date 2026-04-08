using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocDataTableTests
    {
        [Test]
        public void LivingDocDataTable_Constructor_InitialisesRowsAsEmptyList()
        {
            var dataTable = new LivingDocDataTable();

            Assert.That(dataTable.Rows, Is.Not.Null);
            Assert.That(dataTable.Rows, Is.Empty);
        }

        [Test]
        public void LivingDocDataTable_Rows_CanAddAndRetrieveRows()
        {
            var dataTable = new LivingDocDataTable();
            dataTable.Rows.Add(new LivingDocDataTableRow());
            dataTable.Rows.Add(new LivingDocDataTableRow());

            Assert.That(dataTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocDataTable_Rows_CanAddRowsWithCells()
        {
            var dataTable = new LivingDocDataTable();

            var row = new LivingDocDataTableRow();
            row.Cells.Add("Column1");
            row.Cells.Add("Column2");
            dataTable.Rows.Add(row);

            Assert.That(dataTable.Rows[0].Cells[0], Is.EqualTo("Column1"));
            Assert.That(dataTable.Rows[0].Cells[1], Is.EqualTo("Column2"));
        }
    }
}
