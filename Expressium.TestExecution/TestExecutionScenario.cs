using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class TestExecutionScenario
    {
        public string Id { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<TestExecutionExample> Examples { get; set; }

        public TestExecutionScenario()
        {
            Examples = new List<TestExecutionExample>();
        }
    }
}
