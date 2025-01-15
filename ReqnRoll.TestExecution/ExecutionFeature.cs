using System.Collections.Generic;
using System.Linq;

namespace ReqnRoll.TestExecution
{
    public class ExecutionFeature
    {
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FolderPath { get; set; }

        public List<ExecutionScenario> Scenarios { get; set; }

        public ExecutionFeature()
        {
            Scenarios = new List<ExecutionScenario>();
        }

        public bool IsScenarioAdded(string title)
        {
            return Scenarios.Any(m => m.Title == title);
        }

        public ExecutionScenario GetScenario(string title)
        {
            return Scenarios.Find(x => x.Title == title);
        }
    }
}
