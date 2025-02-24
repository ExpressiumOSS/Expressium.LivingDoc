using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class LivingDocExample
    {
        public string Stacktrace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<LivingDocStep> Steps { get; set; }
        public LivingDocTableRow TableHeader { get; set; }
        public List<LivingDocTableRow> TableBody { get; set; }
        public List<string> Attachments { get; set; }

        public LivingDocExample()
        {
            Steps = new List<LivingDocStep>();
            TableHeader = new LivingDocTableRow();
            TableBody = new List<LivingDocTableRow>();
            Attachments = new List<string>();
        }

        public bool IsPassed()
        {
            return Steps.TrueForAll(step => step.IsPassed());
        }

        public bool IsIncomplete()
        {
            return Steps.Any(step => step.IsIncomplete());
        }

        public bool IsFailed()
        {
            return Steps.Any(step => step.IsFailed());
        }

        public bool IsSkipped()
        {
            return Steps.Any(step => step.IsSkipped());
        }

        public string GetStatus()
        {
            if (IsFailed())
                return TestExecutionStatuses.Failed.ToString();
            else if (IsIncomplete())
                return TestExecutionStatuses.Incomplete.ToString();
            else if (IsSkipped())
                return TestExecutionStatuses.Skipped.ToString();
            else if (IsPassed())
                return TestExecutionStatuses.Passed.ToString();

            return TestExecutionStatuses.Undefined.ToString();
        }

        public string GetMessage()
        {
            if (Steps.Any(step => step.Message != null))
                return Steps.First(step => step.Message != null).Message;

            return null;
        }

        public string GetDuration()
        {
            var duration = EndTime - StartTime;

            if (duration.Minutes > 0)
                return $"{duration.Minutes}min {duration.Seconds}s";

            return $"{duration.Seconds}s {duration.Milliseconds.ToString("D3")}ms";
        }
    }
}
