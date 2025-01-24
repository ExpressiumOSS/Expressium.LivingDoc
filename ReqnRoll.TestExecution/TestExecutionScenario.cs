using System;
using System.Collections.Generic;

namespace ReqnRoll.TestExecution
{
    public class TestExecutionScenario
    {
        public string Id { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public TimeSpan Duration { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }

        public List<TestExecutionArgument> Arguments { get; set; }
        public List<TestExecutionStep> Steps { get; set; }
        public List<string> Attachments { get; set; }

        public TestExecutionScenario()
        {
            Arguments = new List<TestExecutionArgument>();
            Steps = new List<TestExecutionStep>();
            Attachments = new List<string>();
        }

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
