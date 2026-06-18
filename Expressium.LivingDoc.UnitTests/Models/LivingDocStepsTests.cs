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

        [Test]
        public void LivingDocStep_IsIncomplete_IncompleteStatus_ReturnsTrue()
        {
            var step = new LivingDocStep { Status = LivingDocStatuses.Incomplete.ToString() };

            Assert.That(step.IsIncomplete(), Is.True);
            Assert.That(step.IsPassed(), Is.False);
            Assert.That(step.IsFailed(), Is.False);
            Assert.That(step.IsSkipped(), Is.False);
        }

        [Test]
        public void LivingDocStep_IsSkipped_SkippedStatus_ReturnsTrue()
        {
            var step = new LivingDocStep { Status = LivingDocStatuses.Skipped.ToString() };

            Assert.That(step.IsSkipped(), Is.True);
            Assert.That(step.IsPassed(), Is.False);
            Assert.That(step.IsFailed(), Is.False);
            Assert.That(step.IsIncomplete(), Is.False);
        }

        [Test]
        public void LivingDocStep_GetFullName_CombinesKeywordAndName()
        {
            var step = new LivingDocStep { Keyword = "Given", Name = "I am logged in" };

            Assert.That(step.GetFullName(), Is.EqualTo("Given I am logged in"));
        }

        [Test]
        public void LivingDocStep_GetDataStatus_ReturnsPrefixedStatus()
        {
            var step = new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() };

            Assert.That(step.GetDataStatus(), Is.EqualTo("@" + LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void LivingDocStep_GetDataStatus_NullStatus_ReturnsPrefixedSkipped()
        {
            var step = new LivingDocStep();

            Assert.That(step.GetDataStatus(), Is.EqualTo("@" + LivingDocStatuses.Skipped.ToString()));
        }

        [TestCase("Failed", "1")]
        [TestCase("Incomplete", "2")]
        [TestCase("Pending", "2")]
        [TestCase("Undefined", "2")]
        [TestCase("Ambiguous", "2")]
        [TestCase("Passed", "3")]
        [TestCase("Skipped", "4")]
        [TestCase("Unknown", "4")]
        public void LivingDocStep_GetStatusSortId_ReturnsCorrectRank(string status, string expected)
        {
            var step = new LivingDocStep { Status = status };

            Assert.That(step.GetStatusSortId(), Is.EqualTo(expected));
        }

        [Test]
        public void LivingDocStep_GetStatusSortId_NullStatus_ReturnsSkippedRank()
        {
            var step = new LivingDocStep();

            Assert.That(step.GetStatusSortId(), Is.EqualTo("4"));
        }
    }
}
