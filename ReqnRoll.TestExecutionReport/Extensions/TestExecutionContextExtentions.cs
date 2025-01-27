using ReqnRoll.TestExecution;
using System.Linq;

namespace ReqnRoll.TestExecutionReport.Extensions
{
    public static class TestExecutionContextExtentions
    {
        public static string GetExecutionTime(this TestExecutionContext context)
        {
            return context.ExecutionTime.ToString("ddd dd. MMM yyyy HH':'mm':'ss \"GMT\"z");
        }

        public static void OrderFeaturesByTags(this TestExecutionContext context)
        {
            context.Features = context.Features.OrderBy(x => x.Tags).ToList();
        }
    }
}
