using Expressium.TestExecution;
using System;
using System.Linq;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionFeatureExtensions
    {
        public static bool IsTagged(this LivingDocFeature feature)
        {
            if (feature.Tags.Count == 0)
                return false;

            return true;
        }

        //public static void OrderByTags(this TestExecutionFeature feature)
        //{
        //    feature.Scenarios = feature.Scenarios.OrderBy(x => x.Tags).ToList();
        //}

        public static string GetTags(this LivingDocFeature feature)
        {
            return string.Join(" ", feature.Tags.Select(tag => tag.Name));
        }

        public static string GetStatus(this LivingDocFeature feature)
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

        public static int GetNumberOfPassed(this LivingDocFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsPassed());
        }

        public static int GetNumberOfIncomplete(this LivingDocFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsIncomplete());
        }

        public static int GetNumberOfFailed(this LivingDocFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsFailed());
        }

        public static int GetNumberOfSkipped(this LivingDocFeature feature)
        {
            return feature.Scenarios.Count(scenario => scenario.IsSkipped());
        }

        public static int GetNumberOfTests(this LivingDocFeature feature)
        {
            return feature.Scenarios.Count();
        }

        public static int GetNumberOfScenarios(this LivingDocFeature feature)
        {
            return feature.Scenarios.Count;
        }

        public static string GetNumberOfScenariosSortId(this LivingDocFeature feature)
        {
            return feature.Scenarios.Count.ToString("D4");
        }

        public static int GetPercentageOfPassed(this LivingDocFeature feature)
        {
            return (int)Math.Round(100.0f / feature.GetNumberOfTests() * feature.GetNumberOfPassed());
        }

        public static string GetPercentageOfPassedSortId(this LivingDocFeature feature)
        {
            return feature.GetPercentageOfPassed().ToString("D4");
        }
    }
}
