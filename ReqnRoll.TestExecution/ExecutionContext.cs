using System.Collections.Generic;
using System.Linq;

namespace ReqnRoll.TestExecution
{
    public class ExecutionContext
    {
        public string Title { get; set; }
        public string ExecutionTime { get; set; }

        public List<ExecutionFeature> Features { get; set; }

        public ExecutionContext()
        {
            Features = new List<ExecutionFeature>();
        }

        public bool IsFeatureAdded(string title)
        {
            return Features.Any(m => m.Title == title);
        }

        public ExecutionFeature GetFeature(string title)
        {
            return Features.Find(x => x.Title == title);
        }
    }
}
