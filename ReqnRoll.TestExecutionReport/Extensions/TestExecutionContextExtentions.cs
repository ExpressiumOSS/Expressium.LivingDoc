using ReqnRoll.TestExecution;

namespace ReqnRoll.TestExecutionReport.Extensions
{
    public static class TestExecutionContextExtentions
    {
        public static string GetExecutionTime(this TestExecutionContext context)
        {
            return context.ExecutionTime.ToString("ddd dd. MMM yyyy HH':'mm':'ss \"GMT\"z");
        }
    }
}
