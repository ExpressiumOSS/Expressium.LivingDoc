using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocStepsTests
    {
        [Test]
        public void LivingDocSteps_GetStatus_Skipped()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocSteps.IsSkipped(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
            Assert.That(livingDocSteps.IsFailed(), Is.False);
            Assert.That(livingDocSteps.IsIncomplete(), Is.False);
        }

        [Test]
        public void LivingDocSteps_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[1].Examples[0].Steps[1];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocSteps.IsFailed(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
            Assert.That(livingDocSteps.IsSkipped(), Is.False);
            Assert.That(livingDocSteps.IsIncomplete(), Is.False);
            Assert.That(livingDocSteps.Message, Does.Contain("Validate the HomePage title property..."));
        }

        [Test]
        public void LivingDocSteps_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[2].Examples[0].Steps[2];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocSteps.IsIncomplete(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
            Assert.That(livingDocSteps.IsFailed(), Is.False);
            Assert.That(livingDocSteps.IsSkipped(), Is.False);
            Assert.That(livingDocSteps.Message, Is.EqualTo("Pending Step Definition"));
        }

        [Test]
        public void LivingDocSteps_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[3].Examples[0].Steps[0];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocSteps.IsPassed(), Is.True);
            Assert.That(livingDocSteps.IsFailed(), Is.False);
            Assert.That(livingDocSteps.IsIncomplete(), Is.False);
            Assert.That(livingDocSteps.IsSkipped(), Is.False);
            Assert.That(livingDocSteps.Message, Is.EqualTo(null));
        }

        [Test]
        public void LivingDocSteps_NullStatus_IsSkipped()
        {
            var livingDocStep = new LivingDocStep();

            Assert.That(livingDocStep.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocStep.IsSkipped(), Is.True);
            Assert.That(livingDocStep.IsPassed(), Is.False);
            Assert.That(livingDocStep.IsFailed(), Is.False);
            Assert.That(livingDocStep.IsIncomplete(), Is.False);
        }

        [Test]
        public void LivingDocStep_Copy_CopiesAllFields()
        {
            var original = new LivingDocStep
            {
                Id = "step-001",
                Keyword = "Given",
                Name = "I am logged in",
                Type = LivingDocStepTypes.Scenario.ToString(),
                Status = LivingDocStatuses.Passed.ToString(),
                Duration = new System.TimeSpan(0, 0, 0, 1, 250),
                Message = "Some message",
                DataTable = new LivingDocDataTable()
            };
            original.DataTable.Rows.Add(new LivingDocDataTableRow());

            var copy = new LivingDocStep().Copy(original);

            Assert.That(copy.Id, Is.EqualTo(original.Id));
            Assert.That(copy.Keyword, Is.EqualTo(original.Keyword));
            Assert.That(copy.Name, Is.EqualTo(original.Name));
            Assert.That(copy.Type, Is.EqualTo(original.Type));
            Assert.That(copy.Status, Is.EqualTo(original.Status));
            Assert.That(copy.Duration, Is.EqualTo(original.Duration));
            Assert.That(copy.Message, Is.EqualTo(original.Message));
            Assert.That(copy.DataTable, Is.SameAs(original.DataTable));
        }

        [Test]
        public void LivingDocStep_IsIncomplete_Pending_ReturnsTrue()
        {
            var step = new LivingDocStep { Status = LivingDocStatuses.Pending.ToString() };

            Assert.That(step.IsIncomplete(), Is.True);
            Assert.That(step.IsPassed(), Is.False);
            Assert.That(step.IsFailed(), Is.False);
            Assert.That(step.IsSkipped(), Is.False);
        }

        [Test]
        public void LivingDocStep_IsIncomplete_Undefined_ReturnsTrue()
        {
            var step = new LivingDocStep { Status = LivingDocStatuses.Undefined.ToString() };

            Assert.That(step.IsIncomplete(), Is.True);
            Assert.That(step.IsPassed(), Is.False);
            Assert.That(step.IsFailed(), Is.False);
            Assert.That(step.IsSkipped(), Is.False);
        }

        [Test]
        public void LivingDocStep_IsIncomplete_Ambiguous_ReturnsTrue()
        {
            var step = new LivingDocStep { Status = LivingDocStatuses.Ambiguous.ToString() };

            Assert.That(step.IsIncomplete(), Is.True);
            Assert.That(step.IsPassed(), Is.False);
            Assert.That(step.IsFailed(), Is.False);
            Assert.That(step.IsSkipped(), Is.False);
        }

        [Test]
        public void LivingDocStep_IsSkipped_UnknownStatus_ReturnsTrue()
        {
            var step = new LivingDocStep { Status = LivingDocStatuses.Unknown.ToString() };

            Assert.That(step.IsSkipped(), Is.True);
            Assert.That(step.IsPassed(), Is.False);
            Assert.That(step.IsFailed(), Is.False);
            Assert.That(step.IsIncomplete(), Is.False);
        }
    }
}
