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
        public LivingDocProjectHistory History { get; set; }

        internal bool ExperimentFlagSymbols { get; set; }
        internal bool ExperimentFlagHealth { get; set; }

        public LivingDocProject()
        {
            Duration = new TimeSpan();

            Features = new List<LivingDocFeature>();
            History = new LivingDocProjectHistory();

            ExperimentFlagSymbols = false;
            ExperimentFlagHealth = false;
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
            var folders = new HashSet<string>();

            foreach (var folder in Features.Select(feature => feature.GetFolder()).Where(f => !string.IsNullOrWhiteSpace(f)))
            {
                folders.Add(folder);

                var tokens = folder.Split('\\');
                for (int i = 1; i < tokens.Length; i++)
                    folders.Add(string.Join("\\", tokens.Take(i)));
            }

            var listOfFolders = folders.OrderBy(f => f).Cast<string>().ToList();
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
            if (this.History.Features.Any(d => d.Date == livingDocProject.Date) ||
                 this.History.Scenarios.Any(d => d.Date == livingDocProject.Date) ||
                 this.History.Steps.Any(d => d.Date == livingDocProject.Date))
                return;

            MergeProjectHistoryResults(livingDocProject);
            MergeExampleHistoryResults(livingDocProject);

            if (ExperimentFlagHealth)
                MergeScenarioHistoryHealth();
        }

        internal void MergeProjectHistoryResults(LivingDocProject livingDocProject)
        {
            this.History.Features.Add(new LivingDocProjectHistoryResults
            {
                Date = livingDocProject.Date,
                Passed = livingDocProject.GetNumberOfPassedFeatures(),
                Incomplete = livingDocProject.GetNumberOfIncompleteFeatures(),
                Failed = livingDocProject.GetNumberOfFailedFeatures(),
                Skipped = livingDocProject.GetNumberOfSkippedFeatures()
            });

            this.History.Scenarios.Add(new LivingDocProjectHistoryResults
            {
                Date = livingDocProject.Date,
                Passed = livingDocProject.GetNumberOfPassedScenarios(),
                Incomplete = livingDocProject.GetNumberOfIncompleteScenarios(),
                Failed = livingDocProject.GetNumberOfFailedScenarios(),
                Skipped = livingDocProject.GetNumberOfSkippedScenarios()
            });

            this.History.Steps.Add(new LivingDocProjectHistoryResults
            {
                Date = livingDocProject.Date,
                Passed = livingDocProject.GetNumberOfPassedSteps(),
                Incomplete = livingDocProject.GetNumberOfIncompleteSteps(),
                Failed = livingDocProject.GetNumberOfFailedSteps(),
                Skipped = livingDocProject.GetNumberOfSkippedSteps()
            });
        }

        internal void MergeExampleHistoryResults(LivingDocProject livingDocProject)
        {
            foreach (var feature in Features)
            {
                var matchingFeature = livingDocProject.Features.FirstOrDefault(x => x.Name == feature.Name);
                if (matchingFeature != null)
                {
                    foreach (var scenario in feature.Scenarios)
                    {
                        var matchingScenario = matchingFeature.Scenarios.FirstOrDefault(x => x.Name == scenario.Name);
                        if (matchingScenario != null)
                        {
                            var indexId = 0;
                            foreach (var example in scenario.Examples)
                            {
                                if (matchingScenario.Examples.Count > indexId)
                                {
                                    var existingExample = example;
                                    existingExample.History.Add(new LivingDocExampleHistoryResults
                                    {
                                        Date = livingDocProject.Date,
                                        Status = matchingScenario.Examples[indexId].GetStatus()
                                    });
                                }

                                indexId++;
                            }
                        }
                    }
                }
            }
        }

        internal void MergeScenarioHistoryHealth()
        {
            foreach (var feature in Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    scenario.Health = null;

                    foreach (var example in scenario.Examples)
                    {
                        var numberOfHistories = example.History.Count;
                        if (numberOfHistories < 2)
                            continue;

                        if (scenario.Health != null && scenario.Health != LivingDocHealths.Fixed.ToString())
                            continue;

                        var prior = numberOfHistories >= 4 ? example.History[numberOfHistories - 4].Status : null;
                        var oldest = numberOfHistories >= 3 ? example.History[numberOfHistories - 3].Status : null;
                        var previous = example.History[numberOfHistories - 2].Status;
                        var newest = example.History[numberOfHistories - 1].Status;

                        var passed = LivingDocStatuses.Passed.ToString();
                        var failed = LivingDocStatuses.Failed.ToString();
                        var incomplete = LivingDocStatuses.Incomplete.ToString();
                        var skipped = LivingDocStatuses.Skipped.ToString();

                        var activeStatuses = new[] { prior, oldest, previous, newest }
                            .Where(s => s != null && s != skipped && s != incomplete)
                            .ToList();

                        // Flaky Pattern...
                        if ((newest == failed || newest == passed) &&
                            activeStatuses.Take(activeStatuses.Count - 1).Contains(passed) &&
                            activeStatuses.Take(activeStatuses.Count - 1).Contains(failed))
                        {
                            scenario.Health = LivingDocHealths.Flaky.ToString();
                            continue;
                        }

                        // Regressed Pattern...
                        if (newest == failed && previous == passed)
                            scenario.Health = LivingDocHealths.Regressed.ToString();

                        // Broken Pattern...
                        else if (newest == failed && (previous == skipped || previous == incomplete || previous == failed))
                            scenario.Health = LivingDocHealths.Broken.ToString();

                        // Fixed Pattern...
                        else if (newest == passed && previous != passed)
                            scenario.Health = LivingDocHealths.Fixed.ToString();
                    }
                }
            }
        }
    }
}
