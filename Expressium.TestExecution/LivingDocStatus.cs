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

            return false;
        }

        public static bool IsIncomplete(this string value)
        {
            if (value.IsPending() || value.IsUndefined() || value.IsAmbiguous())
                return true;

            return false;
        }

        public static bool IsFailed(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Failed.ToString().ToLower())
                return true;

            return false;
        }

        public static bool IsSkipped(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Skipped.ToString().ToLower())
                return true;

            return false;
        }

        public static bool IsPending(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Pending.ToString().ToLower())
                return true;

            return false;
        }

        public static bool IsUndefined(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Undefined.ToString().ToLower())
                return true;

            return false;
        }

        public static bool IsAmbiguous(this string value)
        {
            if (value == null)
                return false;

            if (value.ToLower() == TestExecutionStatuses.Ambiguous.ToString().ToLower())
                return true;

            return false;
        }

        public static string GetStatus(this string value)
        {
            if (value.IsPassed())
                return TestExecutionStatuses.Passed.ToString();
            else if (value.IsIncomplete())
                return TestExecutionStatuses.Incomplete.ToString();
            else if (value.IsFailed())
                return TestExecutionStatuses.Failed.ToString();
            else if (value.IsSkipped())
                return TestExecutionStatuses.Skipped.ToString();
            else
            {
                return TestExecutionStatuses.Undefined.ToString();
            }
        }
    }
}
