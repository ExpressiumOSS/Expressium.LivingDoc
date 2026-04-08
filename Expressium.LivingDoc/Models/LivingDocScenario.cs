using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocScenario
    {
        public string Id { get; set; }
        public string RuleId { get; set; }
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Order { get; set; }

        public List<LivingDocExample> Examples { get; set; }

        public LivingDocScenario()
        {
            Id = Guid.NewGuid().ToString();
            Order = 0;

            Tags = new List<string>();
            Examples = new List<LivingDocExample>();
        }

        public string GetTags()
        {
            return string.Join(" ", Tags);
        }

        public string GetStatus()
        {
            if (Examples.Any(example => example.IsFailed()))
                return LivingDocStatuses.Failed.ToString();

            if (Examples.Any(example => example.IsIncomplete()))
                return LivingDocStatuses.Incomplete.ToString();

            if (Examples.Count == 0 || Examples.Any(example => example.IsSkipped()))
                return LivingDocStatuses.Skipped.ToString();

            if (Examples.Count > 0 && Examples.TrueForAll(example => example.IsPassed()))
                return LivingDocStatuses.Passed.ToString();

            return LivingDocStatuses.Unknown.ToString();
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

        public int GetNumberOfPassedExamples()
        {
            return Examples.Count(e => e.IsPassed());
        }

        public int GetNumberOfIncompleteExamples()
        {
            return Examples.Count(e => e.IsIncomplete());
        }

        public int GetNumberOfFailedExamples()
        {
            return Examples.Count(e => e.IsFailed());
        }

        public int GetNumberOfSkippedExamples()
        {
            return Examples.Count(e => e.IsSkipped());
        }

        public int GetNumberOfPassedSteps()
        {
            return Examples.Sum(e => e.Steps.Count(s => s.IsPassed()));
        }

        public int GetNumberOfIncompleteSteps()
        {
            return Examples.Sum(e => e.Steps.Count(s => s.IsIncomplete()));
        }

        public int GetNumberOfFailedSteps()
        {
            return Examples.Sum(e => e.Steps.Count(s => s.IsFailed()));
        }

        public int GetNumberOfSkippedSteps()
        {
            return Examples.Sum(e => e.Steps.Count(s => s.IsSkipped()));
        }

        public TimeSpan GetSumOfDuration()
        {
            var duration = new TimeSpan();

            foreach (var example in Examples)
                duration += example.Duration;

            return duration;
        }

        public string GetDuration()
        {
            var duration = GetSumOfDuration();
            return duration.FormatAsString();
        }

        public string GetDurationSortId()
        {
            var duration = GetSumOfDuration();
            return $"{duration.Minutes.ToString("D2")}:{duration.Seconds.ToString("D2")}:{duration.Milliseconds.ToString("D3")}";
        }

        public int GetOrder()
        {
            return Order;
        }

        public string GetOrderSortId()
        {
            return Order.ToString("D4");
        }

        public bool HasDataTable()
        {
            if (Examples.Any(example => example.HasDataTable()))
                return true;

            return false;
        }
    }
}
