using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc
{
    public class LivingDocTableRow
    {
        public List<string> Cells { get; set; }

        public LivingDocTableRow()
        {
            Cells = new List<string>();
        }
    }
}
