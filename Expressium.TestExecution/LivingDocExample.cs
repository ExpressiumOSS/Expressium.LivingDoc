using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class LivingDocExample
    {
        public string Stacktrace { get; set; }
        public TimeSpan Duration { get; set; }

        public List<LivingDocStep> Steps { get; set; }
        public LivingDocTableRow TableHeader { get; set; }
        public List<LivingDocTableRow> TableBody { get; set; }
        public List<string> Attachments { get; set; }

        public LivingDocExample()
        {
            Duration = new TimeSpan();
            Steps = new List<LivingDocStep>();
            TableHeader = new LivingDocTableRow();
            TableBody = new List<LivingDocTableRow>();
            Attachments = new List<string>();
        }

        public string GetStatus()
        {
            if (Steps.Any(step => step.IsFailed()))
                return LivingDocStatuses.Failed.ToString();
            else if (Steps.Any(step => step.IsIncomplete()))
                return LivingDocStatuses.Incomplete.ToString();
            else if (Steps.Any(step => step.IsSkipped()))
                return LivingDocStatuses.Skipped.ToString();
            else if (Steps.TrueForAll(step => step.IsPassed()))
                return LivingDocStatuses.Passed.ToString();

            return LivingDocStatuses.Undefined.ToString();
        }

        public string GetMessage()
        {
            if (Steps.Any(step => step.Message != null))
                return Steps.First(step => step.Message != null).Message;

            return null;
        }

        public string GetDuration()
        {
            if (Duration.Minutes > 0)
                return $"{Duration.Minutes}min {Duration.Seconds}s";

            return $"{Duration.Seconds}s {Duration.Milliseconds.ToString("D3")}ms";
        }
    }
}
