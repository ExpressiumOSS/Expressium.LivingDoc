using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.Tests.Generators
{
    [TestFixture]
    public class LivingDocDataAnalyticsGeneratorTests
    {
        private LivingDocDataAnalyticsGenerator dataAnalyticsGenerator;

        [SetUp]
        public void SetUp()
        {
            var livingDocProject = new LivingDocProject();
            dataAnalyticsGenerator = new LivingDocDataAnalyticsGenerator(livingDocProject);
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
            return dataAnalyticsGenerator.CalculatePercentage(numberOfStatuses, numberOfTests);
        }
    }
}
