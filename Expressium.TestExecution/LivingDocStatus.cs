using System;

namespace Expressium.TestExecution
{
    public enum LivingDocStatuses
    {
        Unknown,     // Is?
        Passed,      // IsPassed
        Incomplete,  // IsIncomplete
        Skipped,     // IsSkipped
        Pending,     // IsIncomplete
        Undefined,   // IsIncomplete
        Ambiguous,   // IsIncomplete
        Failed       // IsFailed
    }

    internal static class LivingDocStatus
    {
        public static bool IsUnknown(this string value)
        {
            if (value != null && value == LivingDocStatuses.Unknown.ToString())
                return true;

            return false;
        }

        public static bool IsPassed(this string value)
        {
            if (value != null && value == LivingDocStatuses.Passed.ToString())
                return true;

            return false;
        }

        public static bool IsIncomplete(this string value)
        {
            if (value != null && value == LivingDocStatuses.Incomplete.ToString())
                return true;

            if (value.IsPending() || value.IsUndefined() || value.IsAmbiguous())
                return true;

            return false;
        }

        public static bool IsFailed(this string value)
        {
            if (value != null && value == LivingDocStatuses.Failed.ToString())
                return true;

            return false;
        }

        public static bool IsSkipped(this string value)
        {
            if (value != null && value == LivingDocStatuses.Skipped.ToString())
                return true;

            return false;
        }

        public static bool IsPending(this string value)
        {
            if (value != null && value == LivingDocStatuses.Pending.ToString())
                return true;

            return false;
        }

        public static bool IsUndefined(this string value)
        {
            if (value != null && value == LivingDocStatuses.Undefined.ToString())
                return true;

            return false;
        }

        public static bool IsAmbiguous(this string value)
        {
            if (value != null && value == LivingDocStatuses.Ambiguous.ToString())
                return true;

            return false;
        }

        public static string GetStatus(this string value)
        {
            if (value.IsPassed())
                return LivingDocStatuses.Passed.ToString();
            else if (value.IsIncomplete())
                return LivingDocStatuses.Incomplete.ToString();
            else if (value.IsFailed())
                return LivingDocStatuses.Failed.ToString();
            else if (value.IsSkipped())
                return LivingDocStatuses.Skipped.ToString();
            else
            {
                return LivingDocStatuses.Undefined.ToString();
            }
        }
    }
}
