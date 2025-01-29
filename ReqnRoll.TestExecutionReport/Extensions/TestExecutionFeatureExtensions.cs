using ReqnRoll.TestExecution;

namespace ReqnRoll.TestExecutionReport.Extensions
{
    public static class TestExecutionFeatureExtensions
    {
        public static bool IsTagged(this TestExecutionFeature feature)
        {
            return !string.IsNullOrEmpty(feature.Tags);
        }

        public static string GetTags(this TestExecutionFeature feature)
        {
            return feature.Tags.FormatTags();
        }

        public static string GetStatus(this TestExecutionFeature feature)
        {
            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsFailed())
                    return ReportStatuses.Failed.ToString();
            }

            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsInconclusive())
                    return ReportStatuses.Inconclusive.ToString();
            }

            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsSkipped())
                    return ReportStatuses.Skipped.ToString();
            }

            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.IsPassed())
                    return ReportStatuses.Passed.ToString();
            }

            return ReportStatuses.Undefined.ToString();
        }
    }
}
