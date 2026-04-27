using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataAnalyticsGenerator
    {
        private LivingDocProject project;

        private enum AnalyticsType { Features, Scenarios, Steps }

        internal LivingDocDataAnalyticsGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> GenerateDataAnalyticsFeaturesView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Analytics -->");
            listOfLines.Add("<div id='analytics-features'>");

            listOfLines.AddRange(GenerateDataAnalyticsTitle());
            listOfLines.AddRange(GenerateDataAnalyticsStatusChart(AnalyticsType.Features.ToString()));
            listOfLines.AddRange(GenerateDataAnalyticsDuration());
            listOfLines.AddRange(GenerateDataAnalyticsTrends(AnalyticsType.Features.ToString()));

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsScenariosView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Analytics -->");
            listOfLines.Add("<div id='analytics-scenarios'>");

            listOfLines.AddRange(GenerateDataAnalyticsTitle());
            listOfLines.AddRange(GenerateDataAnalyticsStatusChart(AnalyticsType.Scenarios.ToString()));
            listOfLines.AddRange(GenerateDataAnalyticsDuration());
            listOfLines.AddRange(GenerateDataAnalyticsTrends(AnalyticsType.Scenarios.ToString()));

            if (project.ExperimentFlagHealth)
            {
#if DEBUG
                listOfLines.AddRange(GenerateDataAnalyticsHealths());
                listOfLines.AddRange(GenerateDataAnalyticsFailures());
#endif
            }

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsStepsView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Analytics -->");
            listOfLines.Add("<div id='analytics-steps'>");

            listOfLines.AddRange(GenerateDataAnalyticsTitle());
            listOfLines.AddRange(GenerateDataAnalyticsStatusChart(AnalyticsType.Steps.ToString()));
            listOfLines.AddRange(GenerateDataAnalyticsDuration());
            listOfLines.AddRange(GenerateDataAnalyticsTrends(AnalyticsType.Steps.ToString()));

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

        internal List<string> GenerateDataAnalyticsStatusChart(string type)
        {
            var listOfLines = new List<string>();

            var numberOfPassed = 0;
            var numberOfIncomplete = 0;
            var numberOfFailed = 0;
            var numberOfSkipped = 0;
            var numberOfTests = 0;

            if (type == AnalyticsType.Features.ToString())
            {
                numberOfPassed = project.GetNumberOfPassedFeatures();
                numberOfIncomplete = project.GetNumberOfIncompleteFeatures();
                numberOfFailed = project.GetNumberOfFailedFeatures();
                numberOfSkipped = project.GetNumberOfSkippedFeatures();
                numberOfTests = project.Features.Count;
            }
            else if (type == AnalyticsType.Scenarios.ToString())
            {
                numberOfPassed = project.GetNumberOfPassedScenarios();
                numberOfIncomplete = project.GetNumberOfIncompleteScenarios();
                numberOfFailed = project.GetNumberOfFailedScenarios();
                numberOfSkipped = project.GetNumberOfSkippedScenarios();
                numberOfTests = project.GetNumberOfScenarios();
            }
            else if (type == AnalyticsType.Steps.ToString())
            {
                numberOfPassed = project.GetNumberOfPassedSteps();
                numberOfIncomplete = project.GetNumberOfIncompleteSteps();
                numberOfFailed = project.GetNumberOfFailedSteps();
                numberOfSkipped = project.GetNumberOfSkippedSteps();
                numberOfTests = project.GetNumberOfSteps();
            }

            listOfLines.AddRange(GenerateDataAnalyticsStatusChart(type, numberOfPassed, numberOfIncomplete, numberOfFailed, numberOfSkipped, numberOfTests));

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

            listOfLines.Add("<div class='section' style='width: fit-content; margin: auto;'>");
            listOfLines.Add($"<span class='chart-name' data-testid='{title.ToLower()}-chart-title'>{title}</span>");
            listOfLines.Add("<div class='section chart-outline'>");

            {
                listOfLines.Add("<!-- Data Analytics Status Chart -->");
                listOfLines.Add("<div class='section' style='text-align: center; margin: auto;'>");
                listOfLines.Add("    <svg class='chart-status' viewBox='0 0 42 42'>");
                listOfLines.Add("        <g transform='rotate(-90, 21, 21)'>");

                var borderOffset = ".5";
                if (new[] { numberOfPassed, numberOfSkipped, numberOfIncomplete, numberOfFailed }.Any(x => x == numberOfTests))
                    borderOffset = "";

                if (numberOfPassed > 0)
                    listOfLines.Add($"            <circle class='donut-segment-passed' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfPassed - 1}{borderOffset} {100 - percentageOfPassed}{borderOffset}' stroke-dashoffset='0'></circle>");

                if (numberOfIncomplete > 0)
                    listOfLines.Add($"            <circle class='donut-segment-incomplete' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfIncomplete - 1}{borderOffset} {100 - percentageOfIncomplete}{borderOffset}' stroke-dashoffset='-{percentageOfPassed}'></circle>");

                if (numberOfFailed > 0)
                    listOfLines.Add($"            <circle class='donut-segment-failed' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfFailed - 1}{borderOffset} {100 - percentageOfFailed}{borderOffset}' stroke-dashoffset='-{percentageOfPassed + percentageOfIncomplete}'></circle>");

                if (numberOfSkipped > 0)
                    listOfLines.Add($"            <circle class='donut-segment-skipped' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfSkipped - 1}{borderOffset} {100 - percentageOfSkipped}{borderOffset}' stroke-dashoffset='-{percentageOfPassed + percentageOfIncomplete + percentageOfFailed}'></circle>");

                listOfLines.Add("        </g>");
                listOfLines.Add("        <g class='chart-text'>");
                listOfLines.Add($"            <text x='50%' y='50%' class='chart-number' data-testid='{title.ToLower()}-chart-passed'>{percentageOfPassed}%</text>");
                listOfLines.Add("            <text x='50%' y='50%' class='chart-label'>Passed</text>");
                listOfLines.Add("        </g>");
                listOfLines.Add("    </svg>");
                listOfLines.Add("</div>");
            }

            {
                listOfLines.Add("<div class='section' style='text-align: center; margin: auto;'>");
                listOfLines.Add("<table align='center'>");
                listOfLines.Add("<tr>");

                listOfLines.Add("<td class='color-passed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfPassed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfPassed}%</span><br />");
                listOfLines.Add("<span class='chart-count-status'>Passed</span><br />");
                listOfLines.Add("<div class='chart-count-bar bgcolor-passed'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-incomplete chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfIncomplete}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfIncomplete}%</span><br />");
                listOfLines.Add("<span class='chart-count-status'>Incomplete</span><br />");
                listOfLines.Add("<div class='chart-count-bar bgcolor-incomplete'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-failed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfFailed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfFailed}%</span><br />");
                listOfLines.Add("<span class='chart-count-status'>Failed</span><br />");
                listOfLines.Add("<div class='chart-count-bar bgcolor-failed'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-skipped chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfSkipped}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfSkipped}%</span><br />");
                listOfLines.Add("<span class='chart-count-status'>Skipped</span><br />");
                listOfLines.Add("<div class='chart-count-bar bgcolor-skipped'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-total chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfTests}</span><br />");
                listOfLines.Add("<span class='chart-count-percentage'>100%</span><br />");
                listOfLines.Add("<span class='chart-count-status'>Total</span><br />");
                listOfLines.Add("<div class='chart-count-bar bgcolor-total'></div>");
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
            listOfLines.Add("<span class='bi bi-stopwatch duration-symbol'></span>");
            listOfLines.Add("<span class='duration-text'>");
            listOfLines.Add("<span class='duration-keyword'>Duration: </span>");
            listOfLines.Add($"<span data-testid='project-duration'>{project.GetDuration()}</span>");
            listOfLines.Add("</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsTrends(string type)
        {
            var listOfLines = new List<string>();

            int numberOfTotals = 0;
            if (type == AnalyticsType.Features.ToString())
                numberOfTotals = project.History.GetMaximumNumberOfHistoryFeatures();
            else if (type == AnalyticsType.Scenarios.ToString())
                numberOfTotals = project.History.GetMaximumNumberOfHistoryScenarios();
            else if (type == AnalyticsType.Steps.ToString())
                numberOfTotals = project.History.GetMaximumNumberOfHistorySteps();

            if (numberOfTotals < 2)
                return listOfLines;

            List<LivingDocProjectHistoryResults> historyResults = null;
            if (type == AnalyticsType.Features.ToString())
                historyResults = project.History.Features;
            else if (type == AnalyticsType.Scenarios.ToString())
                historyResults = project.History.Scenarios;
            else if (type == AnalyticsType.Steps.ToString())
                historyResults = project.History.Steps;

            listOfLines.Add("<hr>");

            listOfLines.Add("<!-- Data Analytics Trend Chart -->");
            listOfLines.Add("<div class='section analytics-trends'>");
            listOfLines.Add("<span class='chart-name'>Trends</span>");
            listOfLines.Add("<div class='section'>");

            listOfLines.Add("<table class='analytics-list'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th>Id</th>");
            listOfLines.Add("<th>Date</th>");

            listOfLines.Add("<th style='min-width: 300px;'>Status</th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody>");
            listOfLines.Add("<tr><td></td></tr>");

            int rowIndex = 1;
            foreach (var history in historyResults)
            {
                var percentageOfPassed = 0;
                var percentageOfIncomplete = 0;
                var percentageOfFailed = 0;
                var percentageOfSkipped = 0;

                if (type == AnalyticsType.Features.ToString())
                {
                    percentageOfPassed = CalculatePercentage(history.Passed, numberOfTotals);
                    percentageOfIncomplete = CalculatePercentage(history.Incomplete, numberOfTotals);
                    percentageOfFailed = CalculatePercentage(history.Failed, numberOfTotals);
                    percentageOfSkipped = CalculatePercentage(history.Skipped, numberOfTotals);
                }
                else if (type == AnalyticsType.Scenarios.ToString())
                {
                    percentageOfPassed = CalculatePercentage(history.Passed, numberOfTotals);
                    percentageOfIncomplete = CalculatePercentage(history.Incomplete, numberOfTotals);
                    percentageOfFailed = CalculatePercentage(history.Failed, numberOfTotals);
                    percentageOfSkipped = CalculatePercentage(history.Skipped, numberOfTotals);
                }
                else if (type == AnalyticsType.Steps.ToString())
                {
                    percentageOfPassed = CalculatePercentage(history.Passed, numberOfTotals);
                    percentageOfIncomplete = CalculatePercentage(history.Incomplete, numberOfTotals);
                    percentageOfFailed = CalculatePercentage(history.Failed, numberOfTotals);
                    percentageOfSkipped = CalculatePercentage(history.Skipped, numberOfTotals);
                }

                AdjustPercentagesDiscrepancy(ref percentageOfPassed, ref percentageOfIncomplete, ref percentageOfFailed, ref percentageOfSkipped);

                listOfLines.Add($"<tr title='{history.GetDate()}'>");
                listOfLines.Add($"<td width='24px' style='text-align: center'>{rowIndex}</td>");
                listOfLines.Add($"<td width='45%'>{history.GetDate()}</td>");

                listOfLines.Add("<td>");
                listOfLines.Add("<div style='width: 100%;'>");
                listOfLines.Add($"<div class='bgcolor-passed' title='{percentageOfPassed}%' style='width: {percentageOfPassed}%; height: 0.75em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-incomplete' title='{percentageOfIncomplete}%' style='width: {percentageOfIncomplete}%; height: 0.75em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-failed' title='{percentageOfFailed}%' style='width: {percentageOfFailed}%; height: 0.75em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-skipped' title='{percentageOfSkipped}%' style='width: {percentageOfSkipped}%; height: 0.75em; float: left'></div>");
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

        internal List<string> GenerateDataAnalyticsHealths()
        {
            var listOfLines = new List<string>();

            var hasHealth = project.Features.SelectMany(f => f.Scenarios).Any(s => s.Health != null);
            if (!hasHealth)
                return listOfLines;

            listOfLines.Add("<hr>");

            listOfLines.Add("<!-- Data Analytics Healths Chart -->");
            listOfLines.Add("<div class='section analytics-healths'>");
            listOfLines.Add("<span class='chart-name'>Healths</span>");
            listOfLines.Add("<div class='section'>");

            listOfLines.Add("<table class='analytics-list'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th>Name</th>");
            listOfLines.Add("<th align='center' style='text-align: center'>Health</th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody>");

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    if (scenario.HasHealth())
                    {
                        listOfLines.Add($"<tr class='grid-border' data-role='scenario' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                        listOfLines.Add($"<td><a href='#'>{scenario.Name}</a></td>");

                        if (scenario.IsFixed())
                            listOfLines.Add($"<td class='history-passed'>{LivingDocHealths.Fixed.ToString()}</td>");
                        else if (scenario.IsBroken())
                            listOfLines.Add($"<td class='history-failed'>{LivingDocHealths.Broken.ToString()}</td>");
                        else if (scenario.IsRegressed())
                            listOfLines.Add($"<td class='history-failed'>{LivingDocHealths.Regressed.ToString()}</td>");
                        else if (scenario.IsFlaky())
                            listOfLines.Add($"<td class='history-failed'>{LivingDocHealths.Flaky.ToString()}</td>");

                        listOfLines.Add("</tr>");
                    }
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataAnalyticsFailures()
        {
            var listOfLines = new List<string>();

            var hasHealth = project.Features.SelectMany(f => f.Scenarios).Any(s => s.Health != null);
            if (!hasHealth)
                return listOfLines;

            listOfLines.Add("<hr>");

            listOfLines.Add("<!-- Data Analytics Failures Chart -->");
            listOfLines.Add("<div class='section analytics-failures'>");
            listOfLines.Add("<span class='chart-name'>Failures</span>");
            listOfLines.Add("<div class='section'>");

            listOfLines.Add("<table class='analytics-list'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th>Name</th>");

            var listOfDates = project.History.Scenarios.Select(x => x.GetDate()).Distinct();

            for (int i = 1; i < listOfDates.Count() + 1; i++)
                listOfLines.Add($"<th align='center' style='text-align: center'>{i}</th>");
            listOfLines.Add($"<th align='center' style='text-align: center'>Health</th>");

            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody>");

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    if (scenario.HasHealth())
                    {
                        foreach (var example in scenario.Examples)
                        {
                            listOfLines.Add("<tr>");
                            listOfLines.Add($"<td>{scenario.Name}</td>");

                            foreach (var date in listOfDates)
                            {
                                var hasHistory = false;
                                foreach (var history in example.History)
                                {
                                    if (history.GetDate() == date)
                                    {
                                        if (history.Status == LivingDocStatuses.Failed.ToString())
                                            listOfLines.Add($"<td class='history-failed'>Failed</td>");
                                        else if (history.Status == LivingDocStatuses.Passed.ToString())
                                            listOfLines.Add($"<td class='history-passed'>Passed</td>");
                                        else if (history.Status == LivingDocStatuses.Incomplete.ToString())
                                            listOfLines.Add($"<td class='history-incomplete'>Incomplete</td>");
                                        else if (history.Status == LivingDocStatuses.Skipped.ToString())
                                            listOfLines.Add($"<td class='history-skipped'>Skipped</td>");
                                        else
                                        {
                                            listOfLines.Add($"<td class='history-unknown'></td>");
                                        }

                                        hasHistory = true;
                                    }
                                }

                                if (!hasHistory)
                                    listOfLines.Add($"<td class='history-unknown'></td>");
                            }

                            listOfLines.Add($"<td class='history-skipped'>{scenario.Health}</td>");
                            listOfLines.Add("</tr>");
                        }
                    }
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");
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
    }
}
