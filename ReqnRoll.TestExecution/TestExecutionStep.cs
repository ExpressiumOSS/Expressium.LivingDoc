using System;

namespace ReqnRoll.TestExecution
{
    public class TestExecutionStep
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public TimeSpan Duration { get; set; }

        public bool IsPassed()
        {
            if (Status == TestExecutionStatuses.OK.ToString())
                return true;
            return false;
        }

        public bool IsFailed()
        {
            if (Status == TestExecutionStatuses.TestError.ToString())
                return true;
            return false;
        }

        public bool IsSkipped()
        {
            if (Status == TestExecutionStatuses.Skipped.ToString())
                return true;
            return false;
        }

        public bool IsInconclusive()
        {
            if (Status == TestExecutionStatuses.StepDefinitionPending.ToString() ||
                Status == TestExecutionStatuses.UndefinedStep.ToString() ||
                Status == TestExecutionStatuses.BindingError.ToString())
                return true;
            return false;
        }

        public string GetStatus()
        {
            if (Status == TestExecutionStatuses.OK.ToString())
                return "Passed";
            else if (Status == TestExecutionStatuses.Skipped.ToString())
                return "Skipped";
            else if (Status == TestExecutionStatuses.TestError.ToString())
                return "Failed";
            else
            {
                return "Inconclusive";
            }
        }
    }
}
