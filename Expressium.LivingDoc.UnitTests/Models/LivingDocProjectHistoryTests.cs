using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocProjectHistoryTests
    {
        [Test]
        public void LivingDocProjectHistory_Constructor_InitialisesAllListsAsEmpty()
        {
            var history = new LivingDocProjectHistory();

            Assert.That(history.Features, Is.Not.Null);
            Assert.That(history.Features, Is.Empty);
            Assert.That(history.Scenarios, Is.Not.Null);
            Assert.That(history.Scenarios, Is.Empty);
            Assert.That(history.Steps, Is.Not.Null);
            Assert.That(history.Steps, Is.Empty);
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistoryFeatures_ReturnsZero_WhenEmpty()
        {
            var history = new LivingDocProjectHistory();

            Assert.That(history.GetMaximumNumberOfHistoryFeatures(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistoryScenarios_ReturnsZero_WhenEmpty()
        {
            var history = new LivingDocProjectHistory();

            Assert.That(history.GetMaximumNumberOfHistoryScenarios(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistorySteps_ReturnsZero_WhenEmpty()
        {
            var history = new LivingDocProjectHistory();

            Assert.That(history.GetMaximumNumberOfHistorySteps(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistoryFeatures_ReturnsMaxNotSum()
        {
            var history = new LivingDocProjectHistory();

            history.Features.Add(new LivingDocProjectHistoryResults { Passed = 3, Failed = 1 });
            history.Features.Add(new LivingDocProjectHistoryResults { Passed = 7 });
            history.Features.Add(new LivingDocProjectHistoryResults { Passed = 5, Incomplete = 2 });

            Assert.That(history.GetMaximumNumberOfHistoryFeatures(), Is.EqualTo(7));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistoryScenarios_ReturnsMaxNotSum()
        {
            var history = new LivingDocProjectHistory();

            history.Scenarios.Add(new LivingDocProjectHistoryResults { Passed = 10, Failed = 2 });
            history.Scenarios.Add(new LivingDocProjectHistoryResults { Passed = 15, Failed = 3 });
            history.Scenarios.Add(new LivingDocProjectHistoryResults { Passed = 8 });

            Assert.That(history.GetMaximumNumberOfHistoryScenarios(), Is.EqualTo(18));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistorySteps_ReturnsMaxNotSum()
        {
            var history = new LivingDocProjectHistory();

            history.Steps.Add(new LivingDocProjectHistoryResults { Passed = 20, Failed = 1 });
            history.Steps.Add(new LivingDocProjectHistoryResults { Passed = 30, Failed = 2, Skipped = 1 });
            history.Steps.Add(new LivingDocProjectHistoryResults { Passed = 25, Incomplete = 3 });

            Assert.That(history.GetMaximumNumberOfHistorySteps(), Is.EqualTo(33));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistoryFeatures_SingleEntry_ReturnsItsTotal()
        {
            var history = new LivingDocProjectHistory();

            history.Features.Add(new LivingDocProjectHistoryResults { Passed = 4, Failed = 2, Skipped = 1 });

            Assert.That(history.GetMaximumNumberOfHistoryFeatures(), Is.EqualTo(7));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistoryScenarios_SingleEntry_ReturnsItsTotal()
        {
            var history = new LivingDocProjectHistory();

            history.Scenarios.Add(new LivingDocProjectHistoryResults { Passed = 5, Incomplete = 2 });

            Assert.That(history.GetMaximumNumberOfHistoryScenarios(), Is.EqualTo(7));
        }

        [Test]
        public void LivingDocProjectHistory_GetMaximumNumberOfHistorySteps_SingleEntry_ReturnsItsTotal()
        {
            var history = new LivingDocProjectHistory();

            history.Steps.Add(new LivingDocProjectHistoryResults { Passed = 10, Failed = 3, Skipped = 2 });

            Assert.That(history.GetMaximumNumberOfHistorySteps(), Is.EqualTo(15));
        }

        [Test]
        public void LivingDocProjectHistory_AllLists_AreIndependent()
        {
            var history = new LivingDocProjectHistory();

            history.Features.Add(new LivingDocProjectHistoryResults { Passed = 2 });
            history.Scenarios.Add(new LivingDocProjectHistoryResults { Passed = 10 });
            history.Scenarios.Add(new LivingDocProjectHistoryResults { Passed = 12 });
            history.Steps.Add(new LivingDocProjectHistoryResults { Passed = 50 });

            Assert.That(history.Features.Count, Is.EqualTo(1));
            Assert.That(history.Scenarios.Count, Is.EqualTo(2));
            Assert.That(history.Steps.Count, Is.EqualTo(1));

            Assert.That(history.GetMaximumNumberOfHistoryFeatures(), Is.EqualTo(2));
            Assert.That(history.GetMaximumNumberOfHistoryScenarios(), Is.EqualTo(12));
            Assert.That(history.GetMaximumNumberOfHistorySteps(), Is.EqualTo(50));
        }
    }
}
