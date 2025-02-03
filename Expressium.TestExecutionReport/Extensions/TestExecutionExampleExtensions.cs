using Expressium.TestExecution;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionExampleExtensions
    {
        public static bool IsPassed(this TestExecutionExample example)
        {
            return example.Status.IsPassed();
        }

        public static bool IsIncomplete(this TestExecutionExample example)
        {
            return example.Status.IsIncomplete();
        }

        public static bool IsFailed(this TestExecutionExample example)
        {
            return example.Status.IsFailed();
        }

        public static bool IsSkipped(this TestExecutionExample example)
        {
            return example.Status.IsSkipped();
        }

        public static bool IsStepPending(this TestExecutionExample example)
        {
            return example.Status.IsStepPending();
        }

        public static bool IsStepUndefined(this TestExecutionExample example)
        {
            return example.Status.IsStepUndefined();
        }

        public static bool IsStepBindingError(this TestExecutionExample example)
        {
            return example.Status.IsStepBindingError();
        }

        public static string GetStatus(this TestExecutionExample example)
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

        public static string GetDuration(this TestExecutionExample example)
        {
            var duration = example.EndTime - example.StartTime;

            if (duration.Minutes > 0)
                return $"{duration.Minutes}min {duration.Seconds}s {duration.Milliseconds}ms";

            return $"{duration.Seconds}s {duration.Milliseconds}ms";
        }
    }
}
