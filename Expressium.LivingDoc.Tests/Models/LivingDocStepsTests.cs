using Expressium.LivingDoc.Models;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocStepsTests
    {
        private LivingDocProject livingDocProject;

        [OneTimeSetUp]
        public void OnTimeSetup()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");

            livingDocProject = LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputFilePath);
            livingDocProject.Features = livingDocProject.Features.OrderBy(f => f.Name).ToList();
        }

        [Test]
        public void LivingDocSteps_GetStatus_Failed()
        {
            var livingDocSteps = livingDocProject.Features[1].Scenarios[0].Examples[0].Steps[1];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocSteps.IsFailed(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
            Assert.That(livingDocSteps.Message, Does.Contain("Validate the HomePage title property..."));
        }

        [Test]
        public void LivingDocSteps_GetStatus_Incomplete()
        {
            var livingDocSteps = livingDocProject.Features[1].Scenarios[1].Examples[0].Steps[1];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocSteps.IsIncomplete(), Is.True);
            Assert.That(livingDocSteps.IsPassed(), Is.False);
            Assert.That(livingDocSteps.Message, Is.EqualTo("Pending Step Definition"));
        }

        [Test]
        public void LivingDocSteps_GetStatus_Passed()
        {
            var livingDocSteps = livingDocProject.Features[1].Scenarios[0].Examples[0].Steps[0];

            Assert.That(livingDocSteps.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocSteps.IsIncomplete(), Is.False);
            Assert.That(livingDocSteps.IsPassed(), Is.True);
            Assert.That(livingDocSteps.Message, Is.EqualTo(null));
        }
    }
}
