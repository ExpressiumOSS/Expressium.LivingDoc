using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class TestExecutionDataTable
    {
        public List<TestExecutionTableRow> Rows { get; set; }

        public TestExecutionDataTable()
        {
            Rows = new List<TestExecutionTableRow>();
        }
    }
}
