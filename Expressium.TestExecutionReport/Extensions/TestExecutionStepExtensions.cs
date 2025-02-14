using Expressium.TestExecution;

namespace Expressium.LivingDoc.Extensions
{
    public static class TestExecutionStepExtensions
    {
        public static bool IsPassed(this LivingDocStep step)
        {
            return step.Status.IsPassed();
        }

        public static bool IsFailed(this LivingDocStep step)
        {
            return step.Status.IsFailed();
        }

        public static bool IsIncomplete(this LivingDocStep step)
        {
            return step.Status.IsIncomplete();
        }

        public static bool IsSkipped(this LivingDocStep step)
        {
            return step.Status.IsSkipped();
        }

        public static string GetStatus(this LivingDocStep step)
        {
            return step.Status.GetStatus();
        }
    }
}
