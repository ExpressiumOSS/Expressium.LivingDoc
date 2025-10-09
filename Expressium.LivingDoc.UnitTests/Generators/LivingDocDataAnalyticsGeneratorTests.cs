using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Tests.Generators
{
    [TestFixture]
    public class LivingDocDataAnalyticsGeneratorTests
    {
        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsDuration()
        {
            var livingDocProject = new LivingDocProject();
            livingDocProject.Duration = new System.TimeSpan(0, 4, 12, 30, 0);

            var generator = new LivingDocDataAnalyticsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataAnalyticsDuration();

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[1], Is.EqualTo("<!-- Data Analytics Duration -->"));
            Assert.That(listOfLines[6], Is.EqualTo("<span data-testid='project-duration'>4h 12min</span>"));
        }

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
