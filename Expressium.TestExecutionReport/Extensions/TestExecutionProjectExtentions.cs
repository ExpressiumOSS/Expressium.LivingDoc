using Expressium.TestExecution;
using System.Linq;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionProjectExtentions
    {
        public static void OrderByTags(this TestExecutionProject context)
        {
            foreach (var feature in context.Features)
                feature.OrderByTags();

            context.Features = context.Features.OrderBy(x => x.Tags).ToList();
        }

        public static int GetNumberOfPassed(this TestExecutionProject context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsPassed());
        }

        public static int GetNumberOfIncomplete(this TestExecutionProject context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsIncomplete());
        }

        public static int GetNumberOfFailed(this TestExecutionProject context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsFailed());
        }

        public static int GetNumberOfSkipped(this TestExecutionProject context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsSkipped());
        }

        public static int GetNumberOfTests(this TestExecutionProject context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count();
        }

        public static string GetExecutionTime(this TestExecutionProject context)
        {
            return context.ExecutionTime.ToString("ddd dd. MMM yyyy HH':'mm':'ss \"GMT\"z");
        }

        public static string GetDuration(this TestExecutionProject context)
        {
            var duration = context.EndTime - context.StartTime;

            if (duration.Minutes > 0)
                return $"{duration.Minutes}min {duration.Seconds}s";

            return $"{duration.Seconds}s {duration.Milliseconds}ms";
        }
    }
}
