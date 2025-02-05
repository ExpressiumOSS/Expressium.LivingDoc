using Expressium.TestExecution;
using Expressium.TestExecutionReport.Extensions;
using System.Collections.Generic;

namespace Expressium.TestExecutionReport
{
    internal class TestExecutionReportContentGenerator
    {
        private bool includeTreeview = false;

        internal List<string> GenerateScenarioPreFilters(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features PreFilters Section -->");
            listOfLines.Add("<div class='section prefilters'>");
            listOfLines.Add("<button title='Passed' class='color-passed' onclick='presetScenarios(\"passed\")'>Passed</button>");
            listOfLines.Add("<button title='Incomplete' class='color-incomplete' onclick='presetScenarios(\"incomplete\")'>Incomplete</button>");
            listOfLines.Add("<button title='Failed' class='color-failed' onclick='presetScenarios(\"failed\")'>Failed</button>");
            listOfLines.Add("<button title='Skipped' class='color-skipped' onclick='presetScenarios(\"skipped\")'>Skipped</button>");
            listOfLines.Add("<button title='Clear' class='color-clear' onclick='presetScenarios(\"\")'>Clear</button>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal IEnumerable<string> GenerateScenarioFilter(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features Filter Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<input class='filter' onkeyup='filterScenarios()' id='scenario-filter' type='text' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateScenarioList(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario List Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='scenariolist' class='grid'>");

            if (!includeTreeview)
            {
                listOfLines.Add("<thead>");
                listOfLines.Add("<tr role='header'>");
                listOfLines.Add("<th onClick='sortTableByColumn(0)'>Feature<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th onClick='sortTableByColumn(1)'>Scenario<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th onClick='sortTableByColumn(2)'>Status<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("</tr>");
                listOfLines.Add("</thead>");
            }

            listOfLines.Add("<tbody id='scenario-list'>");

            foreach (var feature in executionContext.Features)
            {
                if (includeTreeview)
                {
                    listOfLines.Add($"<tr role='feature' style='color: dimgray;'>");
                    listOfLines.Add($"<td style='width: 8px;'><b>&#10011;<b></td>");
                    listOfLines.Add($"<td colspan='2' style='border-bottom: 1px solid lightgray;'>");

                    //listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
                    listOfLines.Add($"<span><b>{feature.Title}</b></span>");

                    //if (feature.GetNumberOfPassed() > 0)
                    //    listOfLines.Add($"<span class='status-dot bgcolor-passed'></span>");

                    //if (feature.GetNumberOfIncomplete() > 0)
                    //    listOfLines.Add($"<span class='status-dot bgcolor-incomplete'></span>");

                    //if (feature.GetNumberOfFailed() > 0)
                    //    listOfLines.Add($"<span class='status-dot bgcolor-failed'></span>");

                    //if (feature.GetNumberOfSkipped() > 0)
                    //    listOfLines.Add($"<span class='status-dot bgcolor-skipped'></span>");

                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");

                    foreach (var scenario in feature.Scenarios)
                    {
                        listOfLines.Add($"<tr role='scenario' tags='{feature.Title} {scenario.GetStatus()} {feature.GetTags()} {scenario.GetTags()}' onclick=\"loadScenario('{feature.Id}','{scenario.Id}');\">");
                        listOfLines.Add($"<td style='width: 8px;'></td>");
                        listOfLines.Add($"<td style='width: 24px;'></td>");
                        listOfLines.Add($"<td style='border-bottom: 1px solid lightgray;'>");                        
                        listOfLines.Add($"<span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span>");
                        listOfLines.Add($"<a href='#'>{scenario.Title}</a>");
                        listOfLines.Add($"</td>");
                        listOfLines.Add($"</tr>");
                    }
                }

                if (!includeTreeview)
                {
                    foreach (var scenario in feature.Scenarios)
                    {
                        listOfLines.Add($"<tr tags='{feature.Title} {feature.GetTags()} {scenario.GetTags()}' onclick=\"loadScenario('{feature.Id}','{scenario.Id}');\">");
                        listOfLines.Add($"<td>{feature.Title}</td>");
                        listOfLines.Add($"<td><a href='#'><span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span><span>{scenario.Title}</span></a></td>");
                        listOfLines.Add($"<td>{scenario.GetStatus()}</td>");
                        listOfLines.Add($"</tr>");
                    }
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            return listOfLines;
        }
    }
}
