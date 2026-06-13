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

        public string GetDate()
        {
            return Date.FormatAsString();
        }

        public int GetNumberOfTests()
        {
            return Passed + Incomplete + Failed + Skipped;
        }
    }
}
