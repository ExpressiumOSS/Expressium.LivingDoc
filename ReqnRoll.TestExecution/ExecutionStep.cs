using System.Collections.Generic;

namespace ReqnRoll.TestExecution
{
    public class ExecutionStep
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
        public string Duration { get; set; }
    }
}
