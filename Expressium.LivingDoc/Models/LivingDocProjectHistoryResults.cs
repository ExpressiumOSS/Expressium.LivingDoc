using System;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocProjectHistoryResults
    {
        public DateTime Date { get; set; }

        public int Passed { get; set; }
        public int Incomplete { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }

        public LivingDocProjectHistoryResults()
        {
            Date = DateTime.MinValue;

            Passed = 0;
            Incomplete = 0;
            Failed = 0;
            Skipped = 0;
        }

        public string GetDate()
        {
            return LivingDocUtilities.FormatAsString(Date);
        }

        public int GetNumberOfTests()
        {
            return Passed + Incomplete + Failed + Skipped;
        }
    }
}
