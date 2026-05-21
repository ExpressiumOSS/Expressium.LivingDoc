using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserExampleTablesAttachmentTests
    {
        [Test]
        public void Converting_ExampleTablesAttachment_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables-attachment", "examples-tables-attachment.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Examples Tables - With attachments"));
        }

        [Test]
        public void Converting_ExampleTablesAttachment_Scenario()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables-attachment", "examples-tables-attachment.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("Attaching images in an examples table"));
            Assert.That(scenario.HasDataTable(), Is.True);

            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(scenario.Examples[1].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void Converting_ExampleTablesAttachment_DataTable()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables-attachment", "examples-tables-attachment.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells[0], Is.EqualTo("type"));
            Assert.That(scenario.Examples[0].DataTable.Rows[1].Cells[0], Is.EqualTo("JPEG"));
        }
    }
}
