using ReqnRoll.TestExecution;
using System;

namespace ReqnRoll.TestExecutionReport.Extensions
{
    public static class TestExecutionFeatureExtensions
    {
        public static string GetTags(this TestExecutionFeature feature)
        {
            return feature.Tags.FormatTags();
        }
    }
}
