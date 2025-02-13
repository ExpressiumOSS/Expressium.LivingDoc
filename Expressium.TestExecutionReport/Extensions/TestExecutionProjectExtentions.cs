using Expressium.TestExecution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionProjectExtentions
    {
        //public static void OrderByTags(this TestExecutionProject project)
        //{
        //    foreach (var feature in project.Features)
        //        feature.OrderByTags();

        //    project.Features = project.Features.OrderBy(x => x.Tags).ToList();
        //}

        public static int GetNumberOfPassedFeatures(this TestExecutionProject project)
        {
            return project.Features.Count(feature => feature.GetStatus() == ReportStatuses.Passed.ToString());
        }

        public static int GetNumberOfIncompleteFeatures(this TestExecutionProject project)
        {
            return project.Features.Count(feature => feature.GetStatus() == ReportStatuses.Incomplete.ToString());
        }

        public static int GetNumberOfFailedFeatures(this TestExecutionProject project)
        {
            return project.Features.Count(feature => feature.GetStatus() == ReportStatuses.Failed.ToString());
        }

        public static int GetNumberOfSkippedFeatures(this TestExecutionProject project)
        {
            return project.Features.Count(feature => feature.GetStatus() == ReportStatuses.Skipped.ToString());
        }

        public static int GetNumberOfTestsFeature(this TestExecutionProject project)
        {
            return project.Features.Count;
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

            return $"{duration.Seconds}s {duration.Milliseconds.ToString("D3")}ms";
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

        public static FolderNode GetListOfFolderNodes(this TestExecutionProject project)
        {
            var listOfFolders = new List<string>();

            foreach (var feature in project.Features)
            {
                if (!listOfFolders.Contains(feature.Uri))
                    listOfFolders.Add(feature.Uri);
            }

            return TestExecutionFolderExtension.BuildTree(listOfFolders);
        }
    }
}
