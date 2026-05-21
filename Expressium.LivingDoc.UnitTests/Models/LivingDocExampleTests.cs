using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocExampleTests
    {
        [Test]
        public void LivingDocExample_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "stack-traces", "stack-traces.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocExample.IsFailed(), Is.True);
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("0s 003ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocExample_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "pending", "pending.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocExample.IsIncomplete(), Is.True);
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("0s 003ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocExample_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables", "examples-tables.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocExample.IsPassed(), Is.True);
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("0s 007ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.True);
        }

        [Test]
        public void LivingDocExample_GetStatus_Skipped()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "empty", "empty.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocExample.IsSkipped(), Is.True);
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("0s 000ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocExample_Constructor_DefaultsAreCorrect()
        {
            var example = new LivingDocExample();

            Assert.That(example.Duration, Is.EqualTo(System.TimeSpan.Zero));
            Assert.That(example.Steps, Is.Not.Null);
            Assert.That(example.Steps, Is.Empty);
            Assert.That(example.DataTable, Is.Not.Null);
            Assert.That(example.DataTable.Rows, Is.Empty);
            Assert.That(example.Attachments, Is.Not.Null);
            Assert.That(example.Attachments, Is.Empty);
            Assert.That(example.History, Is.Not.Null);
            Assert.That(example.History, Is.Empty);
        }

        [Test]
        public void LivingDocExample_GetStatus_Skipped_WhenStepsListIsEmpty()
        {
            var example = new LivingDocExample();

            Assert.That(example.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(example.IsSkipped(), Is.True);
            Assert.That(example.IsPassed(), Is.False);
            Assert.That(example.IsFailed(), Is.False);
            Assert.That(example.IsIncomplete(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasDataTable_ReturnsFalse_WhenNoRows()
        {
            var example = new LivingDocExample();

            Assert.That(example.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasDataTable_ReturnsTrue_WhenRowsExist()
        {
            var example = new LivingDocExample();
            example.DataTable.Rows.Add(new LivingDocDataTableRow());

            Assert.That(example.HasDataTable(), Is.True);
        }

        [Test]
        public void LivingDocExample_GetStatus_Passed_WhenAllStepsPassed()
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });

            Assert.That(example.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(example.IsPassed(), Is.True);
        }

        [Test]
        public void LivingDocExample_GetStatus_Failed_WhenAnyStepFailed()
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Failed.ToString() });

            Assert.That(example.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(example.IsFailed(), Is.True);
        }

        [Test]
        public void LivingDocExample_GetStatus_Incomplete_WhenAnyStepIncomplete()
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Incomplete.ToString() });

            Assert.That(example.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(example.IsIncomplete(), Is.True);
        }
    }
}
