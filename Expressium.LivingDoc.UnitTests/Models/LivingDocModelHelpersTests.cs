using Expressium.LivingDoc.Models;
using System;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocModelHelpersTests
    {
        private static LivingDocExample CreateExample(TimeSpan duration)
        {
            return new LivingDocExample { Duration = duration };
        }

        // LivingDocExample.HasStacktraces...

        [Test]
        public void LivingDocExample_HasStacktraces_ReturnsFalse_WhenNoSteps()
        {
            Assert.That(new LivingDocExample().HasStacktraces(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasStacktraces_ReturnsFalse_WhenStepsIsNull()
        {
            Assert.That(new LivingDocExample { Steps = null }.HasStacktraces(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasStacktraces_ReturnsFalse_WhenNoStepHasStacktrace()
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = "Failed", ExceptionMessage = "Boom" });

            Assert.That(example.HasStacktraces(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasStacktraces_ReturnsTrue_WhenAnyStepHasStacktrace()
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = "Passed" });
            example.Steps.Add(new LivingDocStep { Status = "Failed", ExceptionStackTrace = "at Step()" });

            Assert.That(example.HasStacktraces(), Is.True);
        }

        [Test]
        public void LivingDocExample_HasStacktraces_ReturnsTrue_ForEmptyStacktrace()
        {
            // Only null is treated as 'no stacktrace'...
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { ExceptionStackTrace = string.Empty });

            Assert.That(example.HasStacktraces(), Is.True);
        }

        // LivingDocExample.HasBackgrounds...

        [Test]
        public void LivingDocExample_HasBackgrounds_ReturnsFalse_WhenNoSteps()
        {
            Assert.That(new LivingDocExample().HasBackgrounds(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasBackgrounds_ReturnsFalse_WhenStepsIsNull()
        {
            Assert.That(new LivingDocExample { Steps = null }.HasBackgrounds(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasBackgrounds_ReturnsFalse_WhenOnlyScenarioSteps()
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Type = LivingDocStepTypes.Scenario.ToString() });
            example.Steps.Add(new LivingDocStep { Type = LivingDocStepTypes.Hook.ToString() });

            Assert.That(example.HasBackgrounds(), Is.False);
        }

        [Test]
        public void LivingDocExample_HasBackgrounds_ReturnsTrue_WhenAnyBackgroundStep()
        {
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Type = LivingDocStepTypes.Background.ToString() });
            example.Steps.Add(new LivingDocStep { Type = LivingDocStepTypes.Scenario.ToString() });

            Assert.That(example.HasBackgrounds(), Is.True);
        }

        // LivingDocFeature.HasBackground...

        [Test]
        public void LivingDocFeature_HasBackground_ReturnsFalse_WhenBackgroundIsNull()
        {
            Assert.That(new LivingDocFeature { Background = null }.HasBackground(), Is.False);
        }

        [Test]
        public void LivingDocFeature_HasBackground_ReturnsFalse_WhenBackgroundHasNoSteps()
        {
            Assert.That(new LivingDocFeature { Background = new LivingDocBackground() }.HasBackground(), Is.False);
        }

        [Test]
        public void LivingDocFeature_HasBackground_ReturnsTrue_WhenBackgroundHasSteps()
        {
            var feature = new LivingDocFeature { Background = new LivingDocBackground() };
            feature.Background.Steps.Add(new LivingDocStep { Name = "the shop is open" });

            Assert.That(feature.HasBackground(), Is.True);
        }

        // GetSumOfDuration...

        [Test]
        public void LivingDocScenario_GetSumOfDuration_SumsAllExamples()
        {
            var scenario = new LivingDocScenario();
            scenario.Examples.Add(CreateExample(TimeSpan.FromMilliseconds(1500)));
            scenario.Examples.Add(CreateExample(TimeSpan.FromMilliseconds(2750)));
            scenario.Examples.Add(CreateExample(TimeSpan.FromSeconds(60)));

            Assert.That(scenario.GetSumOfDuration(), Is.EqualTo(TimeSpan.FromMilliseconds(64250)));
            Assert.That(scenario.GetDuration(), Is.EqualTo("1min 4s"));
            Assert.That(scenario.GetDurationSortId(), Is.EqualTo("01:04:250"));
        }

        [Test]
        public void LivingDocScenario_GetSumOfDuration_ReturnsZero_WhenNoExamples()
        {
            Assert.That(new LivingDocScenario().GetSumOfDuration(), Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void LivingDocFeature_GetSumOfDuration_SumsAllScenariosAndExamples()
        {
            var feature = new LivingDocFeature();

            var scenarioOne = new LivingDocScenario();
            scenarioOne.Examples.Add(CreateExample(TimeSpan.FromSeconds(10)));
            scenarioOne.Examples.Add(CreateExample(TimeSpan.FromSeconds(20)));

            var scenarioTwo = new LivingDocScenario();
            scenarioTwo.Examples.Add(CreateExample(TimeSpan.FromSeconds(45)));

            feature.Scenarios.Add(scenarioOne);
            feature.Scenarios.Add(scenarioTwo);

            Assert.That(feature.GetSumOfDuration(), Is.EqualTo(TimeSpan.FromSeconds(75)));
            Assert.That(feature.GetDuration(), Is.EqualTo("1min 15s"));
            Assert.That(feature.GetDurationSortId(), Is.EqualTo("01:15:000"));
        }

        // GetDurationSortId...

        [Test]
        public void LivingDocScenario_GetDurationSortId_SortsCorrectly_BelowOneHour()
        {
            var shorter = new LivingDocScenario();
            shorter.Examples.Add(CreateExample(new TimeSpan(0, 0, 9, 59, 999)));

            var longer = new LivingDocScenario();
            longer.Examples.Add(CreateExample(new TimeSpan(0, 0, 10, 0, 0)));

            Assert.That(string.CompareOrdinal(shorter.GetDurationSortId(), longer.GetDurationSortId()), Is.LessThan(0));
        }
    }
}
