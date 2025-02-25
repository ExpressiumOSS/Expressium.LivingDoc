using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class LivingDocScenario
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public List<LivingDocTag> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public string Type { get; set; }

        public List<LivingDocExample> Examples { get; set; }

        public LivingDocScenario()
        {
            Index = 0;
            Tags = new List<LivingDocTag>();
            Examples = new List<LivingDocExample>();
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

        public bool IsPassed()
        {
            if (GetStatus() == LivingDocStatuses.Passed.ToString())
                return true;

            return false;
        }

        public bool IsIncomplete()
        {
            if (GetStatus() == LivingDocStatuses.Incomplete.ToString())
                return true;

            return false;
        }

        public bool IsFailed()
        {
            if (GetStatus() == LivingDocStatuses.Failed.ToString())
                return true;

            return false;
        }

        public bool IsSkipped()
        {
            if (GetStatus() == LivingDocStatuses.Skipped.ToString())
                return true;

            return false;
        }

        public string GetStatus()
        {
            foreach (var example in Examples)
            {
                if (example.IsSkipped())
                    return LivingDocStatuses.Skipped.ToString();
            }

            foreach (var example in Examples)
            {
                if (example.IsFailed())
                    return LivingDocStatuses.Failed.ToString();
            }

            foreach (var example in Examples)
            {
                if (example.IsIncomplete())
                    return LivingDocStatuses.Incomplete.ToString();
            }

            foreach (var example in Examples)
            {
                if (example.IsPassed())
                    return LivingDocStatuses.Passed.ToString();
            }

            return LivingDocStatuses.Undefined.ToString();
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
    }
}
