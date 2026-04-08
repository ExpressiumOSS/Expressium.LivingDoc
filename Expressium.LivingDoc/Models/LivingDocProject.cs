using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocProject
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }

        public string ProtocolVersion { get; set; }
        public string ImplementationName { get; set; }
        public string ImplementationVersion { get; set; }
        public string RuntimeName { get; set; }
        public string RuntimeVersion { get; set; }
        public string OsName { get; set; }
        public string OsVersion { get; set; }
        public string CpuName { get; set; }

        public List<LivingDocFeature> Features { get; set; }
        public List<LivingDocHistory> Histories { get; set; }

        internal bool ExperimentFlag { get; set; }

        public LivingDocProject()
        {
            Duration = new TimeSpan();
            Features = new List<LivingDocFeature>();
            Histories = new List<LivingDocHistory>();

            ExperimentFlag = false;
        }

        internal string GetApplicationName()
        {
            return "Expressium LivingDoc";
        }

        internal string GetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }

        public string GetDate()
        {
            return Date.FormatAsString();
        }

        public string GetDuration()
        {
            return Duration.FormatAsString();
        }

        public int GetNumberOfFeatures()
        {
            return Features.Count;
        }

        public int GetNumberOfScenarios()
        {
            return Features.Sum(feature => feature.GetNumberOfScenarios());
        }

        public int GetNumberOfRules()
        {
            return Features.Sum(feature => feature.GetNumberOfRules());
        }

        public int GetNumberOfSteps()
        {
            return Features.Sum(feature => feature.GetNumberOfSteps());
        }

        public int GetNumberOfPassedFeatures()
        {
            return Features.Count(feature => feature.IsPassed());
        }

        public int GetNumberOfIncompleteFeatures()
        {
            return Features.Count(feature => feature.IsIncomplete());
        }

        public int GetNumberOfFailedFeatures()
        {
            return Features.Count(feature => feature.IsFailed());
        }

        public int GetNumberOfSkippedFeatures()
        {
            return Features.Count(feature => feature.IsSkipped());
        }

        public int GetNumberOfPassedScenarios()
        {
            return Features.Sum(feature => feature.GetNumberOfPassedScenarios());
        }

        public int GetNumberOfIncompleteScenarios()
        {
            return Features.Sum(feature => feature.GetNumberOfIncompleteScenarios());
        }

        public int GetNumberOfFailedScenarios()
        {
            return Features.Sum(feature => feature.GetNumberOfFailedScenarios());
        }

        public int GetNumberOfSkippedScenarios()
        {
            return Features.Sum(feature => feature.GetNumberOfSkippedScenarios());
        }

        public int GetNumberOfPassedSteps()
        {
            return Features.Sum(feature => feature.GetNumberOfPassedSteps());
        }

        public int GetNumberOfIncompleteSteps()
        {
            return Features.Sum(feature => feature.GetNumberOfIncompleteSteps());
        }

        public int GetNumberOfFailedSteps()
        {
            return Features.Sum(feature => feature.GetNumberOfFailedSteps());
        }

        public int GetNumberOfSkippedSteps()
        {
            return Features.Sum(feature => feature.GetNumberOfSkippedSteps());
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

        public void Merge(LivingDocProject project)
        {
            foreach (var feature in project.Features)
            {
                if (!Features.Any(x => x.Name == feature.Name))
                    Features.Add(LivingDocExtensions.DeepClone(feature));
            }

            Duration += project.Duration;
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
                    if (feature.IsPassed())
                        livingDocHistory.Features.Passed.Add(feature.Name);

                    if (feature.IsIncomplete())
                        livingDocHistory.Features.Incomplete.Add(feature.Name);

                    if (feature.IsFailed())
                        livingDocHistory.Features.Failed.Add(feature.Name);

                    if (feature.IsSkipped())
                        livingDocHistory.Features.Skipped.Add(feature.Name);

                    foreach (var scenario in feature.Scenarios)
                    {
                        foreach (var example in scenario.Examples)
                        {
                            if (example.IsPassed())
                                livingDocHistory.Scenarios.Passed.Add(scenario.Name);

                            if (example.IsIncomplete())
                                livingDocHistory.Scenarios.Incomplete.Add(scenario.Name);

                            if (example.IsFailed())
                                livingDocHistory.Scenarios.Failed.Add(scenario.Name);

                            if (example.IsSkipped())
                                livingDocHistory.Scenarios.Skipped.Add(scenario.Name);

                            foreach (var step in example.Steps)
                            {
                                if (step.IsPassed())
                                    livingDocHistory.Steps.Passed.Add(step.Keyword + " " + step.Name);

                                if (step.IsIncomplete())
                                    livingDocHistory.Steps.Incomplete.Add(step.Keyword + " " + step.Name);

                                if (step.IsFailed())
                                    livingDocHistory.Steps.Failed.Add(step.Keyword + " " + step.Name);

                                if (step.IsSkipped())
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
