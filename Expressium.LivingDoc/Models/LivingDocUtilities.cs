using System;

namespace Expressium.LivingDoc.Models
{
    public static class LivingDocUtilities
    {
        public static string FormatAsString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalMinutes > 60)
                return $"{timeSpan.Hours}h {timeSpan.Minutes}min";

            if (timeSpan.TotalSeconds > 60)
                return $"{timeSpan.Minutes}min {timeSpan.Seconds}s";

            return $"{timeSpan.Seconds}s {timeSpan.Milliseconds.ToString("D3")}ms";
        }
    }
}
