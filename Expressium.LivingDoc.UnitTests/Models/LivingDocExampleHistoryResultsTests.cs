using Expressium.LivingDoc.Models;
using System;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocExampleHistoryResultsTests
    {
        [Test]
        public void LivingDocExampleHistoryResults_DefaultConstructor_DateIsMinValue()
        {
            var historyResults = new LivingDocExampleHistoryResults();

            Assert.That(historyResults.Date, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void LivingDocExampleHistoryResults_DefaultConstructor_StatusIsNull()
        {
            var historyResults = new LivingDocExampleHistoryResults();

            Assert.That(historyResults.Status, Is.Null);
        }

        [Test]
        public void LivingDocExampleHistoryResults_SetStatus_Passed()
        {
            var historyResults = new LivingDocExampleHistoryResults
            {
                Status = LivingDocStatuses.Passed.ToString()
            };

            Assert.That(historyResults.Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void LivingDocExampleHistoryResults_SetStatus_Failed()
        {
            var historyResults = new LivingDocExampleHistoryResults
            {
                Status = LivingDocStatuses.Failed.ToString()
            };

            Assert.That(historyResults.Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
        }

        [Test]
        public void LivingDocExampleHistoryResults_SetStatus_Incomplete()
        {
            var historyResults = new LivingDocExampleHistoryResults
            {
                Status = LivingDocStatuses.Incomplete.ToString()
            };

            Assert.That(historyResults.Status, Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
        }

        [Test]
        public void LivingDocExampleHistoryResults_SetStatus_Skipped()
        {
            var historyResults = new LivingDocExampleHistoryResults
            {
                Status = LivingDocStatuses.Skipped.ToString()
            };

            Assert.That(historyResults.Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void LivingDocExampleHistoryResults_GetDate_ReturnsFormattedDate()
        {
            var date = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

            var historyResults = new LivingDocExampleHistoryResults
            {
                Date = date
            };

            Assert.That(historyResults.GetDate(), Is.EqualTo("Sat 15 Jun 2024 at 10:30:45 GMT+0"));
        }

        [Test]
        public void LivingDocExampleHistoryResults_GetDate_ReturnsNonNullString()
        {
            var historyResults = new LivingDocExampleHistoryResults
            {
                Date = DateTime.UtcNow
            };

            Assert.That(historyResults.GetDate(), Is.Not.Null);
            Assert.That(historyResults.GetDate(), Is.Not.Empty);
        }

        [Test]
        public void LivingDocExampleHistoryResults_GetDate_MinValue_ReturnsFormattedString()
        {
            var historyResults = new LivingDocExampleHistoryResults();

            Assert.That(historyResults.GetDate(), Is.Not.Null);
            Assert.That(historyResults.GetDate(), Is.Not.Empty);
        }

        [Test]
        public void LivingDocExampleHistoryResults_SetDate_RoundTrips()
        {
            var date = new DateTime(2025, 1, 20, 8, 0, 0, DateTimeKind.Utc);

            var historyResults = new LivingDocExampleHistoryResults
            {
                Date = date
            };

            Assert.That(historyResults.Date, Is.EqualTo(date));
        }

        [Test]
        public void LivingDocExampleHistoryResults_MultipleInstances_AreIndependent()
        {
            var historyOne = new LivingDocExampleHistoryResults
            {
                Date = DateTime.UtcNow.AddDays(-1),
                Status = LivingDocStatuses.Passed.ToString()
            };

            var historyTwo = new LivingDocExampleHistoryResults
            {
                Date = DateTime.UtcNow,
                Status = LivingDocStatuses.Failed.ToString()
            };

            Assert.That(historyOne.Status, Is.Not.EqualTo(historyTwo.Status));
            Assert.That(historyOne.Date, Is.LessThan(historyTwo.Date));
        }
    }
}
