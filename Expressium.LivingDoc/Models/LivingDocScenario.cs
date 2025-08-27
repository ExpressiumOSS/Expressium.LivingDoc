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
            if (Examples.Any(example => example.GetStatus() == LivingDocStatuses.Failed.ToString()))
                return LivingDocStatuses.Failed.ToString();
            else if (Examples.Any(example => example.GetStatus() == LivingDocStatuses.Incomplete.ToString()))
                return LivingDocStatuses.Incomplete.ToString();
            else if (Examples.Count == 0 || Examples.Any(example => example.GetStatus() == LivingDocStatuses.Skipped.ToString()))
                return LivingDocStatuses.Skipped.ToString();
            else if (Examples.Count > 0 && Examples.TrueForAll(example => example.GetStatus() == LivingDocStatuses.Passed.ToString()))
                return LivingDocStatuses.Passed.ToString();
            else
            {
                return LivingDocStatuses.Unknown.ToString();
            }
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

        public int GetNumberOfTests()
        {
            return Examples.Count;
        }

        public string GetNumberOfTestsSortId()
        {
            return Examples.Count.ToString("D4");
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
