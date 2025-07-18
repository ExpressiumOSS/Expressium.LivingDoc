using System.Collections.Generic;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocDataTable
    {
        public List<LivingDocDataTableRow> Rows { get; set; }

        public LivingDocDataTable()
        {
            Rows = new List<LivingDocDataTableRow>();
        }
    }
}
