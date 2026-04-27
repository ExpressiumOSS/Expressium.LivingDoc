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

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1002 @Ignored"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocScenario.IsSkipped(), Is.True);
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("0s 000ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:00:000"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1003 @Done"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocScenario.IsFailed(), Is.True);
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("3s 478ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:03:478"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[2];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1004 @Done"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocScenario.IsIncomplete(), Is.True);
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("5s 641ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:05:641"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[3];

            Assert.That(livingDocScenario.Id, Is.Not.Null);
            Assert.That(livingDocScenario.GetTags(), Is.EqualTo("@TA-1005 @Review"));
            Assert.That(livingDocScenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocScenario.IsPassed(), Is.True);
            Assert.That(livingDocScenario.GetDuration(), Is.EqualTo("4s 495ms"));
            Assert.That(livingDocScenario.GetDurationSortId(), Is.EqualTo("00:04:495"));
            Assert.That(livingDocScenario.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocScenario_HasDataTable()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            var livingDocScenario = livingDocProject.Features[0].Scenarios[4];

            Assert.That(livingDocScenario.Name, Is.EqualTo("Ordering Coffee Confirmation Notification"));
            Assert.That(livingDocScenario.HasDataTable(), Is.True);
        }

        [Test]
        public void LivingDocScenario_GetNumberOfExamples_ByStatus()
        {
            var scenario = new LivingDocScenario();

            var passedExample = new LivingDocExample();
            passedExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });

            var failedExample = new LivingDocExample();
            failedExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Failed.ToString() });

            var incompleteExample = new LivingDocExample();
            incompleteExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Incomplete.ToString() });

            var skippedExample = new LivingDocExample();

            scenario.Examples.Add(passedExample);
            scenario.Examples.Add(failedExample);
            scenario.Examples.Add(incompleteExample);
            scenario.Examples.Add(skippedExample);

            Assert.That(scenario.GetNumberOfPassedExamples(), Is.EqualTo(1));
            Assert.That(scenario.GetNumberOfFailedExamples(), Is.EqualTo(1));
            Assert.That(scenario.GetNumberOfIncompleteExamples(), Is.EqualTo(1));
            Assert.That(scenario.GetNumberOfSkippedExamples(), Is.EqualTo(1));
        }

        [Test]
        public void LivingDocScenario_GetNumberOfSteps_ByStatus()
        {
            var scenario = new LivingDocScenario();

            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Failed.ToString() });
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Incomplete.ToString() });
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Skipped.ToString() });
            scenario.Examples.Add(example);

            Assert.That(scenario.GetNumberOfPassedSteps(), Is.EqualTo(2));
            Assert.That(scenario.GetNumberOfFailedSteps(), Is.EqualTo(1));
            Assert.That(scenario.GetNumberOfIncompleteSteps(), Is.EqualTo(1));
            Assert.That(scenario.GetNumberOfSkippedSteps(), Is.EqualTo(1));
        }

        [Test]
        public void LivingDocScenario_GetOrder_ReturnsDefaultZero()
        {
            var scenario = new LivingDocScenario();

            Assert.That(scenario.GetOrder(), Is.EqualTo(0));
            Assert.That(scenario.GetOrderSortId(), Is.EqualTo("0000"));
        }

        [TestCase(0, "0000")]
        [TestCase(1, "0001")]
        [TestCase(42, "0042")]
        [TestCase(999, "0999")]
        [TestCase(1000, "1000")]
        public void LivingDocScenario_GetOrderSortId_FormatsAsExpected(int order, string expected)
        {
            var scenario = new LivingDocScenario { Order = order };

            Assert.That(scenario.GetOrder(), Is.EqualTo(order));
            Assert.That(scenario.GetOrderSortId(), Is.EqualTo(expected));
        }

        [Test]
        public void LivingDocScenario_HasHealth_ReturnsFalse_WhenHealthIsNull()
        {
            var scenario = new LivingDocScenario();

            Assert.That(scenario.HasHealth(), Is.False);
        }

        [TestCase("Broken")]
        [TestCase("Flaky")]
        [TestCase("Fixed")]
        public void LivingDocScenario_HasHealth_ReturnsTrue_WhenHealthIsSet(string health)
        {
            var scenario = new LivingDocScenario { Health = health };

            Assert.That(scenario.HasHealth(), Is.True);
        }

        [Test]
        public void LivingDocScenario_HasHealth_ReturnsFalse_AfterClearingHealth()
        {
            var scenario = new LivingDocScenario { Health = LivingDocHealths.Broken.ToString() };

            Assert.That(scenario.HasHealth(), Is.True);

            scenario.Health = null;

            Assert.That(scenario.HasHealth(), Is.False);
        }

        [Test]
        public void LivingDocScenario_GetTags_WithoutHealthOrTags_ReturnsEmptyString()
        {
            var scenario = new LivingDocScenario();

            Assert.That(scenario.GetTags(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void LivingDocScenario_GetTags_WithoutHealth_ReturnsTagsOnly()
        {
            var scenario = new LivingDocScenario();
            scenario.Tags.Add("@TA-1001");
            scenario.Tags.Add("@Done");

            Assert.That(scenario.GetTags(), Is.EqualTo("@TA-1001 @Done"));
        }

        [Test]
        public void LivingDocScenario_GetTags_WithHealth_NoOtherTags()
        {
            var scenario = new LivingDocScenario { Health = LivingDocHealths.Broken.ToString() };

            Assert.That(scenario.GetTags(), Is.EqualTo("@Broken"));
        }

        [Test]
        public void LivingDocScenario_GetTags_WithHealth_AndExistingTags()
        {
            var scenario = new LivingDocScenario { Health = LivingDocHealths.Flaky.ToString() };
            scenario.Tags.Add("@TA-1001");
            scenario.Tags.Add("@Done");

            Assert.That(scenario.GetTags(), Is.EqualTo("@TA-1001 @Done @Flaky"));
        }

        [TestCase("Broken", "@Broken")]
        [TestCase("Flaky", "@Flaky")]
        [TestCase("Fixed", "@Fixed")]
        public void LivingDocScenario_GetTags_WithHealth_AllHealthValues(string health, string expected)
        {
            var scenario = new LivingDocScenario { Health = health };

            Assert.That(scenario.GetTags(), Is.EqualTo(expected));
        }
        [Test]
        public void LivingDocScenario_IsBroken_ReturnsTrue_WhenHealthIsBroken()
        {
            var scenario = new LivingDocScenario { Health = LivingDocHealths.Broken.ToString() };

            Assert.That(scenario.IsBroken(), Is.True);
            Assert.That(scenario.IsFlaky(), Is.False);
            Assert.That(scenario.IsFixed(), Is.False);
        }

        [Test]
        public void LivingDocScenario_IsFlaky_ReturnsTrue_WhenHealthIsFlaky()
        {
            var scenario = new LivingDocScenario { Health = LivingDocHealths.Flaky.ToString() };

            Assert.That(scenario.IsBroken(), Is.False);
            Assert.That(scenario.IsFlaky(), Is.True);
            Assert.That(scenario.IsFixed(), Is.False);
        }

        [Test]
        public void LivingDocScenario_IsFixed_ReturnsTrue_WhenHealthIsFixed()
        {
            var scenario = new LivingDocScenario { Health = LivingDocHealths.Fixed.ToString() };

            Assert.That(scenario.IsBroken(), Is.False);
            Assert.That(scenario.IsFlaky(), Is.False);
            Assert.That(scenario.IsFixed(), Is.True);
        }

        [Test]
        public void LivingDocScenario_HealthHelpers_AllReturnFalse_WhenHealthIsNull()
        {
            var scenario = new LivingDocScenario();

            Assert.That(scenario.IsBroken(), Is.False);
            Assert.That(scenario.IsFlaky(), Is.False);
            Assert.That(scenario.IsFixed(), Is.False);
        }
    }
}
