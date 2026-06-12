using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocFeature
    {
        public string Id { get; set; }
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public string Uri { get; set; }

        public LivingDocBackground Background { get; set; }
        public List<LivingDocRule> Rules { get; set; }
        public List<LivingDocScenario> Scenarios { get; set; }

        public LivingDocFeature()
        {
            Id = Guid.NewGuid().ToString();

            Tags = new List<string>();
            Rules = new List<LivingDocRule>();
            Scenarios = new List<LivingDocScenario>();
        }

        public string GetDataTags()
        {
            return string.Join(" ", Tags);
        }

        public string GetDataStatus()
        {
            return "@" + GetStatus();
        }

        public string GetStatus()
        {
            if (Scenarios.Any(s => s.IsFailed()))
                return LivingDocStatuses.Failed.ToString();

            if (Scenarios.Any(s => s.IsIncomplete()))
                return LivingDocStatuses.Incomplete.ToString();

            if (Scenarios.Count == 0 || Scenarios.Any(s => s.IsSkipped()))
                return LivingDocStatuses.Skipped.ToString();

            if (Scenarios.Count > 0 && Scenarios.TrueForAll(s => s.IsPassed()))
                return LivingDocStatuses.Passed.ToString();

            return LivingDocStatuses.Unknown.ToString();
        }

        public string GetStatusSortId()
        {
            var status = GetStatus();

            if (status == LivingDocStatuses.Failed.ToString())
                return "1";
            else if (status == LivingDocStatuses.Incomplete.ToString())
                return "2";
            else if (status == LivingDocStatuses.Passed.ToString())
                return "3";
            else if (status == LivingDocStatuses.Skipped.ToString())
                return "4";

            return "5";
        }

        public bool IsPassed()
        {
            return GetStatus() == LivingDocStatuses.Passed.ToString();
        }

        public bool IsIncomplete()
        {
            return GetStatus() == LivingDocStatuses.Incomplete.ToString();
        }

        public bool IsFailed()
        {
            return GetStatus() == LivingDocStatuses.Failed.ToString();
        }

        public bool IsSkipped()
        {
            return GetStatus() == LivingDocStatuses.Skipped.ToString();
        }

        public int GetNumberOfScenarios()
        {
            return Scenarios.Sum(s => s.Examples.Count);
        }

        public int GetNumberOfRules()
        {
            return Scenarios.Select(s => s.RuleId).Where(ruleId => ruleId != null).Distinct().Count();
        }

        public int GetNumberOfSteps()
        {
            return Scenarios.Sum(s => s.Examples.Sum(e => e.Steps.Count));
        }

        public int GetNumberOfPassedScenarios()
        {
            return Scenarios.Sum(s => s.GetNumberOfPassedExamples());
        }

        public int GetNumberOfIncompleteScenarios()
        {
            return Scenarios.Sum(s => s.GetNumberOfIncompleteExamples());
        }

        public int GetNumberOfFailedScenarios()
        {
            return Scenarios.Sum(s => s.GetNumberOfFailedExamples());
        }

        public int GetNumberOfSkippedScenarios()
        {
            return Scenarios.Sum(s => s.GetNumberOfSkippedExamples());
        }

        public int GetNumberOfPassedSteps()
        {
            return Scenarios.Sum(s => s.GetNumberOfPassedSteps());
        }

        public int GetNumberOfIncompleteSteps()
        {
            return Scenarios.Sum(s => s.GetNumberOfIncompleteSteps());
        }

        public int GetNumberOfFailedSteps()
        {
            return Scenarios.Sum(s => s.GetNumberOfFailedSteps());
        }

        public int GetNumberOfSkippedSteps()
        {
            return Scenarios.Sum(s => s.GetNumberOfSkippedSteps());
        }

        public string GetNumberOfScenariosSortId()
        {
            return GetNumberOfScenarios().ToString("D4");
        }

        public TimeSpan GetSumOfDuration()
        {
            var duration = new TimeSpan();

            foreach (var scenario in Scenarios)
                duration += scenario.GetSumOfDuration();

            return duration;
        }

        public string GetDuration()
        {
            return GetSumOfDuration().FormatAsString();
        }

        public string GetDurationSortId()
        {
            var duration = GetSumOfDuration();
            return $"{duration.Minutes.ToString("D2")}:{duration.Seconds.ToString("D2")}:{duration.Milliseconds.ToString("D3")}";
        }

        public int GetPassRate()
        {
            var numberOfPassed = GetNumberOfPassedScenarios();
            var numberOfFailed = GetNumberOfFailedScenarios();

            var numberOfScenarios = numberOfPassed + numberOfFailed;
            if (numberOfScenarios == 0)
                return 0;

            return (int)Math.Round(numberOfPassed * 100.0 / numberOfScenarios, MidpointRounding.AwayFromZero);
        }

        public string GetPassRateSortId()
        {
            return GetPassRate().ToString("D3");
        }

        public int GetFlakyRate()
        {
            var numberOfScenarios = GetNumberOfScenarios();
            if (numberOfScenarios == 0)
                return 0;

            var flakyScenarios = Scenarios.Sum(s => (s.IsFlaky() ? 1 : 0) * s.Examples.Count);

            return (int)Math.Round(flakyScenarios * 100.0 / numberOfScenarios, MidpointRounding.AwayFromZero);
        }

        public string GetFlakyRateSortId()
        {
            return GetFlakyRate().ToString("D3");
        }

        public int GetCoverage()
        {
            var numberOfScenarios = GetNumberOfScenarios();
            if (numberOfScenarios == 0)
                return 0;

            var numberOfPassed = GetNumberOfPassedScenarios();
            var numberOfFailed = GetNumberOfFailedScenarios();

            return (int)Math.Round(100.0f / numberOfScenarios * (numberOfPassed + numberOfFailed), MidpointRounding.AwayFromZero);
        }

        public string GetCoverageSortId()
        {
            return GetCoverage().ToString("D3");
        }

        public string GetFolder()
        {
            if (!string.IsNullOrWhiteSpace(Uri))
            {
                var folders = Path.GetDirectoryName(Uri)?.Replace("/", "\\");
                if (string.IsNullOrWhiteSpace(folders))
                    folders = null;

                return folders;
            }

            return null;
        }

        public bool HasBackground()
        {
            return Background != null && Background.Steps.Count > 0;
        }
    }
}
