using ReqnRoll.TestExecution;

namespace ReqnRoll.TestExecutionReport.Extensions
{
    public static class TestExecutionExampleExtensions
    {
        public static bool IsPassed(this TestExecutionExample example)
        {
            return example.Status.IsPassed();
        }

        public static bool IsFailed(this TestExecutionExample example)
        {
            return example.Status.IsFailed();
        }

        public static bool IsSkipped(this TestExecutionExample example)
        {
            return example.Status.IsSkipped();
        }

        public static bool IsInconclusive(this TestExecutionExample example)
        {
            return example.Status.IsInconclusive();
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

            if (example.IsInconclusive())
                return ReportStatuses.Inconclusive.ToString();

            if (example.IsSkipped())
                return ReportStatuses.Skipped.ToString();

            if (example.IsPassed())
                return ReportStatuses.Passed.ToString();

            return ReportStatuses.Undefined.ToString();
        }

        public static string GetDuration(this TestExecutionExample example)
        {
            return $"{example.Duration.Seconds}s {example.Duration.Milliseconds}ms";
        }
    }
}
