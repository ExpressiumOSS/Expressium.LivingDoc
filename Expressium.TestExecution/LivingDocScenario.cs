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
            if (GetStatus() == TestExecutionStatuses.Passed.ToString())
                return true;

            return false;
        }

        public bool IsIncomplete()
        {
            if (GetStatus() == TestExecutionStatuses.Incomplete.ToString())
                return true;

            return false;
        }

        public bool IsFailed()
        {
            if (GetStatus() == TestExecutionStatuses.Failed.ToString())
                return true;

            return false;
        }

        public bool IsSkipped()
        {
            if (GetStatus() == TestExecutionStatuses.Skipped.ToString())
                return true;

            return false;
        }

        public string GetStatus()
        {
            foreach (var example in Examples)
            {
                if (example.IsSkipped())
                    return TestExecutionStatuses.Skipped.ToString();
            }

            foreach (var example in Examples)
            {
                if (example.IsFailed())
                    return TestExecutionStatuses.Failed.ToString();
            }

            foreach (var example in Examples)
            {
                if (example.IsIncomplete())
                    return TestExecutionStatuses.Incomplete.ToString();
            }

            foreach (var example in Examples)
            {
                if (example.IsPassed())
                    return TestExecutionStatuses.Passed.ToString();
            }

            return TestExecutionStatuses.Undefined.ToString();
        }

        public string GetIndexSortId()
        {
            return Index.ToString("D4");
        }

        public string GetDuration()
        {
            TimeSpan? duration = null;

            foreach (var example in Examples)
            {
                if (duration == null)
                    duration = example.EndTime - example.StartTime;
                else
                    duration += example.EndTime - example.StartTime;
            }

            var totalDuration = duration.GetValueOrDefault();

            if (totalDuration.Minutes > 0)
                return $"{totalDuration.Minutes}min {totalDuration.Seconds}s";

            return $"{totalDuration.Seconds}s {totalDuration.Milliseconds.ToString("D3")}ms";
        }

        public string GetDurationSortId()
        {
            TimeSpan? duration = null;

            foreach (var example in Examples)
            {
                if (duration == null)
                    duration = example.EndTime - example.StartTime;
                else
                    duration += example.EndTime - example.StartTime;
            }

            var totalDuration = duration.GetValueOrDefault();

            return $"{totalDuration.Minutes.ToString("D2")}:{totalDuration.Seconds.ToString("D2")}:{totalDuration.Milliseconds.ToString("D3")}";
        }
    }
}
