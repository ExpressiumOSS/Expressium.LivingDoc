using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocProject
    {
        public string Title { get; set; }
        public DateTime ExecutionTime { get; set; }
        public TimeSpan Duration { get; set; }

        public List<LivingDocFeature> Features { get; set; }

        public LivingDocProject()
        {
            Duration = new TimeSpan();
            Features = new List<LivingDocFeature>();
        }

        public bool IsFeatureAdded(string title)
        {
            return Features.Any(m => m.Name == title);
        }

        public LivingDocFeature GetFeature(string title)
        {
            return Features.Find(x => x.Name == title);
        }

        public int GetNumberOfFeatures()
        {
            return Features.Count;
        }

        public int GetNumberOfPassedFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Passed.ToString());
        }

        public int GetNumberOfIncompleteFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Incomplete.ToString());
        }

        public int GetNumberOfFailedFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Failed.ToString());
        }

        public int GetNumberOfSkippedFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Skipped.ToString());
        }

        public int GetNumberOfPassedScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Passed.ToString());
        }

        public int GetNumberOfIncompleteScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Incomplete.ToString());
        }

        public int GetNumberOfFailedScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Failed.ToString());
        }

        public int GetNumberOfSkippedScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Skipped.ToString());
        }

        public int GetNumberOfScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count();
        }

        public int GetNumberOfExamples()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .SelectMany(scenario => scenario.Examples)
                .Count();
        }

        public int GetNumberOfPassedSteps()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .SelectMany(scenario => scenario.Examples)
                .SelectMany(example => example.Steps)
                .Where(step => step.IsPassed())
                .Count();
        }

        public int GetNumberOfIncompleteSteps()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .SelectMany(scenario => scenario.Examples)
                .SelectMany(example => example.Steps)
                .Where(step => step.IsIncomplete())
                .Count();
        }

        public int GetNumberOfFailedSteps()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .SelectMany(scenario => scenario.Examples)
                .SelectMany(example => example.Steps)
                .Where(step => step.IsFailed())
                .Count();
        }

        public int GetNumberOfSkippedSteps()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .SelectMany(scenario => scenario.Examples)
                .SelectMany(example => example.Steps)
                .Where(step => step.IsSkipped())
                .Count();
        }

        public int GetNumberOfSteps()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .SelectMany(scenario => scenario.Examples)
                .SelectMany(example => example.Steps)
                .Count();
        }

        public string GetExecutionTime()
        {
            return ExecutionTime.ToString("ddd dd. MMM yyyy HH':'mm':'ss \"GMT\"z");
        }

        public string GetDuration()
        {
            if (Duration.Minutes > 0)
                return $"{Duration.Minutes}min {Duration.Seconds}s";

            return $"{Duration.Seconds}s {Duration.Milliseconds.ToString("D3")}ms";
        }

        public LivingDocFolder GetListOfFolderNodes()
        {
            var listOfFolders = new List<string>();

            foreach (var feature in Features)
            {
                if (!listOfFolders.Contains(feature.Uri))
                    listOfFolders.Add(feature.Uri);
            }

            return BuildTree(listOfFolders);
        }

        //public static List<string> GetListOfFolders(this LivingDocProject project)
        //{
        //    var listOfFolders = new List<string>();

        //    foreach (var feature in project.Features)
        //    {
        //        if (!listOfFolders.Contains(feature.Uri))
        //            listOfFolders.Add(feature.Uri);
        //    }

        //    return listOfFolders;
        //}

        public LivingDocFolder BuildTree(List<string> tokens)
        {
            var root = new LivingDocFolder("Root");

            foreach (var token in tokens)
            {
                var parts = token.Split('/');
                var current = root;

                foreach (var part in parts)
                {
                    if (!current.Children.ContainsKey(part))
                        current.Children[part] = new LivingDocFolder(part);

                    current = current.Children[part];
                }
            }

            return root;
        }
    }
}
