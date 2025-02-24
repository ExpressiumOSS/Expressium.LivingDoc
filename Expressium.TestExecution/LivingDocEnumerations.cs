using System;

namespace Expressium.TestExecution
{
    public enum TestExecutionStatuses
    {
        OK,
        StepDefinitionPending,
        UndefinedStep,
        BindingError,
        TestError,

        Unknown,
        Passed,
        Skipped,
        Pending,
        Undefined,
        Ambiguous,
        Failed
    }

    public enum ReportStatuses
    {
        Passed,
        Incomplete,
        Failed,
        Skipped,
        Undefined
    }
}
