using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class LivingDataTable
    {
        public List<LivingDocTableRow> Rows { get; set; }

        public LivingDataTable()
        {
            Rows = new List<LivingDocTableRow>();
        }
    }
}
