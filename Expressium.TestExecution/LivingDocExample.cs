using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class LivingDocExample
    {
        public string Status { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<LivingDocStep> Steps { get; set; }
        public LivingDocTableRow TableHeader { get; set; }
        public List<LivingDocTableRow> TableBody { get; set; }
        public List<string> Attachments { get; set; }

        public LivingDocExample()
        {
            Steps = new List<LivingDocStep>();
            TableHeader = new LivingDocTableRow();
            TableBody = new List<LivingDocTableRow>();
            Attachments = new List<string>();
        }
    }
}
