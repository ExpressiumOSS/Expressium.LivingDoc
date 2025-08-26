using Io.Cucumber.Messages.Types;
using System;
using System.Text.RegularExpressions;

namespace Expressium.LivingDoc.Messages
{
    internal static class MessagesUtilities
    {
        internal static DateTime ToDateTime(this Timestamp timestamp)
        {
            var epoch = DateTime.UnixEpoch;

            var dateTime = epoch
                .AddSeconds(timestamp.Seconds)
                .AddTicks(timestamp.Nanos / 100);

            return dateTime.ToLocalTime();
        }

        public static TimeSpan ToTimeSpan(this Timestamp timestampStart, Timestamp timestampEnd)
        {
            var startTime = timestampStart.ToDateTime();
            var endTime = timestampEnd.ToDateTime();

            return endTime - startTime;
        }

        internal static string CapitalizeWords(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(value, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }
    }
}
