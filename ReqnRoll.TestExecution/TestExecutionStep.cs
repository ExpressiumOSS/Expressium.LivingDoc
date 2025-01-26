using System;

namespace ReqnRoll.TestExecution
{
    public class TestExecutionStep
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
