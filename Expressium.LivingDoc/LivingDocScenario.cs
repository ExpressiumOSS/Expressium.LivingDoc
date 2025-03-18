using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc
{
    public class LivingDocScenario
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public List<LivingDocTag> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }

        public List<LivingDocExample> Examples { get; set; }

        public LivingDocScenario()
        {
            Index = 0;
            Tags = new List<LivingDocTag>();
            Examples = new List<LivingDocExample>();
        }

        public string GetTags()
        {
            return string.Join(" ", Tags.Select(tag => tag.Name));
        }

        public string GetStatus()
        {
            if (Examples.Any(example => example.GetStatus() == LivingDocStatuses.Failed.ToString()))
                return LivingDocStatuses.Failed.ToString();
            else if (Examples.Any(example => example.GetStatus() == LivingDocStatuses.Incomplete.ToString()))
                return LivingDocStatuses.Incomplete.ToString();
            else if (Examples.Any(example => example.GetStatus() == LivingDocStatuses.Skipped.ToString()))
                return LivingDocStatuses.Skipped.ToString();
            else if (Examples.TrueForAll(example => example.GetStatus() == LivingDocStatuses.Passed.ToString()))
                return LivingDocStatuses.Passed.ToString();
            else
            {
                return LivingDocStatuses.Undefined.ToString();
            }
        }

        public string GetIndexSortId()
        {
            return Index.ToString("D4");
        }

        public string GetDuration()
        {
            var duration = new TimeSpan();

            foreach (var example in Examples)
                duration += example.Duration;

            if (duration.Minutes > 0)
                return $"{duration.Minutes}min {duration.Seconds}s";

            return $"{duration.Seconds}s {duration.Milliseconds.ToString("D3")}ms";
        }

        public string GetDurationSortId()
        {
            var duration = new TimeSpan();

            foreach (var example in Examples)
                duration += example.Duration;

            return $"{duration.Minutes.ToString("D2")}:{duration.Seconds.ToString("D2")}:{duration.Milliseconds.ToString("D3")}";
        }

        public string GetPercentageOfPassedSortId()
        {
            return GetPercentageOfPassed().ToString("D4");
        }

        public int GetPercentageOfPassed()
        {
            var numberOfSteps = 0;
            var numberOfPassedSteps = 0;

            foreach (var example in Examples)
            {
                numberOfSteps += example.Steps.Count;
                numberOfPassedSteps += example.Steps.Count(step => step.IsPassed());
            }

            return (int)Math.Round(100.0f / numberOfSteps * numberOfPassedSteps);
        }

        public int GetNumberOfSteps()
        {
            var numberOfSteps = 0;

            foreach (var example in Examples)
                numberOfSteps += example.Steps.Count;

            return numberOfSteps;
        }

        public string GetNumberOfStepsSortId()
        {
            return GetNumberOfSteps().ToString("D4");
        }
    }
}
