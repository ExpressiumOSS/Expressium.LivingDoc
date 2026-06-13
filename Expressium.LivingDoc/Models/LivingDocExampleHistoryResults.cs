using System;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocExampleHistoryResults
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }

        public string GetDate()
        {
            return LivingDocUtilities.FormatAsString(Date);
        }
    }
}
