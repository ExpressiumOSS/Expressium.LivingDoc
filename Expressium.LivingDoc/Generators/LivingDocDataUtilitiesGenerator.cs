using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.Generators
{
    internal static class LivingDocDataUtilitiesGenerator
    {
        internal static string GetStatusSymbol(string status)
        {
            if (status == LivingDocStatuses.Passed.ToString().ToLower())
                return "bi bi-check-circle-fill";
            else if (status == LivingDocStatuses.Failed.ToString().ToLower())
                return "bi bi-x-circle-fill";
            else if (status == LivingDocStatuses.Incomplete.ToString().ToLower())
                return "bi bi-exclamation-circle-fill";
            else if (status == LivingDocStatuses.Skipped.ToString().ToLower())
                return "bi bi-dash-circle-fill";
            else
                return "bi bi-question-circle-fill";
        }
    }
}
