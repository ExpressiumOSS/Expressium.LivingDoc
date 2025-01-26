using ReqnRoll.TestExecution;

namespace ReqnRoll.TestExecutionReport.Extensions
{
    public static class TestExecutionScenarioExtensions
    {
        public static string GetTags(this TestExecutionScenario scenario)
        {
            return scenario.Tags.FormatTags();
        }

        public static bool IsPassed(this TestExecutionScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Passed.ToString())
                return true;

            return false;
        }

        public static bool IsFailed(this TestExecutionScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Failed.ToString())
                return true;

            return false;
        }

        public static bool IsSkipped(this TestExecutionScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Skipped.ToString())
                return true;

            return false;
        }

        public static bool IsInconclusive(this TestExecutionScenario scenario)
        {
            if (GetStatus(scenario) == ReportStatuses.Inconclusive.ToString())
                return true;

            return false;
        }

        public static string GetStatus(this TestExecutionScenario scenario)
        {
            foreach (var example in scenario.Examples)
            {
                if (example.IsFailed())
                    return ReportStatuses.Failed.ToString();
            }

            foreach (var example in scenario.Examples)
            {
                if (example.IsInconclusive())
                    return ReportStatuses.Inconclusive.ToString();
            }

            foreach (var example in scenario.Examples)
            {
                if (example.IsSkipped())
                    return ReportStatuses.Skipped.ToString();
            }

            foreach (var example in scenario.Examples)
            {
                if (example.IsPassed())
                    return ReportStatuses.Passed.ToString();
            }

            return ReportStatuses.Undefined.ToString();
        }
    }
}
