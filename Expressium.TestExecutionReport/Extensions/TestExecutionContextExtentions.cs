using Expressium.TestExecution;
using System.Linq;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionContextExtentions
    {
        public static string GetExecutionTime(this TestExecutionContext context)
        {
            return context.ExecutionTime.ToString("ddd dd. MMM yyyy HH':'mm':'ss \"GMT\"z");
        }

        public static void OrderByTags(this TestExecutionContext context)
        {
            foreach (var feature in context.Features)
                feature.OrderByTags();

            context.Features = context.Features.OrderBy(x => x.Tags).ToList();
        }

        public static int GetNumberOfPassed(this TestExecutionContext context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsPassed());
        }

        public static int GetNumberOfFailed(this TestExecutionContext context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsFailed());
        }

        public static int GetNumberOfInconclusive(this TestExecutionContext context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsInconclusive());
        }

        public static int GetNumberOfSkipped(this TestExecutionContext context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsSkipped());
        }

        public static int GetNumberOfTests(this TestExecutionContext context)
        {
            return context.Features
                .SelectMany(feature => feature.Scenarios)
                .Count();
        }
    }
}
