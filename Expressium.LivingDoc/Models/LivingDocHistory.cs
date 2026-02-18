using System;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocHistory
    {
        public DateTime Date { get; set; }
        public string Url { get; set; }

        public LivingDocHistoryResults Features { get; set; }
        public LivingDocHistoryResults Scenarios { get; set; }
        public LivingDocHistoryResults Steps { get; set; }

        public LivingDocHistory()
        {
            Features = new LivingDocHistoryResults();
            Scenarios = new LivingDocHistoryResults();
            Steps = new LivingDocHistoryResults();
        }

        public string GetDate()
        {
            return Date.ToString("ddd d. MMMM yyyy HH':'mm':'ss \"GMT\"z");
        }

        public int GetNumberOfFeatures()
        {
            return Features.GetNumberOfTotals();
        }

        public int GetNumberOfScenarios()
        {
            return Scenarios.GetNumberOfTotals();
        }

        public int GetNumberOfSteps()
        {
            return Steps.GetNumberOfTotals();
        }
    }
}
