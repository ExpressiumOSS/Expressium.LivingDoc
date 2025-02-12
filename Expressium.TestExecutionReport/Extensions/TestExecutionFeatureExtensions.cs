using Expressium.TestExecution;
using Expressium.TestExecution.Cucumber;
using System;
using System.Linq;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionFeatureExtensions
    {
        public static bool IsTagged(this TestExecutionFeature feature)
        {
            return !string.IsNullOrEmpty(feature.Tags);
        }

        public static void OrderByTags(this TestExecutionFeature feature)
        {
            feature.Scenarios = feature.Scenarios.OrderBy(x => x.Tags).ToList();
        }

        public static string GetTags(this TestExecutionFeature feature)
        {
            return feature.Tags.FormatTags();
        }

        public static string GetStatus(this TestExecutionFeature feature)
        {
            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsSkipped())
                    return ReportStatuses.Skipped.ToString();
            }

            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsFailed())
                    return ReportStatuses.Failed.ToString();
            }

            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsIncomplete())
                    return ReportStatuses.Incomplete.ToString();
            }

            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsPassed())
                    return ReportStatuses.Passed.ToString();
            }

            return ReportStatuses.Undefined.ToString();
        }

        public static int GetNumberOfPassed(this TestExecutionFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsPassed());
        }

        public static int GetNumberOfIncomplete(this TestExecutionFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsIncomplete());
        }

        public static int GetNumberOfFailed(this TestExecutionFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsFailed());
        }

        public static int GetNumberOfSkipped(this TestExecutionFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsSkipped());
        }

        public static int GetNumberOfTests(this TestExecutionFeature feature)
        {
            return feature.Scenarios.Count();
        }

        public static int GetNumberOfScenarios(this TestExecutionFeature feature)
        {
            return feature.Scenarios.Count;
        }

        public static string GetNumberOfScenariosSortId(this TestExecutionFeature feature)
        {
            return feature.Scenarios.Count.ToString("D4");
        }

        public static int GetPercentageOfPassed(this TestExecutionFeature feature)
        {
            return (int)Math.Round(100.0f / feature.GetNumberOfTests() * feature.GetNumberOfPassed());
        }

        public static string GetPercentageOfPassedSortId(this TestExecutionFeature feature)
        {
            return feature.GetPercentageOfPassed().ToString("D4");
        }
    }
}
