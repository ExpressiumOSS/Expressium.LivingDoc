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

        public string GetTags()
        {
            return string.Join(" ", Tags);
        }

        public string GetStatus()
        {
            if (Scenarios.Any(example => example.GetStatus() == LivingDocStatuses.Failed.ToString()))
                return LivingDocStatuses.Failed.ToString();

            if (Scenarios.Any(example => example.GetStatus() == LivingDocStatuses.Incomplete.ToString()))
                return LivingDocStatuses.Incomplete.ToString();

            if (Scenarios.Count == 0 || Scenarios.Any(example => example.GetStatus() == LivingDocStatuses.Skipped.ToString()))
                return LivingDocStatuses.Skipped.ToString();

            if (Scenarios.Count > 0 && Scenarios.TrueForAll(example => example.GetStatus() == LivingDocStatuses.Passed.ToString()))
                return LivingDocStatuses.Passed.ToString();

            return LivingDocStatuses.Unknown.ToString();
        }

        public int GetNumberOfPassedScenarios()
        {
            return Scenarios.SelectMany(s => s.Examples).Count(e => e.GetStatus() == LivingDocStatuses.Passed.ToString());
        }

        public int GetNumberOfFailedScenarios()
        {
            return Scenarios.SelectMany(s => s.Examples).Count(e => e.GetStatus() == LivingDocStatuses.Failed.ToString());
        }

        public int GetNumberOfIncompleteScenarios()
        {
            return Scenarios.SelectMany(s => s.Examples).Count(e => e.GetStatus() == LivingDocStatuses.Incomplete.ToString());
        }

        public int GetNumberOfSkippedScenarios()
        {
            return Scenarios.SelectMany(s => s.Examples).Count(e => e.GetStatus() == LivingDocStatuses.Skipped.ToString());
        }

        public int GetNumberOfScenarios()
        {
            return Scenarios.SelectMany(scenario => scenario.Examples).Count();
        }

        public string GetNumberOfScenariosSortId()
        {
            return GetNumberOfScenarios().ToString("D4");
        }

        public int GetPercentageOfPassed()
        {
            var numberOfScenarios = GetNumberOfScenarios();
            if (numberOfScenarios == 0)
                return 0;

            var numberOfPassed = GetNumberOfPassedScenarios();

            return (int)Math.Round(100.0f / numberOfScenarios * numberOfPassed);
        }

        public string GetPercentageOfPassedSortId()
        {
            return GetPercentageOfPassed().ToString("D4");
        }

        public string GetDuration()
        {
            var duration = new TimeSpan();

            foreach (var scenario in Scenarios)
            {
                foreach (var example in scenario.Examples)
                    duration += example.Duration;
            }

            return duration.FormatAsString();
        }

        public string GetDurationSortId()
        {
            var duration = new TimeSpan();

            foreach (var scenario in Scenarios)
            {
                foreach (var example in scenario.Examples)
                    duration += example.Duration;
            }

            return $"{duration.Minutes.ToString("D2")}:{duration.Seconds.ToString("D2")}:{duration.Milliseconds.ToString("D3")}";
        }

        public string GetFolder()
        {
            if (!string.IsNullOrWhiteSpace(Uri))
            {
                var folders = Path.GetDirectoryName(Uri).Replace("/", "\\");
                if (string.IsNullOrWhiteSpace(folders))
                    folders = null;

                return folders;
            }

            return null;
        }
    }
}
