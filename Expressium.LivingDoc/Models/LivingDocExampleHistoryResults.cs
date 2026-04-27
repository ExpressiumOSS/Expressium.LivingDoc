using System;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocExampleHistoryResults
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }

        public LivingDocExampleHistoryResults()
        {
            Date = DateTime.MinValue;
        }

        public string GetDate()
        {
            return LivingDocUtilities.FormatAsString(Date);
        }
    }
}
