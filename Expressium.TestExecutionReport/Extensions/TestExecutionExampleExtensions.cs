using Expressium.TestExecution;

namespace Expressium.LivingDoc.Extensions
{
    public static class TestExecutionExampleExtensions
    {
        public static bool IsPassed(this LivingDocExample example)
        {
            return example.Status.IsPassed();
        }

        public static bool IsIncomplete(this LivingDocExample example)
        {
            return example.Status.IsIncomplete();
        }

        public static bool IsFailed(this LivingDocExample example)
        {
            return example.Status.IsFailed();
        }

        public static bool IsSkipped(this LivingDocExample example)
        {
            return example.Status.IsSkipped();
        }

        public static bool IsStepPending(this LivingDocExample example)
        {
            return example.Status.IsStepPending();
        }

        public static bool IsStepUndefined(this LivingDocExample example)
        {
            return example.Status.IsStepUndefined();
        }

        public static bool IsStepBindingError(this LivingDocExample example)
        {
            return example.Status.IsStepBindingError();
        }

        public static string GetStatus(this LivingDocExample example)
        {
            if (example.IsFailed())
                return ReportStatuses.Failed.ToString();
            else if (example.IsIncomplete())
                return ReportStatuses.Incomplete.ToString();
            else if (example.IsSkipped())
                return ReportStatuses.Skipped.ToString();
            else if (example.IsPassed())
                return ReportStatuses.Passed.ToString();

            return ReportStatuses.Undefined.ToString();
        }

        public static string GetDuration(this LivingDocExample example)
        {
            var duration = example.EndTime - example.StartTime;

            if (duration.Minutes > 0)
                return $"{duration.Minutes}min {duration.Seconds}s";

            return $"{duration.Seconds}s {duration.Milliseconds.ToString("D3")}ms";
        }
    }
}
