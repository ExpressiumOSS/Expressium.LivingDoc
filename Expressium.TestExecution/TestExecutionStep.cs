using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class TestExecutionStep
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public int Line { get; set; }
        public string Status { get; set; }
        public long Duration { get; set; }
        public string Error { get; set; }

        public TestExecutionDataTable DataTable { get; set; }

        public TestExecutionStep()
        {
            DataTable = new TestExecutionDataTable();
        }
    }
}
