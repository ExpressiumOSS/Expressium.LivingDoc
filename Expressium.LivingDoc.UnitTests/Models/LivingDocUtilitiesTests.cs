using Expressium.LivingDoc.Models;
using System;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocUtilitiesTests
    {
        [Test]
        public void LivingDocUtilities_DateTime_FormatAsString()
        {
            var date = new DateTime(2024, 3, 15, 9, 5, 3, DateTimeKind.Utc);

            var result = date.FormatAsString();

            Assert.That(result, Does.Match(@"^Fri 15 Mar 2024 at 09:05:03 GMT[+-]\d+$"));
        }

        [TestCase(0, 0, "0s 000ms")]
        [TestCase(1, 0, "1s 000ms")]
        [TestCase(59, 0, "59s 000ms")]
        [TestCase(0, 587, "0s 587ms")]
        [TestCase(7, 587, "7s 587ms")]
        [TestCase(59, 999, "59s 999ms")]
        public void LivingDocUtilities_TimeSpan_FormatAsString_Below_Minutes(int seconds, int milliseconds, string expected)
        {
            var timeSpan = new TimeSpan(0, 0, 0, seconds, milliseconds);

            Assert.That(timeSpan.FormatAsString(), Is.EqualTo(expected));
        }

        [TestCase(1, 1, "1min 1s")]
        [TestCase(1, 0, "1min 0s")]
        [TestCase(43, 7, "43min 7s")]
        [TestCase(59, 59, "59min 59s")]
        public void LivingDocUtilities_TimeSpan_FormatAsString_Below_Hours(int minutes, int seconds, string expected)
        {
            var timeSpan = new TimeSpan(0, 0, minutes, seconds, 0);

            Assert.That(timeSpan.FormatAsString(), Is.EqualTo(expected));
        }

        [TestCase(1, 0, "1h 0min")]
        [TestCase(1, 1, "1h 1min")]
        [TestCase(2, 43, "2h 43min")]
        [TestCase(23, 59, "23h 59min")]
        public void LivingDocUtilities_TimeSpan_FormatAsString_Below_Days(int hours, int minutes, string expected)
        {
            var timeSpan = new TimeSpan(0, hours, minutes, 0, 0);

            Assert.That(timeSpan.FormatAsString(), Is.EqualTo(expected));
        }

        [Test]
        public void LivingDocUtilities_TimeSpan_FormatAsString_DaysOverflowIntoHours()
        {
            // 1 day + 2 hours = 26 total hours; Hours property returns 2, TotalMinutes >= 60
            var timeSpan = new TimeSpan(1, 2, 43, 7, 587);

            Assert.That(timeSpan.FormatAsString(), Is.EqualTo("2h 43min"));
        }
    }
}
