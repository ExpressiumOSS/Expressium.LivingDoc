using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class LivingDocFeature
    {
        public string Id { get; set; }
        public List<LivingDocTag> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public string Uri { get; set; }

        public List<LivingDocBackground> Backgrounds { get; set; }
        public List<LivingDocScenario> Scenarios { get; set; }

        public LivingDocFeature()
        {
            Tags = new List<LivingDocTag>();
            Backgrounds = new List<LivingDocBackground>();
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

        public bool IsTagged()
        {
            if (Tags.Count == 0)
                return false;

            return true;
        }

        public string GetTags()
        {
            return string.Join(" ", Tags.Select(tag => tag.Name));
        }

        public string GetStatus()
        {
            foreach (var scenario in Scenarios)
            {
                if (scenario.IsSkipped())
                    return LivingDocStatuses.Skipped.ToString();
            }

            foreach (var scenario in Scenarios)
            {
                if (scenario.IsFailed())
                    return LivingDocStatuses.Failed.ToString();
            }

            foreach (var scenario in Scenarios)
            {
                if (scenario.IsIncomplete())
                    return LivingDocStatuses.Incomplete.ToString();
            }

            foreach (var scenario in Scenarios)
            {
                if (scenario.IsPassed())
                    return LivingDocStatuses.Passed.ToString();
            }

            return LivingDocStatuses.Undefined.ToString();
        }

        public int GetNumberOfPassed()
        {
            return Scenarios.Count(scenario => scenario.IsPassed());
        }

        public int GetNumberOfIncomplete()
        {
            return Scenarios.Count(scenario => scenario.IsIncomplete());
        }

        public int GetNumberOfFailed()
        {
            return Scenarios.Count(scenario => scenario.IsFailed());
        }

        public int GetNumberOfSkipped()
        {
            return Scenarios.Count(scenario => scenario.IsSkipped());
        }

        public int GetNumberOfTests()
        {
            return Scenarios.Count();
        }

        public int GetNumberOfScenarios()
        {
            return Scenarios.Count;
        }

        public string GetNumberOfScenariosSortId()
        {
            return Scenarios.Count.ToString("D4");
        }

        public int GetPercentageOfPassed()
        {
            return (int)Math.Round(100.0f / GetNumberOfTests() * GetNumberOfPassed());
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
    }
}
