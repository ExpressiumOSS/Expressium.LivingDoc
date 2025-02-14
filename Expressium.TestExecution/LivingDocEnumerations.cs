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
        Skipped
    }
}
