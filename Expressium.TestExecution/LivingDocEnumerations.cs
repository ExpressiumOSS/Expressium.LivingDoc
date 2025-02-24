using System;

namespace Expressium.TestExecution
{
    public enum TestExecutionStatuses
    {
        Unknown,
        Passed,
        Incomplete,
        Skipped,
        Pending,
        Undefined,
        Ambiguous,
        Failed
    }
}
