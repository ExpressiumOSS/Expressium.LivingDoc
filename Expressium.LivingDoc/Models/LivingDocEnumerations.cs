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
        Broken,      // Was failing across several test runs
        Regressed,   // Was passing but is now failing in latest test run
        Flaky,       // Was alternating unpredictably in latest test runs
        New,         // Was not present but is now in latest test run
        Fixed,       // Was failing but is now passing in latest test run
        Invalid,     // Was incomplete in latest test run
    }
}
