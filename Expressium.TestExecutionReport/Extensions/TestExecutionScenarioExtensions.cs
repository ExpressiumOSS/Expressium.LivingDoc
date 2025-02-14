using Expressium.TestExecution;
using System;
using System.Linq;

namespace Expressium.LivingDoc.Extensions
{
    public static class TestExecutionScenarioExtensions
    {
        public static bool IsTagged(this LivingDocScenario scenario)
        {
            if (scenario.Tags.Count == 0)
                return false;

            return true;
        }

        public static string GetTags(this LivingDocScenario scenario)
        {
            return string.Join(" ", scenario.Tags.Select(tag => tag.Name));
        }

        public static bool IsPassed(this LivingDocScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Passed.ToString())
                return true;

            return false;
        }

        public static bool IsIncomplete(this LivingDocScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Incomplete.ToString())
                return true;

            return false;
        }

        public static bool IsFailed(this LivingDocScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Failed.ToString())
                return true;

            return false;
        }

        public static bool IsSkipped(this LivingDocScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Skipped.ToString())
                return true;

            return false;
        }

        public static string GetStatus(this LivingDocScenario scenario)
        {
            foreach (var example in scenario.Examples)
            {
                if (example.IsSkipped())
                    return ReportStatuses.Skipped.ToString();
            }

            foreach (var example in scenario.Examples)
            {
                if (example.IsFailed())
                    return ReportStatuses.Failed.ToString();
            }

            foreach (var example in scenario.Examples)
            {
                if (example.IsIncomplete())
                    return ReportStatuses.Incomplete.ToString();
            }

            foreach (var example in scenario.Examples)
            {
                if (example.IsPassed())
                    return ReportStatuses.Passed.ToString();
            }

            return ReportStatuses.Undefined.ToString();
        }

        public static string GetIndexSortId(this LivingDocScenario scenario)
        {
            return scenario.Index.ToString("D4");
        }

        public static string GetDuration(this LivingDocScenario scenario)
        {
            TimeSpan? duration = null;

            foreach (var example in scenario.Examples)
            {
                if (duration == null)
                    duration = example.EndTime - example.StartTime;
                else
                    duration += example.EndTime - example.StartTime;
            }

            var totalDuration = duration.GetValueOrDefault();

            if (totalDuration.Minutes > 0)
                return $"{totalDuration.Minutes}min {totalDuration.Seconds}s";

            return $"{totalDuration.Seconds}s {totalDuration.Milliseconds.ToString("D3")}ms";
        }

        public static string GetDurationSortId(this LivingDocScenario scenario)
        {
            TimeSpan? duration = null;

            foreach (var example in scenario.Examples)
            {
                if (duration == null)
                    duration = example.EndTime - example.StartTime;
                else
                    duration += example.EndTime - example.StartTime;
            }

            var totalDuration = duration.GetValueOrDefault();

            return $"{totalDuration.Minutes.ToString("D2")}:{totalDuration.Seconds.ToString("D2")}:{totalDuration.Milliseconds.ToString("D3")}";
        }
    }
}
