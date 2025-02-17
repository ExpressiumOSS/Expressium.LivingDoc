using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class LivingDocDataTable
    {
        public List<LivingDocTableRow> Rows { get; set; }

        public LivingDocDataTable()
        {
            Rows = new List<LivingDocTableRow>();
        }
    }
}
