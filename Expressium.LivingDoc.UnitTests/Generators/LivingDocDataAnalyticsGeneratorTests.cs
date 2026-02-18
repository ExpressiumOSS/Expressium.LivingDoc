using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.Tests.Generators
{
    [TestFixture]
    public class LivingDocDataAnalyticsGeneratorTests
    {
        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTitle()
        {
            var livingDocProject = new LivingDocProject();

            var generator = new LivingDocDataAnalyticsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataAnalyticsTitle();

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<div class='section'>"));
            Assert.That(listOfLines[1], Is.EqualTo("<span class='page-name' data-testid='page-title'>Analytics</span>"));
            Assert.That(listOfLines[2], Is.EqualTo("</div>"));
        }

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

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsStatusChart()
        {
            var livingDocProject = new LivingDocProject();

            var title = "Title";
            var numberOfPassed = 5;
            var numberOfIncomplete = 4;
            var numberOfFailed = 3;
            var numberOfSkipped = 2;
            var numberOfTests = 14;

            var generator = new LivingDocDataAnalyticsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataAnalyticsStatusChart(title, numberOfPassed, numberOfIncomplete, numberOfFailed, numberOfSkipped, numberOfTests);

            Assert.That(listOfLines.Count, Is.EqualTo(56));
            Assert.That(listOfLines[1], Is.EqualTo("<span class='chart-name' data-testid='title-chart-title'>Title</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<!-- Data Analytics Status Chart -->"));
            Assert.That(listOfLines[13].Trim(), Is.EqualTo("<text x='50%' y='50%' class='chart-number' data-testid='title-chart-passed'>36%</text>"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends()
        {
            var project = new LivingDocProject();

            var historyOne = new LivingDocHistory();
            historyOne.Date = System.DateTime.Now.AddDays(-1);
            historyOne.Features.Passed.AddRange(new[] { "F1", "F2", "F3" });
            historyOne.Features.Incomplete.Add("F4");
            historyOne.Features.Failed.Add("F5");

            var historyTwo = new LivingDocHistory();
            historyTwo.Date = System.DateTime.Now;
            historyTwo.Features.Passed.AddRange(new[] { "A", "B", "C", "D", "E", "F" });
            historyTwo.Features.Incomplete.AddRange(new[] { "G", "H" });
            historyTwo.Features.Failed.Add("I");
            historyTwo.Features.Skipped.Add("J");

            project.Histories.Add(historyOne);
            project.Histories.Add(historyTwo);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsTrends("Features");

            Assert.That(listOfLines.Count, Is.EqualTo(43));
            Assert.That(listOfLines[1], Is.EqualTo("<!-- Data Analytics Trend Chart -->"));
            Assert.That(listOfLines[2], Is.EqualTo("<div class='section chart-history-trends' id='features-history-trends'>"));

            Assert.That(listOfLines[20].Trim(), Is.EqualTo("<div class='bgcolor-passed' title='80%' style='width: 80%; height: 0.80em; float: left'></div>"));
            Assert.That(listOfLines[21].Trim(), Is.EqualTo("<div class='bgcolor-incomplete' title='10%' style='width: 10%; height: 0.80em; float: left'></div>"));
            Assert.That(listOfLines[22].Trim(), Is.EqualTo("<div class='bgcolor-failed' title='10%' style='width: 10%; height: 0.80em; float: left'></div>"));
            Assert.That(listOfLines[23].Trim(), Is.EqualTo("<div class='bgcolor-skipped' title='0%' style='width: 0%; height: 0.80em; float: left'></div>"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsFailures()
        {
            var project = new LivingDocProject();

            var historyOne = new LivingDocHistory();
            historyOne.Date = System.DateTime.Now.AddDays(-1);
            historyOne.Features.Failed.Add("Successful User Login with Valid Credentials");

            var historyTwo = new LivingDocHistory();
            historyTwo.Date = System.DateTime.Now;
            historyTwo.Features.Passed.Add("Successful User Login with Valid Credentials");

            project.Histories.Add(historyOne);
            project.Histories.Add(historyTwo);

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsFailures("Features");

            Assert.That(listOfLines.Count, Is.EqualTo(23));
            Assert.That(listOfLines[1], Is.EqualTo("<!-- Data Analytics Failures Chart -->"));
            Assert.That(listOfLines[2], Is.EqualTo("<div class='section chart-history-failures' id='features-history-failures'>"));

            Assert.That(listOfLines[15], Is.EqualTo("<td>Successful User Login with Valid Credentials</td>"));
            Assert.That(listOfLines[16], Is.EqualTo("<td class='history-failed'>Failed</td>"));
            Assert.That(listOfLines[17], Is.EqualTo("<td class='history-passed'>Passed</td>"));
        }
    }
}
