using System;

namespace Expressium.TestExecution
{
    internal static class LivingDocStatus
    {
        public static bool IsPassed(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Passed.ToString().ToLower())
                return true;

            if (value == TestExecutionStatuses.OK.ToString())
                return true;

            return false;
        }

        public static bool IsIncomplete(this string value)
        {
            if (value.IsStepPending() || value.IsStepUndefined() || value.IsStepBindingError())
                return true;

            return false;
        }

        public static bool IsFailed(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Failed.ToString().ToLower())
                return true;

            if (value == TestExecutionStatuses.TestError.ToString())
                return true;

            return false;
        }

        public static bool IsSkipped(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Skipped.ToString().ToLower())
                return true;

            if (value == TestExecutionStatuses.Skipped.ToString())
                return true;

            return false;
        }

        public static bool IsStepPending(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Pending.ToString().ToLower())
                return true;

            if (value == TestExecutionStatuses.StepDefinitionPending.ToString())
                return true;

            return false;
        }

        public static bool IsStepUndefined(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Undefined.ToString().ToLower())
                return true;

            if (value == TestExecutionStatuses.UndefinedStep.ToString())
                return true;

            return false;
        }

        public static bool IsStepBindingError(this string value)
        {
            if (value == null)
                return false;

            if (value == TestExecutionStatuses.BindingError.ToString())
                return true;

            return false;
        }

        public static string GetStatus(this string value)
        {
            if (value.IsPassed())
                return ReportStatuses.Passed.ToString();
            else if (value.IsIncomplete())
                return ReportStatuses.Incomplete.ToString();
            else if (value.IsFailed())
                return ReportStatuses.Failed.ToString();
            else if (value.IsSkipped())
                return ReportStatuses.Skipped.ToString();
            else
            {
                return ReportStatuses.Undefined.ToString();
            }
        }
    }
}
