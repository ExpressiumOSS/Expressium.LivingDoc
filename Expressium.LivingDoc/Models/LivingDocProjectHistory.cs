using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocProjectHistory
    {
        public List<LivingDocProjectHistoryResults> Features { get; set; }
        public List<LivingDocProjectHistoryResults> Scenarios { get; set; }
        public List<LivingDocProjectHistoryResults> Steps { get; set; }

        public LivingDocProjectHistory()
        {
            Features = new List<LivingDocProjectHistoryResults>();
            Scenarios = new List<LivingDocProjectHistoryResults>();
            Steps = new List<LivingDocProjectHistoryResults>();
        }

        public int GetMaximumNumberOfHistoryFeatures()
        {
            return Features
                .Select(h => h.GetNumberOfTests())
                .DefaultIfEmpty(0).Max();
        }

        public int GetMaximumNumberOfHistoryScenarios()
        {
            return Scenarios
                .Select(h => h.GetNumberOfTests())
                .DefaultIfEmpty(0).Max();
        }

        public int GetMaximumNumberOfHistorySteps()
        {
            return Steps
                .Select(h => h.GetNumberOfTests())
                .DefaultIfEmpty(0).Max();
        }
    }
}
