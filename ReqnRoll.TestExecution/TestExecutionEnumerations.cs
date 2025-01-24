using System;

namespace ReqnRoll.TestExecution
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
