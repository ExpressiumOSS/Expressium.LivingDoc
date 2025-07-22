using Expressium.LivingDoc.Models;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocScenarioTests
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
        public void LivingDocScenario_GetStatus_Failed()
        {
            var livingDocScenario = livingDocProject.Features[1].Scenarios[0];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1001 @Done"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("3s 042ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:03:042"));
            Assert.That(livingDocScenario.GetNumberOfSteps(), Is.EqualTo(2));
            Assert.That(livingDocScenario.GetNumberOfStepsSortId(), Is.EqualTo("0002"));
        }

        [Test]
        public void LivingDocScenario_GetStatus_Incomplete()
        {
            var livingDocScenario = livingDocProject.Features[1].Scenarios[1];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1002 @Review"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("3s 103ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:03:103"));
            Assert.That(livingDocScenario.GetNumberOfSteps(), Is.EqualTo(2));
            Assert.That(livingDocScenario.GetNumberOfStepsSortId(), Is.EqualTo("0002"));
        }

        [Test]
        public void LivingDocScenario_GetStatus_Passed()
        {
            var livingDocScenario = livingDocProject.Features[2].Scenarios[0];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-4001 @Review"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("8s 715ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:08:715"));
            Assert.That(livingDocScenario.GetNumberOfSteps(), Is.EqualTo(6));
            Assert.That(livingDocScenario.GetNumberOfStepsSortId(), Is.EqualTo("0006"));
        }
    }
}
