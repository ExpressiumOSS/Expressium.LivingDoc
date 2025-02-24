using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class LivingDocExample
    {
        public string Status { get; set; }
        public string Error { get; set; }
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
            return Status.IsPassed();
        }

        public bool IsIncomplete()
        {
            return Status.IsIncomplete();
        }

        public bool IsFailed()
        {
            return Status.IsFailed();
        }

        public bool IsSkipped()
        {
            return Status.IsSkipped();
        }

        public bool IsStepPending()
        {
            return Status.IsStepPending();
        }

        public bool IsStepUndefined()
        {
            return Status.IsStepUndefined();
        }

        public bool IsStepBindingError()
        {
            return Status.IsStepBindingError();
        }

        public string GetStatus()
        {
            if (IsFailed())
                return ReportStatuses.Failed.ToString();
            else if (IsIncomplete())
                return ReportStatuses.Incomplete.ToString();
            else if (IsSkipped())
                return ReportStatuses.Skipped.ToString();
            else if (IsPassed())
                return ReportStatuses.Passed.ToString();

            return ReportStatuses.Undefined.ToString();
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
