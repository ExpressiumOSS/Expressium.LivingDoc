using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocProject
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }

        public List<LivingDocFeature> Features { get; set; }

        public LivingDocProject()
        {
            Duration = new TimeSpan();
            Features = new List<LivingDocFeature>();
        }

        internal void Merge(LivingDocProject project)
        {
            foreach (var feature in project.Features)
            {
                if (!Features.Any(x => x.Name == feature.Name))
                    Features.Add(LivingDocSerializer.DeepClone(feature));
            }
        }

        public int GetNumberOfFeatures()
        {
            return Features.Count;
        }

        public int GetNumberOfScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count();
        }

        public int GetNumberOfRules()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Where(scenario => scenario.RuleId != null)
                .Select(scenario => scenario.RuleId)
                .Distinct()
                .Count();
        }

        public int GetNumberOfExamples()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .SelectMany(scenario => scenario.Examples)
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

        public int GetNumberOfFailedFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Failed.ToString());
        }

        public int GetNumberOfIncompleteFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Incomplete.ToString());
        }

        public int GetNumberOfPassedFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Passed.ToString());
        }

        public int GetNumberOfSkippedFeatures()
        {
            return Features.Count(feature => feature.GetStatus() == LivingDocStatuses.Skipped.ToString());
        }

        public int GetNumberOfFailedScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Failed.ToString());
        }

        public int GetNumberOfIncompleteScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Incomplete.ToString());
        }

        public int GetNumberOfPassedScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Passed.ToString());
        }

        public int GetNumberOfSkippedScenarios()
        {
            return Features
                .SelectMany(feature => feature.Scenarios)
                .Count(scenario => scenario.GetStatus() == LivingDocStatuses.Skipped.ToString());
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

        public string GetDate()
        {
            return Date.FormatAsString();
        }

        public string GetDuration()
        {
            return Duration.FormatAsString();
        }

        public List<string> GetFolders()
        {
            var listOfFolders = Features
            .Select(feature => feature.GetFolder())
            .Distinct()
            .Where(folder => !string.IsNullOrWhiteSpace(folder))
            .OrderBy(folder => folder)
            .ToList();

            var listOfExpandedFolders = new List<string>();
            foreach (var folder in listOfFolders)
            {
                if (!string.IsNullOrWhiteSpace(folder) && folder.Contains("\\"))
                {
                    var tokens = folder.Split('\\');
                    for (int i = 0; i < tokens.Length - 1; i++)
                    {
                        var expandedFolder = string.Join("\\", tokens.Take(i + 1));
                        if (!listOfFolders.Contains(expandedFolder))
                            listOfExpandedFolders.Add(expandedFolder);
                    }
                }
            }

            foreach (var folder in listOfExpandedFolders)
            {
                if (!listOfFolders.Contains(folder))
                    listOfFolders.Add(folder);
            }

            listOfFolders.Sort();
            listOfFolders.Add(null);

            return listOfFolders;
        }
    }
}
