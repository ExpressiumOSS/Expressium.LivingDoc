using System;

namespace Expressium.LivingDoc.Models
{
    public enum LivingDocStatuses
    {
        Failed,
        Incomplete,
        Pending,
        Undefined,
        Ambiguous,
        Passed,
        Skipped,
        Unknown
    }

    public class LivingDocStep
    {
        public string Id { get; set; }
        public string TableBodyId { get; set; }
        public string Keyword { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public TimeSpan Duration { get; set; }
        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }

        public LivingDocDataTable DataTable { get; set; }

        public LivingDocStep()
        {
            DataTable = new LivingDocDataTable();
        }

        public LivingDocStep Copy(LivingDocStep step)
        {
            var copy = new LivingDocStep
            {
                Id = step.Id,
                Keyword = step.Keyword,
                Name = step.Name,
                Status = step.Status,
                Duration = step.Duration,
                Message = step.Message,
                DataTable = step.DataTable
            };

            return copy;
        }

        public bool IsPassed()
        {
            if (Status == null)
                return false;

            if (Status == LivingDocStatuses.Passed.ToString())
                return true;

            return false;
        }

        public bool IsIncomplete()
        {
            if (Status == null)
                return false;

            if (Status == LivingDocStatuses.Incomplete.ToString())
                return true;

            if (Status == LivingDocStatuses.Pending.ToString())
                return true;

            if (Status == LivingDocStatuses.Undefined.ToString())
                return true;

            if (Status == LivingDocStatuses.Ambiguous.ToString())
                return true;

            return false;
        }

        public bool IsFailed()
        {
            if (Status == null)
                return false;

            if (Status == LivingDocStatuses.Failed.ToString())
                return true;

            return false;
        }

        public bool IsSkipped()
        {
            if (Status == null)
                return true;

            if (Status == LivingDocStatuses.Skipped.ToString())
                return true;

            if (Status == LivingDocStatuses.Unknown.ToString())
                return true;

            return false;
        }

        public string GetStatus()
        {
            if (IsFailed())
                return LivingDocStatuses.Failed.ToString();
            else if (IsIncomplete())
                return LivingDocStatuses.Incomplete.ToString();
            else if (IsSkipped())
                return LivingDocStatuses.Skipped.ToString();
            else if (IsPassed())
                return LivingDocStatuses.Passed.ToString();
            else
            {
                return LivingDocStatuses.Undefined.ToString();
            }
        }
    }
}
