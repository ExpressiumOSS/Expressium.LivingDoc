using System.Collections.Generic;

namespace Expressium.LivingDoc
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
