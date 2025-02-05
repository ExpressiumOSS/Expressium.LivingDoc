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

        private bool includeProjectNavigation = false;
        private bool includeTreeview = false;

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
            listOfLines.Add("<body onload=\"loadAnalytics('analytics');\">");
            listOfLines.AddRange(GenerateHeader(executionContext));
            listOfLines.AddRange(GenerateNavigation(executionContext));
            listOfLines.AddRange(GenerateContent(executionContext));
            listOfLines.AddRange(GenerateFooter(executionContext));
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

        private List<string> GenerateHeader(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Header Section -->");
            listOfLines.Add("<header>");
            listOfLines.Add("<span class='project-name'>" + executionContext.Title + "</span><br />");
            listOfLines.Add("<span class='project-date'>generated " + executionContext.GetExecutionTime() + "</span>");
            listOfLines.Add("</header>");

            return listOfLines;
        }

        private List<string> GenerateNavigation(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            if (includeProjectNavigation)
            {
                listOfLines.Add("<!-- Project Navigation Section -->");
                listOfLines.Add("<div class='bg-light'>");
                listOfLines.Add("<nav style='background-color: darkgray; padding: 8px; padding-left: 24px; padding-right: 24px;'>");
                listOfLines.Add("<a href='#' style='color: white;' onclick=\"loadAnalytics('analytics');\">Analytics</a>");
                listOfLines.Add("</nav>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateContent(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

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

            listOfLines.AddRange(GenerateScenarioPreFilters(executionContext));
            listOfLines.AddRange(GenerateScenarioFilter(executionContext));
            listOfLines.AddRange(GenerateScenarioList(executionContext));

            return listOfLines;
        }


        private List<string> GenerateScenarioPreFilters(TestExecutionContext executionContext)
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

        private IEnumerable<string> GenerateScenarioFilter(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features Filter Section -->");
            listOfLines.Add("<div class='section'>");
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
                    listOfLines.Add($"<tr role='feature'>");
                    listOfLines.Add($"<td width='16px;'>&#10010;</td>");
                    listOfLines.Add($"<td colspan='2' style='border-bottom: 1px solid lightgray;'><b>{feature.Title}</b></td>");
                    listOfLines.Add($"<td style='border-bottom: 1px solid lightgray; text-align: right; padding-right: 15px;'>");

                    if (feature.GetNumberOfPassed() > 0)
                        listOfLines.Add($"<span class='status-dot bgcolor-passed'></span>");

                    if (feature.GetNumberOfIncomplete() > 0)
                        listOfLines.Add($"<span class='status-dot bgcolor-incomplete'></span>");

                    if (feature.GetNumberOfFailed() > 0)
                        listOfLines.Add($"<span class='status-dot bgcolor-failed'></span>");

                    if (feature.GetNumberOfSkipped() > 0)
                        listOfLines.Add($"<span class='status-dot bgcolor-skipped'></span>");

                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");

                    foreach (var scenario in feature.Scenarios)
                    {
                        listOfLines.Add($"<tr role='scenario' tags='{feature.Title} {scenario.GetStatus()} {feature.GetTags()} {scenario.GetTags()}' onclick=\"loadScenario('{feature.Id}','{scenario.Id}');\">");
                        listOfLines.Add($"<td width='16px;'></td>");
                        listOfLines.Add($"<td width='16px;'><span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span></td>");
                        listOfLines.Add($"<td style='border-bottom: 1px solid lightgray;'>");
                        listOfLines.Add($"<a href='#'>{scenario.Title}</a></td>");
                        listOfLines.Add("<td style='border-bottom: 1px solid lightgray;'></td>");
                        listOfLines.Add($"</tr>");
                    }
                }

                if (!includeTreeview)
                {
                    foreach (var scenario in feature.Scenarios)
                    {
                        listOfLines.Add($"<tr tags='{feature.Title} {feature.GetTags()} {scenario.GetTags()}' onclick=\"loadScenario('{feature.Id}','{scenario.Id}');\">");
                        listOfLines.Add($"<td>{feature.Title}</td>");
                        listOfLines.Add($"<td><a href='#'>{scenario.Title}</a></td>");
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

        private List<string> GenerateFooter(TestExecutionContext executionContext)
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

                listOfLines.Add("<div class='section'>");
                listOfLines.AddRange(GenerateFeatureTagSection(feature));
                listOfLines.AddRange(GenerateFeatureNameSection(feature));
                listOfLines.AddRange(GenerateFeatureDescriptionSection(feature));
                listOfLines.Add("</div>");

                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateFeatureTagSection(TestExecutionFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Tag Section -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='tag-names'>" + feature.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateFeatureNameSection(TestExecutionFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Name Section -->");
            listOfLines.Add("<div>");
            listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<span class='feature-name'>&nbsp;Feature: {feature.Title}</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateFeatureDescriptionSection(TestExecutionFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Description Section -->");
            listOfLines.Add("<div class='feature-description'>");
            var listOfDescription = feature.Description.Trim().Split("\n");
            foreach (var line in listOfDescription)
                listOfLines.Add("<span>" + line.Trim() + "</span><br />");
            listOfLines.Add("</div>");
            listOfLines.Add("<p></p>");

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

                        listOfLines.Add("<!-- Scenario Outline Section -->");
                        listOfLines.Add("<div class='section'>");

                        listOfLines.AddRange(GenerateScenarioTagSection(scenario));

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
            listOfLines.Add("<span class='tag-names'>" + scenario.GetTags() + "</span>");

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
            listOfLines.Add($"<td><span class='status-dot bgcolor-{example.GetStatus().ToLower()}'></span></td>");
            listOfLines.Add("<td>");
            listOfLines.Add("<span class='scenario-name'>" + scenarioKeyword + " " + scenario.Title + " </span>");
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
                listOfLines.Add($"<td>");
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
                    listOfLines.Add($"<td class='examples' style='padding-left: 64px;'>");

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
                listOfLines.Add($"<td></td>");
                listOfLines.Add($"<td class='examples'><b>Examples:</b></td>");
                listOfLines.Add($"</tr>");

                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td></td>");
                listOfLines.Add($"<td class='examples'>");

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
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='2'>");
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
                listOfLines.Add("<div class='section' style='padding-top: 2px; padding-bottom: 0px;'>");

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

            listOfLines.Add("<!-- Analytics Data Section -->");
            listOfLines.Add($"<div class='data-item' id='analytics'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='project-name'>Analytics</span>");
            listOfLines.Add("</div>");

            listOfLines.AddRange(GenerateScenarioStatusChart(executionContext));
            listOfLines.Add("<p></p>");
            listOfLines.AddRange(GenerateScenarioStatusProperties(executionContext));

            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateScenarioStatusChart(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            var numberOfPassed = executionContext.GetNumberOfPassed();
            var numberOfIncomplete = executionContext.GetNumberOfIncomplete();
            var numberOfFailed = executionContext.GetNumberOfFailed();
            var numberOfSkipped = executionContext.GetNumberOfSkipped();
            var numberOfTests = executionContext.GetNumberOfTests();

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

            {
                listOfLines.Add("<!-- Status Master Chart Section -->");
                listOfLines.Add("<div class='section' style='text-align: center; max-width: 500px; margin: auto;'>");
                listOfLines.Add($"<span class='chart-percentage'>{numberOfPercentPassed.ToString("0")}%</span><br />");
                listOfLines.Add("<span class='chart-status'>Passed</span><br />");

                listOfLines.Add("<p></p>");

                listOfLines.Add($"<div style='width: 100%; height: 0.8em;'>");
                listOfLines.Add($"<div class='bgcolor-passed' style='width: {numberOfPercentPassed}%; height: 0.8em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-incomplete' style='width: {numberOfPercentIncomplete}%; height: 0.8em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-failed' style='width: {numberOfPercentFailed}%; height: 0.8em; float: left'></div>");
                listOfLines.Add($"<div class='bgcolor-skipped' style='width: {numberOfPercentSkipped}%; height: 0.8em; float: left'></div>");
                listOfLines.Add("</div>");

                var message = GetStatusMessage((int)numberOfPercentPassed);
                listOfLines.Add($"<span style='color: gray; font-style: italic; margin: 8px; '>{message}</span>");
                listOfLines.Add("</div>");
            }

            {
                listOfLines.Add("<!-- Status Slave Chart Section -->");
                listOfLines.Add("<div class='section' style='text-align: center; max-width: 600px; margin: auto;'>");
                listOfLines.Add("<table align='center'>");
                listOfLines.Add("<tr>");

                listOfLines.Add("<td class='color-passed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfPassed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{numberOfPercentPassed}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Passed</span><br />");
                listOfLines.Add($"<div class='bgcolor-passed' style='width: 110px; height: 0.4em;'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-incomplete chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfIncomplete}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{numberOfPercentIncomplete}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Incomplete</span><br />");
                listOfLines.Add($"<div class='bgcolor-incomplete' style='width: 110px; height: 0.4em;'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-failed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfFailed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{numberOfPercentFailed}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Failed</span><br />");
                listOfLines.Add($"<div class='bgcolor-failed' style='width: 110px; height: 0.4em;'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-skipped chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfSkipped}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{numberOfPercentSkipped}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Skipped</span><br />");
                listOfLines.Add($"<div class='bgcolor-skipped' style='width: 110px; height: 0.4em;'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-total chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfTests}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>100%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Total</span><br />");
                listOfLines.Add($"<div style='background-color: black; width: 110px; height: 0.4em;'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("</tr>");
                listOfLines.Add("</table>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private List<string> GenerateScenarioStatusProperties(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Status Properties Section -->");
            listOfLines.Add("<div class='section' style='max-width: 600px; margin: auto;'>");
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