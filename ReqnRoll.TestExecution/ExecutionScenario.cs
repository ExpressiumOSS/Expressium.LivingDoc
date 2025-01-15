using System.Collections.Generic;

namespace ReqnRoll.TestExecution
{
    public class ExecutionScenario
    {
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }

        public List<ExecutionStep> Steps { get; set; }

        public ExecutionScenario()
        {
            Steps = new List<ExecutionStep>();
        }
    }
}
