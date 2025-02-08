using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.TestExecution
{
    public class TestExecutionProject
    {
        public string Title { get; set; }
        public DateTime ExecutionTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<TestExecutionFeature> Features { get; set; }

        public TestExecutionProject()
        {
            Features = new List<TestExecutionFeature>();
        }

        public bool IsFeatureAdded(string title)
        {
            return Features.Any(m => m.Title == title);
        }

        public TestExecutionFeature GetFeature(string title)
        {
            return Features.Find(x => x.Title == title);
        }
    }
}
