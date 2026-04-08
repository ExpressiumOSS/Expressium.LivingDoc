using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocDataTableRowTests
    {
        [Test]
        public void LivingDocDataTableRow_Constructor_InitialisesCellsAsEmptyList()
        {
            var row = new LivingDocDataTableRow();

            Assert.That(row.Cells, Is.Not.Null);
            Assert.That(row.Cells, Is.Empty);
        }

        [Test]
        public void LivingDocDataTableRow_Cells_CanAddAndRetrieveCells()
        {
            var row = new LivingDocDataTableRow();
            row.Cells.Add("Alice");
            row.Cells.Add("30");
            row.Cells.Add("Engineer");

            Assert.That(row.Cells.Count, Is.EqualTo(3));
            Assert.That(row.Cells[0], Is.EqualTo("Alice"));
            Assert.That(row.Cells[1], Is.EqualTo("30"));
            Assert.That(row.Cells[2], Is.EqualTo("Engineer"));
        }

        [Test]
        public void LivingDocDataTableRow_Cells_CanContainEmptyStringValues()
        {
            var row = new LivingDocDataTableRow();
            row.Cells.Add("");
            row.Cells.Add("Value");
            row.Cells.Add("");

            Assert.That(row.Cells.Count, Is.EqualTo(3));
            Assert.That(row.Cells[0], Is.EqualTo(""));
            Assert.That(row.Cells[2], Is.EqualTo(""));
        }
    }
}
