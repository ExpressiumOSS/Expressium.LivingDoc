using Expressium.LivingDoc.Generators;

namespace Expressium.LivingDoc.Tests.Generators
{
    [TestFixture]
    public class LivingDocDataAnalyticsGeneratorTests
    {
        [TestCase(0, 10, ExpectedResult = 0)]
        [TestCase(5, 10, ExpectedResult = 50)]
        [TestCase(1, 100, ExpectedResult = 1)]
        [TestCase(99, 100, ExpectedResult = 99)]
        [TestCase(1, 200, ExpectedResult = 1)]
        [TestCase(2, 3, ExpectedResult = 67)]
        [TestCase(1, 3, ExpectedResult = 33)]
        [TestCase(3, 3, ExpectedResult = 100)]
        [TestCase(0, 1, ExpectedResult = 0)]
        [TestCase(1, 1, ExpectedResult = 100)]
        public int LivingDocDataAnalyticsGenerator_CalculatePercentage(int numberOfStatuses, int numberOfTests)
        {
            return LivingDocDataAnalyticsGenerator.CalculatePercentage(numberOfStatuses, numberOfTests);
        }

        [TestCase(100, 0, 0, 0, 100, 0, 0, 0)]
        [TestCase(0, 0, 0, 0, 0, 0, 0, 0)]
        [TestCase(10, 20, 30, 55, 10, 20, 30, 40)]
        [TestCase(10, 20, 30, 45, 10, 20, 30, 40)]
        [TestCase(11, 21, 31, 41, 11, 21, 31, 37)]
        [TestCase(11, 19, 30, 40, 11, 19, 30, 40)]
        public void LivingDocDataAnalyticsGenerator_AdjustPercentagesDiscrepancy(
            int passed, int incomplete, int failed, int skipped,
            int passedOut, int incompleteOut, int failedOut, int skippedOut)
        {
            LivingDocDataAnalyticsGenerator.AdjustPercentagesDiscrepancy(ref passed, ref incomplete, ref failed, ref skipped);

            Assert.That(passed, Is.EqualTo(passedOut));
            Assert.That(incomplete, Is.EqualTo(incompleteOut));
            Assert.That(failed, Is.EqualTo(failedOut));
            Assert.That(skipped, Is.EqualTo(skippedOut));
        }
    }
}
