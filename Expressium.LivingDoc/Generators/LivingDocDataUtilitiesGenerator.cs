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

        internal static string GetHealtSymbol(string health)
        {
            if (health == LivingDocHealths.Broken.ToString())
                return "bi bi-cloud-rain-heavy";
            else if (health == LivingDocHealths.Regressed.ToString())
                return "bi bi-cloud-rain";
            else if (health == LivingDocHealths.Flaky.ToString())
                return "bi bi-clouds";
            else if (health == LivingDocHealths.Fixed.ToString())
                return "bi bi-cloud-sun";
            else
                return string.Empty;
        }
    }
}
