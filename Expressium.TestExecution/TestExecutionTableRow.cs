using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class TestExecutionTableRow
    {
        public List<TestExecutionTableCell> Cells { get; set; }

        public TestExecutionTableRow()
        {
            Cells = new List<TestExecutionTableCell>();
        }
    }
}
