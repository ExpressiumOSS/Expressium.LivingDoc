using Expressium.TestExecution;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionStepExtensions
    {
        public static bool IsPassed(this TestExecutionStep step)
        {
            return step.Status.IsPassed();
        }

        public static bool IsFailed(this TestExecutionStep step)
        {
            return step.Status.IsFailed();
        }

        public static bool IsSkipped(this TestExecutionStep step)
        {
            return step.Status.IsSkipped();
        }

        public static bool IsIncomplete(this TestExecutionStep step)
        {
            return step.Status.IsIncomplete();
        }

        public static string GetStatus(this TestExecutionStep step)
        {
            return step.Status.GetStatus();
        }
    }
}
