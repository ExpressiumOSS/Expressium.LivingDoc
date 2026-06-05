using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocEnumerationsTests
    {
        // LivingDocStatuses

        [Test]
        public void LivingDocStatuses_Failed_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Failed.ToString(), Is.EqualTo("Failed"));
        }

        [Test]
        public void LivingDocStatuses_Incomplete_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Incomplete.ToString(), Is.EqualTo("Incomplete"));
        }

        [Test]
        public void LivingDocStatuses_Pending_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Pending.ToString(), Is.EqualTo("Pending"));
        }

        [Test]
        public void LivingDocStatuses_Undefined_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Undefined.ToString(), Is.EqualTo("Undefined"));
        }

        [Test]
        public void LivingDocStatuses_Ambiguous_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Ambiguous.ToString(), Is.EqualTo("Ambiguous"));
        }

        [Test]
        public void LivingDocStatuses_Passed_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Passed.ToString(), Is.EqualTo("Passed"));
        }

        [Test]
        public void LivingDocStatuses_Skipped_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Skipped.ToString(), Is.EqualTo("Skipped"));
        }

        [Test]
        public void LivingDocStatuses_Unknown_StringValue_IsExpected()
        {
            Assert.That(LivingDocStatuses.Unknown.ToString(), Is.EqualTo("Unknown"));
        }

        [Test]
        public void LivingDocStatuses_ContainsExactlyEightValues()
        {
            var values = System.Enum.GetValues(typeof(LivingDocStatuses));

            Assert.That(values.Length, Is.EqualTo(8));
        }

        [Test]
        public void LivingDocHealths_New_StringValue_IsExpected()
        {
            Assert.That(LivingDocHealths.New.ToString(), Is.EqualTo("New"));
        }

        [Test]
        public void LivingDocHealths_Broken_StringValue_IsExpected()
        {
            Assert.That(LivingDocHealths.Broken.ToString(), Is.EqualTo("Broken"));
        }

        [Test]
        public void LivingDocHealths_Regressed_StringValue_IsExpected()
        {
            Assert.That(LivingDocHealths.Regressed.ToString(), Is.EqualTo("Regressed"));
        }

        [Test]
        public void LivingDocHealths_Flaky_StringValue_IsExpected()
        {
            Assert.That(LivingDocHealths.Flaky.ToString(), Is.EqualTo("Flaky"));
        }

        [Test]
        public void LivingDocHealths_Fixed_StringValue_IsExpected()
        {
            Assert.That(LivingDocHealths.Fixed.ToString(), Is.EqualTo("Fixed"));
        }

        [Test]
        public void LivingDocHealths_Invalid_StringValue_IsExpected()
        {
            Assert.That(LivingDocHealths.Invalid.ToString(), Is.EqualTo("Invalid"));
        }

        [Test]
        public void LivingDocHealths_ContainsExactlySixValues()
        {
            var values = System.Enum.GetValues(typeof(LivingDocHealths));

            Assert.That(values.Length, Is.EqualTo(6));
        }

        [Test]
        public void LivingDocStepTypes_Hook_StringValue_IsExpected()
        {
            Assert.That(LivingDocStepTypes.Hook.ToString(), Is.EqualTo("Hook"));
        }

        [Test]
        public void LivingDocStepTypes_Background_StringValue_IsExpected()
        {
            Assert.That(LivingDocStepTypes.Background.ToString(), Is.EqualTo("Background"));
        }

        [Test]
        public void LivingDocStepTypes_Rule_StringValue_IsExpected()
        {
            Assert.That(LivingDocStepTypes.Rule.ToString(), Is.EqualTo("Rule"));
        }

        [Test]
        public void LivingDocStepTypes_Scenario_StringValue_IsExpected()
        {
            Assert.That(LivingDocStepTypes.Scenario.ToString(), Is.EqualTo("Scenario"));
        }

        [Test]
        public void LivingDocStepTypes_Unknown_StringValue_IsExpected()
        {
            Assert.That(LivingDocStepTypes.Unknown.ToString(), Is.EqualTo("Unknown"));
        }

        [Test]
        public void LivingDocStepTypes_ContainsExactlyFiveValues()
        {
            var values = System.Enum.GetValues(typeof(LivingDocStepTypes));

            Assert.That(values.Length, Is.EqualTo(5));
        }
    }
}
