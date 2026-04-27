using Expressium.LivingDoc.Models;
using System;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocProjectHistoryResultsTests
    {
        [Test]
        public void LivingDocProjectHistoryResults_DefaultConstructor_DateIsMinValue()
        {
            var historyResults = new LivingDocProjectHistoryResults();

            Assert.That(historyResults.Date, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void LivingDocProjectHistoryResults_DefaultConstructor_CountsAreZero()
        {
            var historyResults = new LivingDocProjectHistoryResults();

            Assert.That(historyResults.Passed, Is.EqualTo(0));
            Assert.That(historyResults.Incomplete, Is.EqualTo(0));
            Assert.That(historyResults.Failed, Is.EqualTo(0));
            Assert.That(historyResults.Skipped, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocProjectHistoryResults_DefaultConstructor_GetNumberOfTests_IsZero()
        {
            var historyResults = new LivingDocProjectHistoryResults();

            Assert.That(historyResults.GetNumberOfTests(), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocProjectHistoryResults_SetDate_RoundTrips()
        {
            var date = new DateTime(2025, 3, 10, 12, 0, 0, DateTimeKind.Utc);

            var historyResults = new LivingDocProjectHistoryResults
            {
                Date = date
            };

            Assert.That(historyResults.Date, Is.EqualTo(date));
        }

        [Test]
        public void LivingDocProjectHistoryResults_GetDate_ReturnsFormattedDate()
        {
            var historyResults = new LivingDocProjectHistoryResults
            {
                Date = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc)
            };

            Assert.That(historyResults.GetDate(), Is.EqualTo("Sat 15 Jun 2024 at 10:30:45 GMT+0"));
        }

        [Test]
        public void LivingDocProjectHistoryResults_GetDate_ReturnsNonNullString()
        {
            var historyResults = new LivingDocProjectHistoryResults
            {
                Date = DateTime.UtcNow
            };

            Assert.That(historyResults.GetDate(), Is.Not.Null);
            Assert.That(historyResults.GetDate(), Is.Not.Empty);
        }

        [Test]
        public void LivingDocProjectHistoryResults_GetDate_MinValue_ReturnsFormattedString()
        {
            var historyResults = new LivingDocProjectHistoryResults();

            Assert.That(historyResults.GetDate(), Is.Not.Null);
            Assert.That(historyResults.GetDate(), Is.Not.Empty);
        }

        [Test]
        public void LivingDocProjectHistoryResults_GetNumberOfTests_OnlyPassed()
        {
            var historyResults = new LivingDocProjectHistoryResults
            {
                Passed = 5
            };

            Assert.That(historyResults.GetNumberOfTests(), Is.EqualTo(5));
        }

        [Test]
        public void LivingDocProjectHistoryResults_GetNumberOfTests_OnlyFailed()
        {
            var historyResults = new LivingDocProjectHistoryResults
            {
                Failed = 3
            };

            Assert.That(historyResults.GetNumberOfTests(), Is.EqualTo(3));
        }

        [Test]
        public void LivingDocProjectHistoryResults_GetNumberOfTests_OnlyIncomplete()
        {
            var historyResults = new LivingDocProjectHistoryResults
            {
                Incomplete = 2
            };

            Assert.That(historyResults.GetNumberOfTests(), Is.EqualTo(2));
        }

        [Test]
        public void LivingDocProjectHistoryResults_GetNumberOfTests_OnlySkipped()
        {
            var historyResults = new LivingDocProjectHistoryResults
            {
                Skipped = 4
            };

            Assert.That(historyResults.GetNumberOfTests(), Is.EqualTo(4));
        }

        [TestCase(5, 0, 0, 0, 5)]
        [TestCase(0, 2, 0, 0, 2)]
        [TestCase(0, 0, 3, 0, 3)]
        [TestCase(0, 0, 0, 4, 4)]
        [TestCase(5, 2, 3, 4, 14)]
        [TestCase(10, 0, 2, 1, 13)]
        [TestCase(0, 0, 0, 0, 0)]
        public void LivingDocProjectHistoryResults_GetNumberOfTests(int passed, int incomplete, int failed, int skipped, int total)
        {
            var historyResults = new LivingDocProjectHistoryResults
            {
                Passed = passed,
                Incomplete = incomplete,
                Failed = failed,
                Skipped = skipped
            };

            Assert.That(historyResults.GetNumberOfTests(), Is.EqualTo(total));
        }

        [Test]
        public void LivingDocProjectHistoryResults_MultipleInstances_AreIndependent()
        {
            var historyOne = new LivingDocProjectHistoryResults
            {
                Date = DateTime.UtcNow.AddDays(-1),
                Passed = 10,
                Failed = 2
            };

            var historyTwo = new LivingDocProjectHistoryResults
            {
                Date = DateTime.UtcNow,
                Passed = 5,
                Failed = 1
            };

            Assert.That(historyOne.Passed, Is.Not.EqualTo(historyTwo.Passed));
            Assert.That(historyOne.Failed, Is.Not.EqualTo(historyTwo.Failed));
            Assert.That(historyOne.Date, Is.LessThan(historyTwo.Date));
            Assert.That(historyOne.GetNumberOfTests(), Is.EqualTo(12));
            Assert.That(historyTwo.GetNumberOfTests(), Is.EqualTo(6));
        }
    }
}
