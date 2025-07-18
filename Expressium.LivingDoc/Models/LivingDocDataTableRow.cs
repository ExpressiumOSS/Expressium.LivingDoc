using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocDataTableRow
    {
        public List<string> Cells { get; set; }

        public LivingDocDataTableRow()
        {
            Cells = new List<string>();
        }
    }
}
