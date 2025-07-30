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

        public bool IsScenarioAdded(string title)
        {
            return Scenarios.Any(m => m.Name == title);
        }

        public LivingDocScenario GetScenario(string title)
        {
            return Scenarios.Find(x => x.Name == title);
        }

        public string GetTags()
        {
            return string.Join(" ", Tags);
        }

        public string GetStatus()
        {
            if (Scenarios.Any(example => example.GetStatus() == LivingDocStatuses.Failed.ToString()))
                return LivingDocStatuses.Failed.ToString();
            else if (Scenarios.Any(example => example.GetStatus() == LivingDocStatuses.Incomplete.ToString()))
                return LivingDocStatuses.Incomplete.ToString();
            else if (Scenarios.Count == 0 || Scenarios.Any(example => example.GetStatus() == LivingDocStatuses.Skipped.ToString()))
                return LivingDocStatuses.Skipped.ToString();
            else if (Scenarios.Count > 0 && Scenarios.TrueForAll(example => example.GetStatus() == LivingDocStatuses.Passed.ToString()))
                return LivingDocStatuses.Passed.ToString();
            else
            {
                return LivingDocStatuses.Unknown.ToString();
            }
        }

        public int GetNumberOfScenarios()
        {
            return Scenarios.Count;
        }

        public string GetNumberOfScenariosSortId()
        {
            return GetNumberOfScenarios().ToString("D4");
        }

        public int GetPercentageOfPassed()
        {
            var numberOfPassed = Scenarios.Count(scenario => scenario.GetStatus() == LivingDocStatuses.Passed.ToString());

            if (Scenarios.Count == 0)
                return 0;

            return (int)Math.Round(100.0f / Scenarios.Count * numberOfPassed);
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

            if (duration.Minutes > 0)
                return $"{duration.Minutes}min {duration.Seconds}s";

            return $"{duration.Seconds}s {duration.Milliseconds.ToString("D3")}ms";
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

        public int GetFolderDepth()
        {
            var depth = 0;

            var folder = GetFolder();

            if (string.IsNullOrWhiteSpace(folder))
                return 0;

            var tokens = folder.Split('\\');
            if (tokens.Length > depth)
                depth = tokens.Length;

            return depth;
        }
    }
}
