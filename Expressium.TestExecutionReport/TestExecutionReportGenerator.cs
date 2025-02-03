using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Expressium.TestExecution;
using Expressium.TestExecutionReport.Extensions;
using Expressium.TestExecutionReport.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.TestExecutionReport
{
    internal class TestExecutionReportGenerator
    {
        private string filePath;
        private string outputPath;

        private bool includeHeader = true;
        private bool includeProjectInformation = false;
        private bool includePageHeader = false;
        private bool includeOnLoadAnalytics = true;
        private bool includeStatusChart = false;
        private bool includeStatusChartAnalytics = false;

        internal TestExecutionReportGenerator(string filePath, string outputPath)
        {
            this.filePath = filePath;
            this.outputPath = outputPath;
        }

        internal void Execute()
        {
            Console.WriteLine("");
            Console.WriteLine("Generating Test Execution Report...");
            Console.WriteLine("FilePath: " + filePath);
            Console.WriteLine("OutputPath: " + outputPath);
            Console.WriteLine("");

            Console.WriteLine("Parsing Test Execution JSON File...");
            var executionContext = TestExecutionUtilities.DeserializeAsJson<TestExecutionContext>(filePath);

            Console.WriteLine("Creating Test Execution Output Directories...");
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);
            Directory.CreateDirectory(Path.Combine(outputPath, "Attachments"));

            // Sort list of Features by Tags...
            executionContext.OrderByTags();

            // Assign Unique Identifier to all Scenarios...
            foreach (var feature in executionContext.Features)
            {
                feature.Id = Guid.NewGuid().ToString();

                foreach (var scenario in feature.Scenarios)
                    scenario.Id = Guid.NewGuid().ToString();
            }

            // Copy Attachments to Output Directory...
            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        foreach (var attachment in example.Attachments)
                        {
                            if (File.Exists(attachment))
                                File.Copy(attachment, Path.Combine(outputPath, "Attachments", Path.GetFileName(attachment)), true);
                        }
                    }
                }
            }

            Console.WriteLine("Generating Test Execution HTML Report...");
            GenerateTestExecutionReport(executionContext);

            Console.WriteLine("Generating Test Execution Report Completed");
            Console.WriteLine("");
        }

        private void GenerateTestExecutionReport(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!DOCTYPE html>");
            listOfLines.Add("<html>");
            listOfLines.AddRange(GenerateHead());

            if (includeOnLoadAnalytics)
                listOfLines.Add("<body onload=\"loadAnalytics('analytics');\">");
            else
                listOfLines.Add("<body>");

            listOfLines.AddRange(GenerateHeader());
            listOfLines.AddRange(GenerateHeaderProjectInformation(executionContext));
            listOfLines.AddRange(GenerateContent(executionContext));
            listOfLines.AddRange(GenerateFooter());
            listOfLines.AddRange(GenerateData(executionContext));
            listOfLines.Add("</body>");
            listOfLines.Add("</html>");

            var htmlFilePath = Path.Combine(outputPath, "LivingDoc.html");
            SaveListOfLinesToFile(htmlFilePath, listOfLines);
        }

        private static void SaveListOfLinesToFile(string filePath, List<string> listOfLines)
        {
            var content = string.Join(Environment.NewLine, listOfLines);

            var htmlParser = new HtmlParser();
            var htmlDocument = htmlParser.ParseDocument(content);

            using (var streamWriter = new StringWriter())
            {
                htmlDocument.ToHtml(streamWriter, new PrettyMarkupFormatter
                {
                    Indentation = "\t",
                    NewLine = "\n"
                });

                File.WriteAllText(filePath, streamWriter.ToString());
            }
        }

        private List<string> GenerateHead()
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<head>");

            listOfLines.AddRange(GenerateHeads());
            listOfLines.AddRange(GenerateStyles());
            listOfLines.AddRange(GenerateScripts());

            listOfLines.Add($"</head>");

            return listOfLines;
        }

        private List<string> GenerateHeads()
        {
            return Resources.Heads.Split(Environment.NewLine).ToList();
        }

        private List<string> GenerateStyles()
        {
            return Resources.Styles.Split(Environment.NewLine).ToList();
        }

        private List<string> GenerateScripts()
        {
            return Resources.Scripts.Split(Environment.NewLine).ToList();
        }

        private List<string> GenerateHeader()
        {
            List<string> listOfLines = new List<string>();

            if (includeHeader)
            {
                listOfLines.Add("<!-- Header Section -->");
                listOfLines.Add("<header>");
                listOfLines.Add("Expressium LivingDoc");
                listOfLines.Add("</header>");
            }

            return listOfLines;
        }

        private List<string> GenerateContent(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            //if (includeOnLoadAnalytics)
            //{
            //    listOfLines.Add("<div class='bg-light'>");
            //    listOfLines.Add("<nav style='background-color: darkgray; padding: 8px;'>");
            //    listOfLines.Add("<a href='#' style='color: white;' onclick=\"loadAnalytics('analytics');\">Analytics</a>");
            //    listOfLines.Add("</nav>");
            //    listOfLines.Add("</div>");
            //}

            listOfLines.Add("<!-- Content Wrapper Section -->");
            listOfLines.Add("<div id='content-wrapper'>");

            listOfLines.Add("<!-- Left Content Section -->");
            listOfLines.Add("<div id='left-section' class='bg-light p-3'>");
            listOfLines.AddRange(GenerateContentOverview(executionContext));
            listOfLines.Add("</div>");

            listOfLines.Add("<!-- Splitter Content Section -->");
            listOfLines.Add("<div id='splitter'></div>");

            listOfLines.Add("<!-- Right Content Section -->");
            listOfLines.Add("<div id='right-section' class='bg-light p-3'>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            listOfLines.Add("<!-- Content Splitter Script -->");
            listOfLines.AddRange(Resources.SplitterScript.Split(Environment.NewLine).ToList());

            return listOfLines;
        }

        private List<string> GenerateContentOverview(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            if (includePageHeader)
                listOfLines.Add("<div class='page-name'>Living Documentation</div>");

            listOfLines.AddRange(GenerateProjectInformation(executionContext));
            listOfLines.AddRange(GenerateScenarioStatusChart(executionContext));
            listOfLines.AddRange(GenerateScenarioListSeperator(executionContext));
            listOfLines.AddRange(GenerateScenarioPreFilters(executionContext));
            listOfLines.AddRange(GenerateScenarioFilter(executionContext));
            listOfLines.AddRange(GenerateScenarioList(executionContext));

            return listOfLines;
        }

        private List<string> GenerateHeaderProjectInformation(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            if (includeProjectInformation)
            {
                listOfLines.Add("<!-- Project Information Section -->");
                listOfLines.Add("<div class='bg-light'>");
                listOfLines.Add("<div style='padding: 8px; padding-left: 24px; padding-right: 24px; background-color: #C7D9E8;'>");
                listOfLines.Add("<span class='project-name'>" + executionContext.Title + "</span><br />");
                listOfLines.Add("<span class='project-date'>generated " + executionContext.GetExecutionTime() + "</span>");
                listOfLines.Add("</div>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateProjectInformation(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            if (!includeProjectInformation)
            {
                listOfLines.Add("<!-- Project Information Section -->");
                listOfLines.Add("<div class='section' style='padding-bottom: 8px;'>");
                listOfLines.Add("<span class='project-name'>" + executionContext.Title + "</span><br />");
                listOfLines.Add("<span class='project-date'>generated " + executionContext.GetExecutionTime() + "</span>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateScenarioStatusChart(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            if (includeStatusChart)
            {
                var numberOfPassed = executionContext.GetNumberOfPassed();
                var numberOfIncomplete = executionContext.GetNumberOfIncomplete();
                var numberOfFailed = executionContext.GetNumberOfFailed();
                var numberOfSkipped = executionContext.GetNumberOfSkipped();
                var numberOfTests = executionContext.GetNumberOfTests();

                listOfLines.Add("<!-- Status Chart Section -->");
                listOfLines.Add("<div id='status-chart'>");

                listOfLines.Add("<div class='container' style='text-align: center; max-width: 500px; padding-bottom: 16px;'>");

                var numberOfPercentPassed = (int)Math.Round(100.0f / numberOfTests * numberOfPassed);
                var numberOfPercentIncomplete = (int)Math.Round(100.0f / numberOfTests * numberOfIncomplete);
                var numberOfPercentFailed = (int)Math.Round(100.0f / numberOfTests * numberOfFailed);
                var numberOfPercentSkipped = (int)Math.Round(100.0f / numberOfTests * numberOfSkipped);

                var sumOfPercent = numberOfPercentPassed + numberOfPercentIncomplete + numberOfPercentFailed + numberOfPercentSkipped;
                if (sumOfPercent > 100)
                {
                    if (numberOfPercentPassed > 1)
                        numberOfPercentPassed -= 1;
                    else if (numberOfPercentIncomplete > 1)
                        numberOfPercentIncomplete -= 1;
                    else if (numberOfPercentFailed > 1)
                        numberOfPercentFailed -= 1;
                    else if (numberOfPercentSkipped > 1)
                        numberOfPercentSkipped -= 1;
                    else
                    {
                    }
                }

                listOfLines.Add($"<span class='chart-percentage'>{numberOfPercentPassed.ToString("0")}%</span><br />");
                listOfLines.Add("<span class='chart-status'>Passed</span>");

                listOfLines.Add("<p></p>");
                //listOfLines.Add($"<p style='color: gray; font-style: italic; margin: 8px; '>{numberOfPassed} Passed, {numberOfIncomplete} Incomplete, {numberOfFailed} Failed, {numberOfSkipped} Skipped Scenarios</p>");

                listOfLines.Add($"<div style='width: 100%; height: 0.8em;'>");
                listOfLines.Add($"<div class='bgcolor-passed' style='width: {numberOfPercentPassed}%; height: 0.8em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-incomplete' style='width: {numberOfPercentIncomplete}%; height: 0.8em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-failed' style='width: {numberOfPercentFailed}%; height: 0.8em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-skipped' style='width: {numberOfPercentSkipped}%; height: 0.8em; float: left'></div>");
                listOfLines.Add("</div>");

                var message = GetStatusMessage((int)numberOfPercentPassed);
                listOfLines.Add($"<span style='color: gray; font-style: italic; margin: 8px; '>{message}</span>");

                listOfLines.Add("</div>");

                if (includeStatusChartAnalytics)
                {
                    listOfLines.Add("<!-- Status Analytics Section -->");
                    listOfLines.Add("<div class='container' style='text-align: center; max-width: 600px; padding-bottom: 16px;'>");

                    listOfLines.Add("<table width='100%'>");
                    listOfLines.Add("<tr>");

                    listOfLines.Add("<td class='color-passed' align='center'>");
                    listOfLines.Add($"<span class='chart-count'>");
                    listOfLines.Add($"<span class='chart-count-number'>{numberOfPassed}</span><br />");
                    listOfLines.Add($"<span>{numberOfPercentPassed}%<br /></span>");
                    listOfLines.Add($"<span>Passed</span><br />");
                    listOfLines.Add($"<div class='bgcolor-passed' style='width: 110px; height: 0.4em;'></div>");
                    listOfLines.Add($"</span>");
                    listOfLines.Add("</td>");

                    listOfLines.Add("<td class='color-incomplete' align='center'>");
                    listOfLines.Add($"<span class='chart-count'>");
                    listOfLines.Add($"<span class='chart-count-number'>{numberOfIncomplete}</span><br />");
                    listOfLines.Add($"<span>{numberOfPercentIncomplete}%<br /></span>");
                    listOfLines.Add($"<span>Incomplete</span><br />");
                    listOfLines.Add($"<div class='bgcolor-incomplete' style='width: 110px; height: 0.4em;'></div>");
                    listOfLines.Add($"</span>");
                    listOfLines.Add("</td>");

                    listOfLines.Add("<td class='color-failed' align='center'>");
                    listOfLines.Add($"<span class='chart-count'>");
                    listOfLines.Add($"<span class='chart-count-number'>{numberOfFailed}</span><br />");
                    listOfLines.Add($"<span>{numberOfPercentFailed}%<br /></span>");
                    listOfLines.Add($"<span>Failed</span><br />");
                    listOfLines.Add($"<div class='bgcolor-failed' style='width: 110px; height: 0.4em;'></div>");
                    listOfLines.Add($"</span>");
                    listOfLines.Add("</td>");

                    listOfLines.Add("<td class='color-skipped' align='center'>");
                    listOfLines.Add($"<span class='chart-count'>");
                    listOfLines.Add($"<span class='chart-count-number'>{numberOfSkipped}</span><br />");
                    listOfLines.Add($"<span>{numberOfPercentSkipped}%<br /></span>");
                    listOfLines.Add($"<span>Skipped</span><br />");
                    listOfLines.Add($"<div class='bgcolor-skipped' style='width: 110px; height: 0.4em;'></div>");
                    listOfLines.Add($"</span>");
                    listOfLines.Add("</td>");

                    listOfLines.Add("<td class='color-total' align='center'>");
                    listOfLines.Add($"<span class='chart-count'>");
                    listOfLines.Add($"<span class='chart-count-number'>{numberOfTests}</span><br />");
                    listOfLines.Add($"<span>100%</span><br />");
                    listOfLines.Add($"<span>Total</span><br />");
                    listOfLines.Add($"<div style='background-color: black; width: 110px; height: 0.4em;'></div>");
                    listOfLines.Add($"</span>");
                    listOfLines.Add("</td>");

                    listOfLines.Add("</tr>");
                    listOfLines.Add("</table>");

                    listOfLines.Add("</div>");
                }

                if (includeStatusChart)
                {
                    listOfLines.Add("<!-- Status Properties Section -->");
                    listOfLines.Add("<br />");
                    listOfLines.Add("<div class='container'>");
                    listOfLines.Add("<span class='feature-name'>Properties</span><br />");
                    listOfLines.Add("<table width='100%' align='center' border='1'>");
                    listOfLines.Add("<thead>");
                    listOfLines.Add("<tr><th>Name</th><th>Value</th></tr>");
                    listOfLines.Add("</thead>");
                    listOfLines.Add("<tbody>");
                    listOfLines.Add($"<tr><td><b>Project: </b></td><td>{executionContext.Title}</td></tr>");
                    listOfLines.Add($"<tr><td><b>Execution Time: </b></td><td>{executionContext.GetExecutionTime()}</td></tr>");
                    listOfLines.Add($"<tr><td><b>Duration: </b></td><td>{executionContext.GetDuration()}</td></tr>");
                    listOfLines.Add($"<tr><td><b>Environment: </b></td><td>{executionContext.Environment}</td></tr>");
                    listOfLines.Add("</tbody>");
                    listOfLines.Add("</table>");
                    listOfLines.Add("</div>");
                    listOfLines.Add("<br />");
                }

                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateScenarioListSeperator(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<div style='height: 8px;'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateScenarioPreFilters(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features PreFilters Section -->");

            listOfLines.Add("<div class='section' style='text-align: right; padding-bottom: 6px;'>");
            listOfLines.Add("<button title='Passed' class='color-passed' onclick='presetScenarios(\"passed\")'>Passed</button>");
            listOfLines.Add("<button title='Incomplete' class='color-incomplete' onclick='presetScenarios(\"incomplete\")'>Incomplete</button>");
            listOfLines.Add("<button title='Failed' class='color-failed' onclick='presetScenarios(\"failed\")'>Failed</button>");
            listOfLines.Add("<button title='Skipped' class='color-skipped' onclick='presetScenarios(\"skipped\")'>Skipped</button>");
            listOfLines.Add("<button title='Clear' class='color-clear' onclick='presetScenarios(\"\")'>Clear</button>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private IEnumerable<string> GenerateScenarioFilter(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features Filter Section -->");
            listOfLines.Add("<div class='section' style='padding-bottom: 6px;'>");
            listOfLines.Add("<input class='filter' onkeyup='filterScenarios()' id='scenario-filter' type='text' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateScenarioList(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario List Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='scenariolist' class='grid'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th onClick='sortTableByColumn(0)'>Feature<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Scenario<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(2)'>Status<span class='sort-column'>&udarr;</span></th>");
            //listOfLines.Add("<th></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody id='scenario-list'>");

            foreach (var feature in executionContext.Features)
            {
                //listOfLines.Add($"<tr>");
                //listOfLines.Add($"<td colspan='3'><b>{feature.Title}<b></td>");
                //listOfLines.Add($"</tr>");

                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add($"<tr tags='{feature.GetTags()} {scenario.GetTags()}' onclick=\"loadScenario('{feature.Id}','{scenario.Id}');\">");
                    listOfLines.Add($"<td>{feature.Title}</td>");
                    listOfLines.Add($"<td><a href='#'>{scenario.Title}</a></td>");
                    listOfLines.Add($"<td>{scenario.GetStatus()}</td>");
                    //listOfLines.Add($"<td><span class='status-dot bgcolor-{scenario.GetStatus()}'></span></td>");
                    listOfLines.Add($"</tr>");
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateFooter()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Footer Section -->");
            listOfLines.Add("<footer>");
            listOfLines.Add("©2025 Expressium All Rights Reserved");
            listOfLines.Add("</footer>");

            return listOfLines;
        }

        private List<string> GenerateData(TestExecutionContext executionContext)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateFeatureDataSections(executionContext));
            listOfLines.AddRange(GenerateScenarioDataSections(executionContext));
            listOfLines.AddRange(GenerateAnalyticsSection(executionContext));

            return listOfLines;
        }

        private List<string> GenerateFeatureDataSections(TestExecutionContext executionContext)
        {
            var listOfLines = new List<string>();

            foreach (var feature in executionContext.Features)
            {
                listOfLines.Add("<!-- Feature Data Section -->");
                listOfLines.Add($"<div class='data-item' id='{feature.Id}'>");
                listOfLines.AddRange(GenerateFeatureTagSection(feature));
                listOfLines.AddRange(GenerateFeatureNameSection(feature));
                listOfLines.AddRange(GenerateFeatureDescriptionSection(feature));
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateFeatureTagSection(TestExecutionFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Tag Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='tag-names'>" + feature.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateFeatureNameSection(TestExecutionFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Name Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<span class='feature-name'>&nbsp;Feature: {feature.Title}</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateFeatureDescriptionSection(TestExecutionFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Description Section -->");
            listOfLines.Add("<div class='section feature-description' style='padding-bottom: 24px;'>");
            var listOfDescription = feature.Description.Trim().Split("\n");
            foreach (var line in listOfDescription)
                listOfLines.Add("<span>" + line.Trim() + "</span><br />");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateScenarioDataSections(TestExecutionContext executionContext)
        {
            var listOfLines = new List<string>();

            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<!-- Scenario Data Section -->");
                    listOfLines.Add($"<div class='data-item' id='{scenario.Id}'>");

                    bool exampleSplitter = false;
                    foreach (var example in scenario.Examples)
                    {
                        if (exampleSplitter)
                            listOfLines.Add("<hr>");
                        exampleSplitter = true;

                        listOfLines.AddRange(GenerateScenarioTagSection(scenario));

                        listOfLines.Add("<!-- Scenario Outline Section -->");
                        listOfLines.Add("<div class='section' style='padding-bottom: 16px;'>");
                        listOfLines.Add("<table>");
                        listOfLines.Add("<tbody>");

                        listOfLines.AddRange(GenerateScenarioTitleSection(scenario, example));
                        listOfLines.AddRange(GenerateScenarioStepSection(example));
                        listOfLines.AddRange(GenerateScenarioExamplesSection(example));
                        listOfLines.AddRange(GenerateScenarioMessageSection(example));

                        listOfLines.Add("</tbody>");
                        listOfLines.Add("</table>");
                        listOfLines.Add("</div>");

                        listOfLines.AddRange(GenerateScenarioAttachments(example));
                    }
                    listOfLines.Add("</div>");
                }
            }

            return listOfLines;
        }

        private List<string> GenerateScenarioTagSection(TestExecutionScenario scenario)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Tag Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='tag-names'>" + scenario.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateScenarioTitleSection(TestExecutionScenario scenario, TestExecutionExample example)
        {
            var scenarioKeyword = "Scenario:";
            if (example.Arguments.Count > 0)
                scenarioKeyword = "Scenario Outline:";

            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Title Section -->");
            listOfLines.Add("<tr>");
            listOfLines.Add("<td colspan='3'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{example.GetStatus().ToLower()}'></span>");
            listOfLines.Add("<span class='scenario-name'>&nbsp;" + scenarioKeyword + " " + scenario.Title + " </span>");
            listOfLines.Add("<span class='duration'>&nbsp;" + example.GetDuration() + "</span>");
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            return listOfLines;
        }

        private List<string> GenerateScenarioStepSection(TestExecutionExample example)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Steps Section -->");

            foreach (var step in example.Steps)
            {
                var status = step.GetStatus().ToLower();

                var stepMarker = "";
                if (step.IsPassed())
                    stepMarker = "&check;";
                else
                    stepMarker = "&cross;";

                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td></td>");
                listOfLines.Add($"<td colspan='2'>");
                listOfLines.Add($"<span class='step-indent color-{status}'><b>{stepMarker}</b></span>");
                listOfLines.Add($"<span class='step-keyword'> " + step.Type + "</span> ");
                listOfLines.Add($"<span>" + step.Text + "</span>");
                listOfLines.Add($"</td>");
                listOfLines.Add($"</tr>");

                if (step.Arguments.Count > 0)
                {
                    listOfLines.Add("<!-- Scenario Steps Table Section -->");
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td></td>");
                    listOfLines.Add($"<td class='step-examples' colspan='2'>");

                    listOfLines.Add("<table>");
                    listOfLines.Add("<tbody>");

                    listOfLines.Add($"<tr>");
                    foreach (var argument in step.Arguments)
                        listOfLines.Add($"<td><i>| " + argument.Name + "</i></td>");
                    listOfLines.Add($"<td>|</td>");
                    listOfLines.Add($"</tr>");

                    listOfLines.Add($"<tr>");
                    foreach (var argument in step.Arguments)
                        listOfLines.Add($"<td>| " + argument.Value + "</td>");
                    listOfLines.Add($"<td>|</td>");
                    listOfLines.Add($"</tr>");

                    listOfLines.Add("</tbody>");
                    listOfLines.Add("</table>");

                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }
            }

            return listOfLines;
        }

        private List<string> GenerateScenarioExamplesSection(TestExecutionExample example)
        {
            var listOfLines = new List<string>();

            if (example.Arguments.Count > 0)
            {
                listOfLines.Add("<!-- Scenario Examples Section -->");
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='3' class='examples'><b>Examples:</b></td>");
                listOfLines.Add($"</tr>");

                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='3' class='examples'>");

                listOfLines.Add("<table>");
                listOfLines.Add("<tbody>");

                listOfLines.Add($"<tr>");
                foreach (var argument in example.Arguments)
                    listOfLines.Add($"<td><i>| " + argument.Name + "</i></td>");
                listOfLines.Add($"<td>|</td>");
                listOfLines.Add($"</tr>");

                listOfLines.Add($"<tr>");
                foreach (var argument in example.Arguments)
                    listOfLines.Add($"<td>| " + argument.Value + "</td>");
                listOfLines.Add($"<td>|</td>");
                listOfLines.Add($"</tr>");

                listOfLines.Add("</tbody>");
                listOfLines.Add("</table>");

                listOfLines.Add($"</td>");
                listOfLines.Add($"</tr>");
            }

            return listOfLines;
        }

        private List<string> GenerateScenarioMessageSection(TestExecutionExample example)
        {
            var listOfLines = new List<string>();

            var status = example.GetStatus().ToLower();

            string message = null;
            if (example.Error != null)
                message = example.Error;
            else if (example.IsStepPending())
                message = "Pending Step Definition";
            else if (example.IsStepUndefined())
                message = "Undefined Step Definition";
            else if (example.IsStepBindingError())
                message = "Binding Error Step Definition";
            else
            {
            }

            if (message != null)
            {
                listOfLines.Add("<!-- Scenario Message Section -->");
                listOfLines.Add($"<tr><td></td></tr>");
                listOfLines.Add($"<tr><td></td></tr>");
                //listOfLines.Add($"<tr><td colspan='3'><span class='scenario-name'>Message:</span></td></tr>");
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='3'>");
                listOfLines.Add($"<div class='step-{status}'>{message}</div>");
                listOfLines.Add($"</td>");
                listOfLines.Add($"</tr>");
            }

            return listOfLines;
        }

        private List<string> GenerateScenarioAttachments(TestExecutionExample example)
        {
            var listOfLines = new List<string>();

            if (example.Attachments.Count > 0)
            {
                listOfLines.Add("<!-- Scenario Attachments Section -->");
                listOfLines.Add("<div class='section attachments'>");
                listOfLines.Add("<span class='scenario-name'>Attachments:</span>");
                listOfLines.Add("<ul>");

                foreach (var attachment in example.Attachments)
                {
                    var filePath = Path.GetFileName(attachment);
                    listOfLines.Add($"<li><a target='_blank' href='./Attachments/{filePath}'>{filePath}</a></li>");
                }

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateAnalyticsSection(TestExecutionContext executionContext)
        {
            var listOfLines = new List<string>();

            var _includeStatusChart = includeStatusChart;
            var _includeStatusChartAnalytics = includeStatusChartAnalytics;

            includeStatusChart = true;
            includeStatusChartAnalytics = true;

            listOfLines.Add("<!-- Analytics Data Section -->");
            listOfLines.Add($"<div class='data-item' id='analytics'>");

            if (includePageHeader)
                listOfLines.Add("<div class='page-name'>Analytics</div>");

            if (!includeProjectInformation)
            {
                listOfLines.Add("<div class='section' style='padding-bottom: 8px;'>");
                listOfLines.Add("<span class='project-name'>Analytics</span>");
                listOfLines.Add("<br />");
                listOfLines.Add("</div>");
            }

            listOfLines.AddRange(GenerateScenarioStatusChart(executionContext));

            if (1 == 2)
            {
                listOfLines.Add("<br />");
                listOfLines.Add("<div class='container'>");
                listOfLines.Add("<table width='100%'>");
                listOfLines.Add("<thead>");
                listOfLines.Add("<tr>");
                listOfLines.Add("<th></th>");
                listOfLines.Add("<th colspan='5' align='center'>Scenario</th>");
                listOfLines.Add("</tr>");
                listOfLines.Add("<tr>");
                listOfLines.Add("<th>Feature</th>");
                listOfLines.Add("<th>Passed</th>");
                listOfLines.Add("<th>Incomplete</th>");
                listOfLines.Add("<th>Failed</th>");
                listOfLines.Add("<th>Skipped</th>");
                listOfLines.Add("<th>Tests</th>");
                listOfLines.Add("</tr>");
                listOfLines.Add("</thead>");
                listOfLines.Add("<tbody>");
                foreach (var feature in executionContext.Features)
                {
                    listOfLines.Add("<tr>");
                    listOfLines.Add($"<td>{feature.Title}</td>");
                    listOfLines.Add($"<td align='center'>{feature.GetNumberOfPassed()}</td>");
                    listOfLines.Add($"<td align='center'>{feature.GetNumberOfIncomplete()}</td>");
                    listOfLines.Add($"<td align='center'>{feature.GetNumberOfFailed()}</td>");
                    listOfLines.Add($"<td align='center'>{feature.GetNumberOfSkipped()}</td>");
                    listOfLines.Add($"<td align='center'>{feature.GetNumberOfTests()}</td>");
                    listOfLines.Add("</tr>");
                }
                listOfLines.Add("</tbody>");
                listOfLines.Add("</table>");
                listOfLines.Add("</div>");
            }

            listOfLines.Add("</div>");

            includeStatusChart = _includeStatusChart;
            includeStatusChartAnalytics = _includeStatusChartAnalytics;

            return listOfLines;
        }

        private string GetStatusMessage(int status)
        {
            if (status == 100)
                return "The system is fully covered and successfully validated!";
            else if (status >= 90)
                return "The system is extensively covered with minor potential risks!";
            else if (status >= 75)
                return "The system is well covered with significant potential risks!";
            else if (status >= 50)
                return "The system is moderately covered with significant potential risks!";
            else if (status >= 25)
                return "The system is partially covered with many potential risks!";
            else if (status >= 10)
                return "The system is minimally covered with many undetected risks!";
            else if (status < 10)
                return "The system is not covered with a uncertainties in reliability!";
            else
            {
            }

            return null;
        }
    }
}

// https://www.toptal.com/designers/htmlarrows/symbols