using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class TestExecutionFeature
    {
        public string Id { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public string Uri { get; set; }

        public List<TestExecutionScenario> Scenarios { get; set; }

        public TestExecutionFeature()
        {
            Scenarios = new List<TestExecutionScenario>();
        }

        public bool IsScenarioAdded(string title)
        {
            return Scenarios.Any(m => m.Name == title);
        }

        public TestExecutionScenario GetScenario(string title)
        {
            return Scenarios.Find(x => x.Name == title);
        }
    }
}
