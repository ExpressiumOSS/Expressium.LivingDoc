using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataAnalyticsGeneratorTrendsTests
    {
        private static readonly DateTime BaseDate = new DateTime(2026, 1, 1, 12, 0, 0);

        private static LivingDocProject CreateProject(params LivingDocProjectHistoryResults[] scenarioHistories)
        {
            var project = new LivingDocProject();
            project.History.Scenarios.AddRange(scenarioHistories);
            return project;
        }

        private static string[] GetBarStyles(System.Collections.Generic.List<string> listOfLines, string color)
        {
            return listOfLines.Where(l => l.StartsWith($"<div class='bgcolor-{color}'")).ToArray();
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_OneRowPerRun()
        {
            var project = CreateProject(
                new LivingDocProjectHistoryResults { Date = BaseDate, Passed = 2 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1), Passed = 1, Failed = 1 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(2), Failed = 2 });

            var listOfLines = new LivingDocDataAnalyticsGenerator(project).GenerateDataAnalyticsTrends("Scenarios");

            Assert.That(GetBarStyles(listOfLines, "passed").Length, Is.EqualTo(3));
            Assert.That(listOfLines, Does.Contain($"<td>{project.History.Scenarios[0].GetDate()}</td>"));
            Assert.That(listOfLines, Does.Contain($"<td>{project.History.Scenarios[2].GetDate()}</td>"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_SkipsRunsWithoutResults()
        {
            var project = CreateProject(
                new LivingDocProjectHistoryResults { Date = BaseDate, Passed = 2 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1) },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(2), Passed = 1 });

            var listOfLines = new LivingDocDataAnalyticsGenerator(project).GenerateDataAnalyticsTrends("Scenarios");

            Assert.That(GetBarStyles(listOfLines, "passed").Length, Is.EqualTo(2));
            Assert.That(listOfLines, Does.Not.Contain($"<td>{project.History.Scenarios[1].GetDate()}</td>"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_WidthsAreRelativeToLargestRun()
        {
            var project = CreateProject(
                new LivingDocProjectHistoryResults { Date = BaseDate, Passed = 4 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1), Passed = 1, Failed = 1 });

            var listOfLines = new LivingDocDataAnalyticsGenerator(project).GenerateDataAnalyticsTrends("Scenarios");

            var passed = GetBarStyles(listOfLines, "passed");
            var failed = GetBarStyles(listOfLines, "failed");

            Assert.That(passed[0], Does.Contain("title='4' style='width: 100%;"));
            Assert.That(passed[1], Does.Contain("title='1' style='width: 25%;"));
            Assert.That(failed[1], Does.Contain("title='1' style='width: 25%;"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_RoundingAbove100_IsAdjustedOnLargestShare()
        {
            // 5 of 8 = 62.5% => 63% and 3 of 8 = 37.5% => 38% gives 101%...
            var project = CreateProject(
                new LivingDocProjectHistoryResults { Date = BaseDate, Passed = 5, Failed = 3 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1), Passed = 8 });

            var listOfLines = new LivingDocDataAnalyticsGenerator(project).GenerateDataAnalyticsTrends("Scenarios");

            var passed = GetBarStyles(listOfLines, "passed");
            var failed = GetBarStyles(listOfLines, "failed");

            Assert.That(passed[0], Does.Contain("width: 62%;"));
            Assert.That(failed[0], Does.Contain("width: 38%;"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_SmallShare_IsAtLeastOnePercent()
        {
            var project = CreateProject(
                new LivingDocProjectHistoryResults { Date = BaseDate, Passed = 999, Failed = 1 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1), Passed = 1000 });

            var listOfLines = new LivingDocDataAnalyticsGenerator(project).GenerateDataAnalyticsTrends("Scenarios");

            Assert.That(GetBarStyles(listOfLines, "failed")[0], Does.Contain("title='1' style='width: 1%;"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsTrends_UnknownType_RendersNoRows()
        {
            var project = CreateProject(
                new LivingDocProjectHistoryResults { Date = BaseDate, Passed = 1 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1), Passed = 1 });

            var listOfLines = new LivingDocDataAnalyticsGenerator(project).GenerateDataAnalyticsTrends("Rules");

            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Trend Chart -->"));
            Assert.That(GetBarStyles(listOfLines, "passed").Length, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_AllViews_IncludeTrends_OnlyWithHistory()
        {
            var withHistory = new LivingDocDataAnalyticsGenerator(CreateProject(
                new LivingDocProjectHistoryResults { Date = BaseDate, Passed = 1 },
                new LivingDocProjectHistoryResults { Date = BaseDate.AddDays(1), Passed = 1 }));

            var withoutHistory = new LivingDocDataAnalyticsGenerator(new LivingDocProject());

            Assert.That(withHistory.GenerateDataAnalyticsScenariosView(), Does.Contain("<!-- Data Analytics Trend Chart -->"));
            Assert.That(withoutHistory.GenerateDataAnalyticsScenariosView(), Does.Not.Contain("<!-- Data Analytics Trend Chart -->"));
            Assert.That(withoutHistory.GenerateDataAnalyticsFeaturesView(), Does.Not.Contain("<!-- Data Analytics Trend Chart -->"));
            Assert.That(withoutHistory.GenerateDataAnalyticsStepsView(), Does.Not.Contain("<!-- Data Analytics Trend Chart -->"));
        }
    }
}
