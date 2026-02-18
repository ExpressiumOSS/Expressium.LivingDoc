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
        public List<LivingDocHistory> Histories { get; set; }

        public LivingDocProject()
        {
            Duration = new TimeSpan();
            Features = new List<LivingDocFeature>();
            Histories = new List<LivingDocHistory>();
        }

        public void Merge(LivingDocProject project)
        {
            foreach (var feature in project.Features)
            {
                if (!Features.Any(x => x.Name == feature.Name))
                    Features.Add(LivingDocSerializer.DeepClone(feature));
            }

            Duration += project.Duration;
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

        public void MergeHistory(LivingDocProject livingDocProject)
        {
            try
            {
                if (this.Histories.Any(d => d.Date == livingDocProject.Date))
                    return;

                var livingDocHistory = new LivingDocHistory();
                livingDocHistory.Date = livingDocProject.Date;

                foreach (var feature in livingDocProject.Features)
                {
                    if (feature.GetStatus() == LivingDocStatuses.Passed.ToString())
                        livingDocHistory.Features.Passed.Add(feature.Name);

                    if (feature.GetStatus() == LivingDocStatuses.Incomplete.ToString())
                        livingDocHistory.Features.Incomplete.Add(feature.Name);

                    if (feature.GetStatus() == LivingDocStatuses.Failed.ToString())
                        livingDocHistory.Features.Failed.Add(feature.Name);

                    if (feature.GetStatus() == LivingDocStatuses.Skipped.ToString())
                        livingDocHistory.Features.Skipped.Add(feature.Name);

                    foreach (var scenario in feature.Scenarios)
                    {
                        if (scenario.GetStatus() == LivingDocStatuses.Passed.ToString())
                            livingDocHistory.Scenarios.Passed.Add(scenario.Name);

                        if (scenario.GetStatus() == LivingDocStatuses.Incomplete.ToString())
                            livingDocHistory.Scenarios.Incomplete.Add(scenario.Name);

                        if (scenario.GetStatus() == LivingDocStatuses.Failed.ToString())
                            livingDocHistory.Scenarios.Failed.Add(scenario.Name);

                        if (scenario.GetStatus() == LivingDocStatuses.Skipped.ToString())
                            livingDocHistory.Scenarios.Skipped.Add(scenario.Name);

                        foreach (var example in scenario.Examples)
                        {
                            foreach (var step in example.Steps)
                            {
                                if (step.GetStatus() == LivingDocStatuses.Passed.ToString())
                                    livingDocHistory.Steps.Passed.Add(step.Keyword + " " + step.Name);

                                if (step.GetStatus() == LivingDocStatuses.Incomplete.ToString())
                                    livingDocHistory.Steps.Incomplete.Add(step.Keyword + " " + step.Name);

                                if (step.GetStatus() == LivingDocStatuses.Failed.ToString())
                                    livingDocHistory.Steps.Failed.Add(step.Keyword + " " + step.Name);

                                if (step.GetStatus() == LivingDocStatuses.Skipped.ToString())
                                    livingDocHistory.Steps.Skipped.Add(step.Keyword + " " + step.Name);
                            }
                        }
                    }
                }

                Histories.Add(livingDocHistory);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Unexpected error: {ex.Message}", ex);
            }
        }

        public int GetMaximumNumberOfHistoryFeatures()
        {
            return Histories.Max(h => h.GetNumberOfFeatures());
        }

        public int GetMaximumNumberOfHistoryScenarios()
        {
            return Histories.Max(h => h.GetNumberOfScenarios());
        }

        public int GetMaximumNumberOfHistorySteps()
        {
            return Histories.Max(h => h.GetNumberOfSteps());
        }

        private const int FailedWeight = 100;
        private const int IncompleteWeight = 10;
        private const int SkippedWeight = 1;
        private const int PassedWeight = 0;
        private const int ReportWeight = FailedWeight;

        public List<string> GetHistoryFeatureFailures()
        {
            var mapOfFailures = Histories
                .SelectMany(history =>
                    history.Features.Failed.Select(name => (name, FailedWeight))
                    .Concat(history.Features.Incomplete.Select(name => (name, IncompleteWeight)))
                    .Concat(history.Features.Skipped.Select(name => (name, SkippedWeight)))
                    .Concat(history.Features.Passed.Select(name => (name, PassedWeight))))
                .GroupBy(x => x.name)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2));

            var orderedFailures = mapOfFailures.OrderByDescending(x => x.Value).Where(d => d.Value >= ReportWeight);

            return orderedFailures.Select(x => x.Key).ToList();
        }

        public List<string> GetHistoryScenarioFailures()
        {
            var mapOfFailures = Histories
                .SelectMany(history =>
                    history.Scenarios.Failed.Select(name => (name, FailedWeight))
                    .Concat(history.Scenarios.Incomplete.Select(name => (name, IncompleteWeight)))
                    .Concat(history.Scenarios.Skipped.Select(name => (name, SkippedWeight)))
                    .Concat(history.Scenarios.Passed.Select(name => (name, PassedWeight))))
                .GroupBy(x => x.name)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2));

            var orderedFailures = mapOfFailures.OrderByDescending(x => x.Value).Where(d => d.Value >= ReportWeight);

            return orderedFailures.Select(x => x.Key).ToList();
        }

        public List<string> GetHistoryStepFailures()
        {
            var mapOfFailures = Histories
                .SelectMany(history =>
                    history.Steps.Failed.Select(name => (name, FailedWeight))
                    .Concat(history.Steps.Incomplete.Select(name => (name, IncompleteWeight)))
                    .Concat(history.Steps.Skipped.Select(name => (name, SkippedWeight)))
                    .Concat(history.Steps.Passed.Select(name => (name, PassedWeight))))
                .GroupBy(x => x.name)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2));

            var orderedFailures = mapOfFailures.OrderByDescending(x => x.Value).Where(d => d.Value >= ReportWeight);

            return orderedFailures.Select(x => x.Key).ToList();
        }
    }
}
