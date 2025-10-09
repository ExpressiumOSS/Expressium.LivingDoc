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
        Background,
        Rule,
        Scenario,
        Unknown
    }
}
