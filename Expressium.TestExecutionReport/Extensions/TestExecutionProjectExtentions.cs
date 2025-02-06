using Expressium.TestExecution;
using System;
using System.Linq;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionProjectExtentions
    {
        public static void OrderByTags(this TestExecutionProject project)
        {
            foreach (var feature in project.Features)
                feature.OrderByTags();

            project.Features = project.Features.OrderBy(x => x.Tags).ToList();
        }

        public static int GetNumberOfPassed(this TestExecutionProject project)
        {
            return project.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsPassed());
        }

        public static int GetNumberOfIncomplete(this TestExecutionProject project)
        {
            return project.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsIncomplete());
        }

        public static int GetNumberOfFailed(this TestExecutionProject project)
        {
            return project.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsFailed());
        }

        public static int GetNumberOfSkipped(this TestExecutionProject project)
        {
            return project.Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.IsSkipped());
        }

        public static int GetNumberOfTests(this TestExecutionProject project)
        {
            return project.Features
                .SelectMany(feature => feature.Scenarios)
                .Count();
        }

        public static string GetExecutionTime(this TestExecutionProject project)
        {
            return project.ExecutionTime.ToString("ddd dd. MMM yyyy HH':'mm':'ss \"GMT\"z");
        }

        public static string GetDuration(this TestExecutionProject project)
        {
            var duration = project.EndTime - project.StartTime;

            if (duration.Minutes > 0)
                return $"{duration.Minutes}min {duration.Seconds}s";

            return $"{duration.Seconds}s {duration.Milliseconds}ms";
        }

        public static double GetPercentageOfPassed(this TestExecutionProject project)
        {
            return Math.Round(100.0f / project.GetNumberOfTests() * project.GetNumberOfPassed());
        }

        public static double GetPercentageOfIncomplete(this TestExecutionProject project)
        {
            return Math.Round(100.0f / project.GetNumberOfTests() * project.GetNumberOfIncomplete());
        }

        public static double GetPercentageOfFailed(this TestExecutionProject project)
        {
            return Math.Round(100.0f / project.GetNumberOfTests() * project.GetNumberOfFailed());
        }

        public static double GetPercentageOfSkipped(this TestExecutionProject project)
        {
            return Math.Round(100.0f / project.GetNumberOfTests() * project.GetNumberOfSkipped());
        }

        public static string GetStatusMessage(this TestExecutionProject project, int percentage)
        {
            if (percentage >= 100)
                return "The system is fully covered and successfully validated!";
            else if (percentage >= 90)
                return "The system is extensively covered with minor potential risks!";
            else if (percentage >= 75)
                return "The system is well covered with significant potential risks!";
            else if (percentage >= 50)
                return "The system is moderately covered with significant potential risks!";
            else if (percentage >= 25)
                return "The system is partially covered with many potential risks!";
            else if (percentage >= 10)
                return "The system is minimally covered with many undetected risks!";
            else if (percentage < 10)
                return "The system is not covered with a uncertainties in reliability!";
            else
            {
            }

            return null;
        }
    }
}
