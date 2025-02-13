using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class TestExecutionExample
    {
        public string Status { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<TestExecutionStep> Steps { get; set; }
        public TestExecutionTableRow TableHeader { get; set; }
        public List<TestExecutionTableRow> TableBody { get; set; }
        public List<string> Attachments { get; set; }

        public TestExecutionExample()
        {
            Steps = new List<TestExecutionStep>();
            TableHeader = new TestExecutionTableRow();
            TableBody = new List<TestExecutionTableRow>();
            Attachments = new List<string>();
        }
    }
}
