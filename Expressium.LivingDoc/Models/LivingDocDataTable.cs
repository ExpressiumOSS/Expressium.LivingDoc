using System.Collections.Generic;

namespace Expressium.LivingDoc.Models
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
