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

            listOfLines.AddRange(GenerateDataAnalyticsTrends("Features"));
            listOfLines.AddRange(GenerateDataAnalyticsFailures("Features"));

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

            listOfLines.AddRange(GenerateDataAnalyticsTrends("Scenarios"));
            listOfLines.AddRange(GenerateDataAnalyticsFailures("Scenarios"));

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

            listOfLines.AddRange(GenerateDataAnalyticsTrends("Steps"));
            listOfLines.AddRange(GenerateDataAnalyticsFailures("Steps"));

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

        internal List<string> GenerateDataAnalyticsStatusChart(string title, int numberOfPassed, int numberOfIncomplete, int numberOfFailed, int numberOfSkipped, int numberOfTests)
        {
            var percentageOfPassed = CalculatePercentage(numberOfPassed, numberOfTests);
            var percentageOfIncomplete = CalculatePercentage(numberOfIncomplete, numberOfTests);
            var percentageOfFailed = CalculatePercentage(numberOfFailed, numberOfTests);
            var percentageOfSkipped = CalculatePercentage(numberOfSkipped, numberOfTests);

            AdjustPercentagesDiscrepancy(ref percentageOfPassed, ref percentageOfIncomplete, ref percentageOfFailed, ref percentageOfSkipped);

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
            listOfLines.Add($"<span class='bi bi-stopwatch duration-icon'></span>");
            listOfLines.Add($"<span class='duration-text'>");
            listOfLines.Add($"<span class='duration-keyword'>Duration: </span>");
            listOfLines.Add($"<span data-testid='project-duration'>{project.GetDuration()}</span>");
            listOfLines.Add("</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal static int CalculatePercentage(int numberOfStatuses, int numberOfTests)
        {
            if (numberOfStatuses == 0)
                return 0;

            var percentage = (int)Math.Round(100.0f / numberOfTests * numberOfStatuses);

            if (numberOfStatuses > 0 && percentage == 0)
                percentage = 1;

            return percentage;
        }

        internal static void AdjustPercentagesDiscrepancy(ref int percentageOfPassed, ref int percentageOfIncomplete, ref int percentageOfFailed, ref int percentageOfSkipped)
        {
            var totalPercentage = percentageOfPassed + percentageOfIncomplete + percentageOfFailed + percentageOfSkipped;

            int discrepancy = 100 - totalPercentage;
            if (totalPercentage != 0 && discrepancy != 0)
            {
                int[] percentages = { percentageOfPassed, percentageOfIncomplete, percentageOfFailed, percentageOfSkipped };

                int maxIndex = Array.IndexOf(percentages, percentages.Max());
                percentages[maxIndex] += discrepancy;

                percentageOfPassed = percentages[0];
                percentageOfIncomplete = percentages[1];
                percentageOfFailed = percentages[2];
                percentageOfSkipped = percentages[3];
            }
        }

        internal List<string> GenerateDataAnalyticsTrends(string type)
        {
            var listOfLines = new List<string>();

            if (project.Histories.Count < 2)
                return listOfLines;

            int numberOfTotals = 0;

            if (type == "Features")
                numberOfTotals = project.GetMaximumNumberOfHistoryFeatures();
            else if (type == "Scenarios")
                numberOfTotals = project.GetMaximumNumberOfHistoryScenarios();
            else if (type == "Steps")
                numberOfTotals = project.GetMaximumNumberOfHistorySteps();

            var sortedHistories = project.Histories.OrderBy(x => x.Date);

            listOfLines.Add("<hr>");

            listOfLines.Add("<!-- Data Analytics Trend Chart -->");
            listOfLines.Add($"<div class='section chart-history-trends' id='{type.ToLower()}-history-trends'>");
            listOfLines.Add($"<span class='chart-name'>Trends</span>");
            listOfLines.Add("<div class='section'>");

            listOfLines.Add("<table class='chart-history-grid'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th>Id</th>");
            listOfLines.Add("<th>Date</th>");

            listOfLines.Add("<th style='min-width: 300px;'>Status</th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody>");
            listOfLines.Add($"<tr><td></td></tr>");

            int rowIndex = 1;
            foreach (var history in sortedHistories)
            {
                var percentageOfPassed = 0;
                var percentageOfIncomplete = 0;
                var percentageOfFailed = 0;
                var percentageOfSkipped = 0;

                if (type == "Features")
                {
                    percentageOfPassed = CalculatePercentage(history.Features.GetNumberOfPassed(), numberOfTotals);
                    percentageOfIncomplete = CalculatePercentage(history.Features.GetNumberOfIncomplete(), numberOfTotals);
                    percentageOfFailed = CalculatePercentage(history.Features.GetNumberOfFailed(), numberOfTotals);
                    percentageOfSkipped = CalculatePercentage(history.Features.GetNumberOfSkipped(), numberOfTotals);
                }
                else if (type == "Scenarios")
                {
                    percentageOfPassed = CalculatePercentage(history.Scenarios.GetNumberOfPassed(), numberOfTotals);
                    percentageOfIncomplete = CalculatePercentage(history.Scenarios.GetNumberOfIncomplete(), numberOfTotals);
                    percentageOfFailed = CalculatePercentage(history.Scenarios.GetNumberOfFailed(), numberOfTotals);
                    percentageOfSkipped = CalculatePercentage(history.Scenarios.GetNumberOfSkipped(), numberOfTotals);
                }
                else if (type == "Steps")
                {
                    percentageOfPassed = CalculatePercentage(history.Steps.GetNumberOfPassed(), numberOfTotals);
                    percentageOfIncomplete = CalculatePercentage(history.Steps.GetNumberOfIncomplete(), numberOfTotals);
                    percentageOfFailed = CalculatePercentage(history.Steps.GetNumberOfFailed(), numberOfTotals);
                    percentageOfSkipped = CalculatePercentage(history.Steps.GetNumberOfSkipped(), numberOfTotals);
                }

                AdjustPercentagesDiscrepancy(ref percentageOfPassed, ref percentageOfIncomplete, ref percentageOfFailed, ref percentageOfSkipped);

                listOfLines.Add($"<tr title='{history.GetDate()}'>");
                listOfLines.Add($"<td width='24px' style='text-align: center'>{rowIndex}</td>");
                listOfLines.Add($"<td width='45%'>{history.GetDate()}</td>");

                listOfLines.Add("<td>");
                listOfLines.Add("<div style='width: 100%; height: 0.80em;'>");
                listOfLines.Add($"<div class='bgcolor-passed' title='{percentageOfPassed}%' style='width: {percentageOfPassed}%; height: 0.80em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-incomplete' title='{percentageOfIncomplete}%' style='width: {percentageOfIncomplete}%; height: 0.80em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-failed' title='{percentageOfFailed}%' style='width: {percentageOfFailed}%; height: 0.80em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-skipped' title='{percentageOfSkipped}%' style='width: {percentageOfSkipped}%; height: 0.80em; float: left'></div>");
                listOfLines.Add("</div>");
                listOfLines.Add("</td>");
                listOfLines.Add("</tr>");

                rowIndex++;
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");

            listOfLines.Add("</div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsFailures(string type)
        {
            var listOfLines = new List<string>();

            if (project.Histories.Count < 2)
                return listOfLines;

            var historyFailureNames = new List<string>();
            if (type == "Features")
                historyFailureNames = project.GetHistoryFeatureFailures();
            else if (type == "Scenarios")
                historyFailureNames = project.GetHistoryScenarioFailures();
            else if (type == "Steps")
                historyFailureNames = project.GetHistoryStepFailures();

            if (historyFailureNames.Count == 0)
                return listOfLines;

            listOfLines.Add("<hr>");

            listOfLines.Add("<!-- Data Analytics Failures Chart -->");
            listOfLines.Add($"<div class='section chart-history-failures' id='{type.ToLower()}-history-failures'>");
            listOfLines.Add($"<span class='chart-name'>Failures</span>");
            listOfLines.Add("<div class='section'>");

            listOfLines.Add("<table class='chart-history-grid'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th>Name</th>");

            for (int i = 1; i < project.Histories.Count + 1; i++)
                listOfLines.Add($"<th align='center' style='text-align: center'>{i}</th>");

            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody>");

            var orderedHistories = project.Histories.OrderBy(x => x.Date);

            foreach (var failureName in historyFailureNames)
            {
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td>{failureName}</td>");

                foreach (var history in orderedHistories)
                {
                    var listOfPassed = new List<string>();
                    var listOfIncomplete = new List<string>();
                    var listOfFailed = new List<string>();
                    var listOfSkipped = new List<string>();

                    if (type == "Features")
                    {
                        listOfPassed = history.Features.Passed;
                        listOfIncomplete = history.Features.Incomplete;
                        listOfFailed = history.Features.Failed;
                        listOfSkipped = history.Features.Skipped;
                    }
                    else if (type == "Scenarios")
                    {
                        listOfPassed = history.Scenarios.Passed;
                        listOfIncomplete = history.Scenarios.Incomplete;
                        listOfFailed = history.Scenarios.Failed;
                        listOfSkipped = history.Scenarios.Skipped;
                    }
                    else if (type == "Steps")
                    {
                        listOfPassed = history.Steps.Passed;
                        listOfIncomplete = history.Steps.Incomplete;
                        listOfFailed = history.Steps.Failed;
                        listOfSkipped = history.Steps.Skipped;
                    }

                    if (listOfFailed.Contains(failureName))
                        listOfLines.Add($"<td class='history-failed'>Failed</td>");
                    else if (listOfPassed.Contains(failureName))
                        listOfLines.Add($"<td class='history-passed'>Passed</td>");
                    else if (listOfIncomplete.Contains(failureName))
                        listOfLines.Add($"<td class='history-incomplete'>Incomplete</td>");
                    else if (listOfSkipped.Contains(failureName))
                        listOfLines.Add($"<td class='history-skipped'>Skipped</td>");
                    else
                    {
                        listOfLines.Add($"<td class='history-unknown'></td>");
                    }
                }

                listOfLines.Add("</tr>");
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }
    }
}
