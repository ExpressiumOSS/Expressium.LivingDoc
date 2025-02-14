using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class LivingDocFeature
    {
        public string Id { get; set; }
        public List<LivingDocTag> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public string Uri { get; set; }

        public List<LivingDocScenario> Scenarios { get; set; }

        public LivingDocFeature()
        {
            Tags= new List<LivingDocTag>();
            Scenarios = new List<LivingDocScenario>();
        }

        public bool IsScenarioAdded(string title)
        {
            return Scenarios.Any(m => m.Name == title);
        }

        public LivingDocScenario GetScenario(string title)
        {
            return Scenarios.Find(x => x.Name == title);
        }
    }
}
