using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class LivingDocStep
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public int Line { get; set; }
        public string Status { get; set; }
        public long Duration { get; set; }
        public string Error { get; set; }

        public LivingDocDataTable DataTable { get; set; }

        public LivingDocStep()
        {
            DataTable = new LivingDocDataTable();
        }
    }
}
