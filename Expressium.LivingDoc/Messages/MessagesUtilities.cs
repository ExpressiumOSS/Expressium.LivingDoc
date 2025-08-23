using Io.Cucumber.Messages.Types;
using System;

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
    }
}
