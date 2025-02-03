using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Expressium.TestExecution
{
    public class TestExecutionStep
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<TestExecutionArgument> Arguments { get; set; }

        public TestExecutionStep()
        {
            Arguments = new List<TestExecutionArgument>();
        }
    }
}
