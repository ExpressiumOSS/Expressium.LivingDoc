using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataAnalyticsGenerator
    {
        private LivingDocProject project;

        internal LivingDocDataAnalyticsGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> GenerateDataAnalyticsFeaturesView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Analytics -->");
            listOfLines.Add($"<div class='data-item' id='analytics-features'>");

            listOfLines.AddRange(GenerateDataAnalyticsTitle());
            listOfLines.AddRange(GenerateDataAnalyticsFeaturesStatusChart());
            listOfLines.AddRange(GenerateDataAnalyticsDuration());

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsScenariosView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Analytics -->");
            listOfLines.Add($"<div class='data-item' id='analytics-scenarios'>");

            listOfLines.AddRange(GenerateDataAnalyticsTitle());
            listOfLines.AddRange(GenerateDataAnalyticsScenariosStatusChart());
            listOfLines.AddRange(GenerateDataAnalyticsDuration());

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsStepsView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Analytics -->");
            listOfLines.Add($"<div class='data-item' id='analytics-steps'>");

            listOfLines.AddRange(GenerateDataAnalyticsTitle());
            listOfLines.AddRange(GenerateDataAnalyticsStepsStatusChart());
            listOfLines.AddRange(GenerateDataAnalyticsDuration());

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsTitle()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='page-name' data-testid='page-title'>Analytics</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<hr>");
            listOfLines.Add("<hr>");

            return listOfLines;

        }

        internal List<string> GenerateDataAnalyticsFeaturesStatusChart()
        {
            var listOfLines = new List<string>();

            var numberOfPassed = project.GetNumberOfPassedFeatures();
            var numberOfIncomplete = project.GetNumberOfIncompleteFeatures();
            var numberOfFailed = project.GetNumberOfFailedFeatures();
            var numberOfSkipped = project.GetNumberOfSkippedFeatures();
            var numberOfTests = project.Features.Count;

            listOfLines.AddRange(GenerateDataAnalyticsStatusChart("Features", numberOfPassed, numberOfIncomplete, numberOfFailed, numberOfSkipped, numberOfTests));

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsScenariosStatusChart()
        {
            var listOfLines = new List<string>();

            var numberOfPassed = project.GetNumberOfPassedScenarios();
            var numberOfIncomplete = project.GetNumberOfIncompleteScenarios();
            var numberOfFailed = project.GetNumberOfFailedScenarios();
            var numberOfSkipped = project.GetNumberOfSkippedScenarios();
            var numberOfTests = project.GetNumberOfScenarios();

            listOfLines.AddRange(GenerateDataAnalyticsStatusChart("Scenarios", numberOfPassed, numberOfIncomplete, numberOfFailed, numberOfSkipped, numberOfTests));

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsStepsStatusChart()
        {
            var listOfLines = new List<string>();

            var numberOfPassed = project.GetNumberOfPassedSteps();
            var numberOfIncomplete = project.GetNumberOfIncompleteSteps();
            var numberOfFailed = project.GetNumberOfFailedSteps();
            var numberOfSkipped = project.GetNumberOfSkippedSteps();
            var numberOfTests = project.GetNumberOfSteps();

            listOfLines.AddRange(GenerateDataAnalyticsStatusChart("Steps", numberOfPassed, numberOfIncomplete, numberOfFailed, numberOfSkipped, numberOfTests));

            return listOfLines;
        }

        internal int CalculatePercentage(int numberOfStatuses, int numberOfTests)
        {
            if (numberOfStatuses == 0)
                return 0;

            var percentage = (int)Math.Round(100.0f / numberOfTests * numberOfStatuses);

            if (numberOfStatuses > 0 && percentage == 0)
                percentage = 1;

            return percentage;
        }

        internal List<string> GenerateDataAnalyticsStatusChart(string title, int numberOfPassed, int numberOfIncomplete, int numberOfFailed, int numberOfSkipped, int numberOfTests)
        {
            var percentageOfPassed = CalculatePercentage(numberOfPassed, numberOfTests);
            var percentageOfIncomplete = CalculatePercentage(numberOfIncomplete, numberOfTests);
            var percentageOfFailed = CalculatePercentage(numberOfFailed, numberOfTests);
            var percentageOfSkipped = CalculatePercentage(numberOfSkipped, numberOfTests);

            var totalPercentage = percentageOfPassed + percentageOfIncomplete + percentageOfFailed + percentageOfSkipped;

            // Adjust the percentages at discrepancy...
            int difference = 100 - totalPercentage;
            if (totalPercentage != 0 && difference != 0)
            {
                var percentages = new List<(int value, Action<int> setter)>
                {
                    (percentageOfPassed, val => percentageOfPassed = val),
                    (percentageOfIncomplete, val => percentageOfIncomplete = val),
                    (percentageOfFailed, val => percentageOfFailed = val),
                    (percentageOfSkipped, val => percentageOfSkipped = val)
                };

                var maxCategory = percentages.OrderByDescending(p => p.value).First();
                maxCategory.setter(maxCategory.value + difference);
            }

            var listOfLines = new List<string>();

            listOfLines.Add($"<div class='section' style='width: fit-content; margin: auto;'>");
            listOfLines.Add($"<span class='chart-name' data-testid='{title.ToLower()}-chart-title'>{title}</span>");
            listOfLines.Add("<div class='section chart-outline'>");

            {
                listOfLines.Add("<!-- Data Analytics Status Chart -->");
                listOfLines.Add($"<div class='section' style='text-align: center; margin: auto;'>");
                listOfLines.Add($"    <svg width='160px' height='168px' viewBox='0 0 42 42'>");
                listOfLines.Add($"        <g transform='rotate(-90, 21, 21)'>");
                listOfLines.Add($"            <circle class='donut-segment-skipped' cx='21' cy='21' r='15.9155'></circle>");
                listOfLines.Add($"            <circle class='donut-segment-passed' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfPassed} {100 - percentageOfPassed}' stroke-dashoffset='0'></circle>");
                listOfLines.Add($"            <circle class='donut-segment-incomplete' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfIncomplete} {100 - percentageOfIncomplete}' stroke-dashoffset='-{percentageOfPassed}'></circle>");
                listOfLines.Add($"            <circle class='donut-segment-failed' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfFailed} {100 - percentageOfFailed}' stroke-dashoffset='-{percentageOfPassed + percentageOfIncomplete}'></circle>");
                listOfLines.Add($"        </g>");
                listOfLines.Add($"        <g class='chart-text'>");
                listOfLines.Add($"            <text x='50%' y='50%' class='chart-number' data-testid='{title.ToLower()}-chart-passed'>{percentageOfPassed}%</text>");
                listOfLines.Add($"            <text x='50%' y='50%' class='chart-label'>Passed</text>");
                listOfLines.Add($"        </g>");
                listOfLines.Add($"    </svg>");
                listOfLines.Add($"</div>");
            }

            {
                listOfLines.Add("<div class='section' style='text-align: center; margin: auto;'>");
                listOfLines.Add("<table align='center'>");
                listOfLines.Add("<tr>");

                listOfLines.Add("<td class='color-passed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfPassed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfPassed}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Passed</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-passed'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-incomplete chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfIncomplete}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfIncomplete}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Incomplete</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-incomplete'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-failed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfFailed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfFailed}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Failed</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-failed'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-skipped chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfSkipped}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfSkipped}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Skipped</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-skipped'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-total chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfTests}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>100%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Total</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-total'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("</tr>");
                listOfLines.Add("</table>");
                listOfLines.Add("</div>");
            }

            listOfLines.Add("</div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsDuration()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<hr>");

            listOfLines.Add("<!-- Data Analytics Duration -->");
            listOfLines.Add("<div style='padding-top: 6px; text-align: center; justify-content: center; align-items: center; display: flex;'>");
            listOfLines.Add($"<span style='font-size: 2.5em;'>&#9201;</span>");
            listOfLines.Add($"<span style='font-size: 1.25em; padding-top: 6px;'>");
            listOfLines.Add($"<span style='color: gray;'>Duration </span>");
            listOfLines.Add($"<span data-testid='project-duration'>{project.GetDuration()}</span>");
            listOfLines.Add("</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }
    }
}
