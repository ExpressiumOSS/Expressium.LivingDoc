using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocScenarioTests
    {
        [Test]
        public void LivingDocScenario_GetStatus_Skipped()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1002 @Ignored"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("0s 000ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:00:000"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1003 @Done"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("3s 478ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:03:478"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[2];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1004 @Done"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("5s 641ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:05:641"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[3];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1005 @Review"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("4s 495ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:04:495"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_HasDataTable()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[4];

            Assert.That(livingDocScenario.Name, Is.EqualTo("Ordering Coffee Confirmation Notification"));
            Assert.That(livingDocScenario.HasDataTable(), Is.True);
        }
    }
}
