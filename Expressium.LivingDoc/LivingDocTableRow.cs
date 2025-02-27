using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc
{
    public class LivingDocTableRow
    {
        public List<LivingDocTableCell> Cells { get; set; }

        public LivingDocTableRow()
        {
            Cells = new List<LivingDocTableCell>();
        }
    }
}
