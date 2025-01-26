using System;
using System.Collections.Generic;

namespace ReqnRoll.TestExecution
{
    public class TestExecutionExample
    {
        public string Status { get; set; }
        public TimeSpan Duration { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }

        public List<TestExecutionArgument> Arguments { get; set; }
        public List<TestExecutionStep> Steps { get; set; }
        public List<string> Attachments { get; set; }

        public TestExecutionExample()
        {
            Arguments = new List<TestExecutionArgument>();
            Steps = new List<TestExecutionStep>();
            Attachments = new List<string>();
        }
    }
}
