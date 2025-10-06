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

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocSteps.IsSkipped(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
        }

        [Test]
        public void LivingDocSteps_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[1].Examples[0].Steps[1];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocSteps.IsFailed(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
            Assert.That(livingDocSteps.Message, Does.Contain("Validate the HomePage title property..."));
        }

        [Test]
        public void LivingDocSteps_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[2].Examples[0].Steps[2];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocSteps.IsIncomplete(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
            Assert.That(livingDocSteps.Message, Is.EqualTo("Pending Step Definition"));
        }

        [Test]
        public void LivingDocSteps_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocSteps = livingDocProject.Features[0].Scenarios[3].Examples[0].Steps[0];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocSteps.IsIncomplete(), Is.False);
            Assert.That(livingDocSteps.IsPassed(), Is.True);
            Assert.That(livingDocSteps.Message, Is.EqualTo(null));
        }
    }
}
