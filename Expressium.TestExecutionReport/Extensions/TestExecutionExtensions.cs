using Expressium.TestExecution;
using System;

namespace Expressium.TestExecutionReport.Extensions
{
    public enum ReportStatuses
    {
        Passed,
        Incomplete,
        Failed,
        Skipped,
        Undefined
    }

    internal static class TestExecutionExtensions
    {
        public static bool IsPassed(this string value)
        {
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
            if (value == TestExecutionStatuses.TestError.ToString())
                return true;
            return false;
        }

        public static bool IsSkipped(this string value)
        {
            if (value == TestExecutionStatuses.Skipped.ToString())
                return true;
            return false;
        }

        public static bool IsStepPending(this string value)
        {
            if (value == TestExecutionStatuses.StepDefinitionPending.ToString())
                return true;
            return false;
        }

        public static bool IsStepUndefined(this string value)
        {
            if (value == TestExecutionStatuses.UndefinedStep.ToString())
                return true;
            return false;
        }

        public static bool IsStepBindingError(this string value)
        {
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

        //public static string FormatTags(this string value)
        //{
        //    if (string.IsNullOrWhiteSpace(value))
        //        return string.Empty;

        //    var tags = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        //    for (int i = 0; i < tags.Length; i++)
        //        tags[i] = "@" + tags[i];

        //    return string.Join(' ', tags);
        //}
    }
}
