using ReqnRoll.TestExecution;
using System;

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
    }
}
