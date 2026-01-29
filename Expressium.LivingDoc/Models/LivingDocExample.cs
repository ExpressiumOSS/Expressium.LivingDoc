using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocExample
    {
        public string Stacktrace { get; set; }
        public TimeSpan Duration { get; set; }

        public List<LivingDocStep> Steps { get; set; }
        public LivingDocDataTable DataTable { get; set; }
        public List<string> Attachments { get; set; }

        public LivingDocExample()
        {
            Duration = new TimeSpan();
            Steps = new List<LivingDocStep>();
            DataTable = new LivingDocDataTable();
            Attachments = new List<string>();
        }

        public string GetStatus()
        {
            if (Steps.Any(step => step.IsFailed()))
                return LivingDocStatuses.Failed.ToString();

            if (Steps.Any(step => step.IsIncomplete()))
                return LivingDocStatuses.Incomplete.ToString();

            if (Steps.Count == 0 || Steps.Any(step => step.IsSkipped()))
                return LivingDocStatuses.Skipped.ToString();

            if (Steps.Count > 0 && Steps.TrueForAll(step => step.IsPassed()))
                return LivingDocStatuses.Passed.ToString();

            return LivingDocStatuses.Unknown.ToString();
        }

        public string GetDuration()
        {
            return Duration.FormatAsString();
        }

        public bool HasDataTable()
        {
            return DataTable.Rows.Count > 0;
        }
    }
}
