using System.Collections.Generic;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocFolder
    {
        public string Name { get; set; }
        public Dictionary<string, LivingDocFolder> Children { get; set; } = new();

        public LivingDocFolder(string name)
        {
            Name = name;
        }
    }
}
