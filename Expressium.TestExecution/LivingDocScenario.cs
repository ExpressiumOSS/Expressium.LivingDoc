using System;
using System.Collections.Generic;

namespace Expressium.TestExecution
{
    public class LivingDocScenario
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public List<LivingDocTag> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public string Type { get; set; }

        public List<LivingDocExample> Examples { get; set; }

        public LivingDocScenario()
        {
            Index = 0;
            Tags = new List<LivingDocTag>();
            Examples = new List<LivingDocExample>();
        }
    }
}
