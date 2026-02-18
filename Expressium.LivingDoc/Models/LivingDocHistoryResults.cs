using System.Collections.Generic;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocHistoryResults
    {
        public List<string> Passed { get; set; }
        public List<string> Incomplete { get; set; }
        public List<string> Failed { get; set; }
        public List<string> Skipped { get; set; }

        public LivingDocHistoryResults()
        {
            Passed = new List<string>();
            Incomplete = new List<string>();
            Failed = new List<string>();
            Skipped = new List<string>();
        }

        public int GetNumberOfPassed()
        {
            return Passed.Count;
        }

        public int GetNumberOfIncomplete()
        {
            return Incomplete.Count;
        }

        public int GetNumberOfFailed()
        {
            return Failed.Count;
        }

        public int GetNumberOfSkipped()
        {
            return Skipped.Count;
        }

        public int GetNumberOfTotals()
        {
            return Passed.Count + Incomplete.Count + Failed.Count + Skipped.Count;
        }
    }
}
