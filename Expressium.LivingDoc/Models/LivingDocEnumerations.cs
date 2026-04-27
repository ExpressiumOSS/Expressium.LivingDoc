using System;

namespace Expressium.LivingDoc.Models
{
    public enum LivingDocStatuses
    {
        Failed,
        Incomplete,
        Pending,
        Undefined,
        Ambiguous,
        Passed,
        Skipped,
        Unknown
    }

    public enum LivingDocStepTypes
    {
        Hook,
        Background,
        Rule,
        Scenario,
        Unknown
    }

    public enum LivingDocHealths
    {
        Broken,      // Persistently failing across several test runs
        Regressed,   // Was passing but is now failing in latest test run
        Flaky,       // Was alternating unpredictably in latest test runs
        Fixed,       // Was failing but is now passing in latest test run
    }
}
