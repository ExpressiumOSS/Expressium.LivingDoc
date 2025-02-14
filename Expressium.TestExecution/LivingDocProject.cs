using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class LivingDocProject
    {
        public string Title { get; set; }
        public DateTime ExecutionTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<LivingDocFeature> Features { get; set; }

        public LivingDocProject()
        {
            Features = new List<LivingDocFeature>();
        }

        public bool IsFeatureAdded(string title)
        {
            return Features.Any(m => m.Name == title);
        }

        public LivingDocFeature GetFeature(string title)
        {
            return Features.Find(x => x.Name == title);
        }
    }
}
