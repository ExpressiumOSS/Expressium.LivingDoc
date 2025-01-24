using AngleSharp.Html;
using AngleSharp.Html.Parser;
using ReqnRoll.TestExecution;
using ReqnRoll.TestExecutionReport.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReqnRoll.TestExecutionReport
{
    internal class TestExecutionReportGenerator
    {
        private string filePath;
        private string outputPath;

        public TestExecutionReportGenerator(string filePath, string outputPath)
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

            // Assign Unique Identifier to all Scenarios...
            int scenarioId = 1200001;
            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    scenario.Id = "id" + scenarioId.ToString();
                    scenarioId++;
                }
            }

            // Copy Attachments to Output Directory...
            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var attachment in scenario.Attachments)
                    {
                        if (File.Exists(attachment))
                            File.Copy(attachment, Path.Combine(outputPath, "Attachments", Path.GetFileName(attachment)), true);
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
            listOfLines.Add("<body>");
            listOfLines.AddRange(GenerateHeader());
            listOfLines.AddRange(GenerateContent(executionContext));
            listOfLines.AddRange(GenerateFooter());
            listOfLines.AddRange(GenerateData(executionContext));
            listOfLines.Add("</body>");
            listOfLines.Add("</html>");

            var htmlFilePath = Path.Combine(outputPath, "LivingDoc.html");
            SaveListOfLinesToFile(htmlFilePath, listOfLines);
        }

        private List<string> GenerateHead()
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<head>");

            listOfLines.Add("<meta charset='UTF-8'>");
            listOfLines.Add("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            listOfLines.Add("<title>ReqnRoll LivingDoc</title>");
            listOfLines.Add("<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'>");

            listOfLines.AddRange(GenerateStyles());
            listOfLines.AddRange(GenerateScripts());

            listOfLines.Add($"</head>");

            return listOfLines;
        }

        internal List<string> GenerateStyles()
        {
            return Resources.Styles.Split(Environment.NewLine).ToList();
        }

        internal List<string> GenerateScripts()
        {
            return Resources.Scripts.Split(Environment.NewLine).ToList();
        }

        private List<string> GenerateHeader()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Header Section -->");
            listOfLines.Add("<header>");
            listOfLines.Add("ReqnRoll LivingDoc");
            listOfLines.Add("</header>");

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

            listOfLines.AddRange(GenerateProjectInformation(executionContext));

            listOfLines.Add("<!-- Content Overview Section -->");
            listOfLines.Add("<table>");

            listOfLines.Add("<tr><td></td></tr>");
            listOfLines.Add("<tr><td></td></tr>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<td align='center'>");
            listOfLines.AddRange(GenerateScenarioStatusChart(executionContext));
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            listOfLines.Add("<tr><td></td></tr>");
            listOfLines.Add("<tr><td></td></tr>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<td>");
            listOfLines.AddRange(GenerateScenarioFilter(executionContext));
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            listOfLines.Add("<tr>");
            listOfLines.Add("<td>");
            listOfLines.AddRange(GenerateScenarioTableGrid(executionContext));
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            listOfLines.Add("</table>");

            return listOfLines;
        }

        private List<string> GenerateProjectInformation(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Information Section -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='project-name'>" + executionContext.Title + "</span><br />");
            listOfLines.Add("<span class='project-date'>generated " + executionContext.ExecutionTime.ToString("ddd dd. MMM yyyy HH':'mm':'ss \"GMT\"z") + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateScenarioStatusChart(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            var numberOfPassed = 0;
            var numberOfSkipped = 0;
            var numberOfInconclusive = 0;
            var numberOfFailed = 0;

            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    if (scenario.IsPassed())
                        numberOfPassed++;
                    else if (scenario.IsSkipped())
                        numberOfSkipped++;
                    else if (scenario.IsFailed())
                        numberOfFailed++;
                    else
                    {
                        numberOfInconclusive++;
                    }
                }
            }

            var numberOfTests = numberOfPassed + numberOfSkipped + numberOfInconclusive + numberOfFailed;

            listOfLines.Add("<!-- Status Chart Section -->");
            listOfLines.Add("<div style='width: 80%'>");

            listOfLines.AddRange(CreateScenarioStatusChartPercentage(numberOfTests, numberOfPassed));
            listOfLines.AddRange(CreateScenarioStatusChartGraphics(numberOfTests, numberOfPassed, numberOfSkipped, numberOfInconclusive, numberOfFailed, "Testcases"));

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal static List<string> CreateScenarioStatusChartPercentage(int numberOfTests, int numberOfPassed)
        {
            var listOfLines = new List<string>();

            var procentOfPassed = numberOfPassed / (float)numberOfTests * 100;
            if (float.IsNaN(procentOfPassed))
                procentOfPassed = 0;

            listOfLines.Add("<div>");
            listOfLines.Add($"<span class='chart-percentage'>{procentOfPassed.ToString("0")}%<br></span>");
            listOfLines.Add("<span class='chart-status'>Passed</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal static List<string> CreateScenarioStatusChartGraphics(int numberOfTests, int numberOfPassed, int numberOfSkipped, int numberOfInconclusive, int numberOfFailed, string objectType)
        {
            var numberOfPassedPercent = 100.0f / numberOfTests * numberOfPassed;
            var numberOfSkippedPercent = 100.0f / numberOfTests * numberOfSkipped;
            var numberOfInconclusivePercent = 100.0f / numberOfTests * numberOfInconclusive;
            var numberOfFailedPercent = 100.0f / numberOfTests * numberOfFailed;

            if (numberOfSkippedPercent > 0.0 && numberOfSkippedPercent < 1.0f)
                numberOfSkippedPercent += 1.0f;

            if (numberOfInconclusivePercent > 0.0 && numberOfInconclusivePercent < 1.0f)
                numberOfInconclusivePercent += 1.0f;

            if (numberOfFailedPercent > 0.0 && numberOfFailedPercent < 1.0f)
                numberOfFailedPercent += 1.0f;

            var listOfLines = new List<string>();
            listOfLines.Add($"<p style='color: gray; font-style: italic; margin: 8px; '>{numberOfFailed} Failed, {numberOfInconclusive} Inconclusive, {numberOfSkipped} Skipped, {numberOfPassed} Passed {objectType}</p>");
            listOfLines.Add($"<div class='bgcolor-passed' style='width: 100%; height: 1em;'>");
            listOfLines.Add($"<div class='bgcolor-failed' style='width: {(int)numberOfFailedPercent}%; height: 1em; float: left'></div>");
            listOfLines.Add($"<div class='bgcolor-inconclusive' style='width: {(int)numberOfInconclusivePercent}%; height: 1em; float: left'></div>");
            listOfLines.Add($"<div class='bgcolor-skipped' style='width: {(int)numberOfSkippedPercent}%; height: 1em; float: left'></div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private IEnumerable<string> GenerateScenarioFilter(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features Filter Section -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<input class='filter' onkeyup='filterFunction()' id='search-filter' type='text' placeholder='Filter by Text'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateScenarioTableGrid(TestExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenarios Table Section -->");
            listOfLines.Add("<table class='grid'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            //listOfLines.Add("<th></th>");
            listOfLines.Add("<th>Feature</th>");
            listOfLines.Add("<th>Scenario</th>");
            listOfLines.Add("<th>Status</th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody id='search-list'>");

            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add($"<tr onclick='loadScenario(\"{scenario.Id}\");'>");
                    //listOfLines.Add($"<td>" + GetScenarioMarker(scenario) + "</td>");
                    listOfLines.Add($"<td>" + feature.Title + "</td>");
                    listOfLines.Add($"<td><a href='#'>" + scenario.Title + "</a></td>");
                    listOfLines.Add($"<td>" + scenario.GetStatus() + "</td>");
                    listOfLines.Add($"</tr>");
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");

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

            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<!-- Scenario Data Section -->");
                    listOfLines.Add($"<div class='data-item' id='{scenario.Id}'>");

                    listOfLines.AddRange(GenerateDataFeatureInformation(feature, scenario));
                    listOfLines.AddRange(GenerateDataScenarioInformation(feature, scenario));
                    listOfLines.AddRange(GenerateDataAttachments(scenario));

                    listOfLines.Add("</div>");
                }
            }

            return listOfLines;
        }

        private List<string> GenerateDataFeatureInformation(TestExecutionFeature feature, TestExecutionScenario scenario)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<div>");

            if (!string.IsNullOrEmpty(feature.Tags))
            {
                listOfLines.Add("<span class='tag-names'>" + GetFormattedTags(feature.Tags) + "</span>");
                listOfLines.Add("<br />");
            }

            listOfLines.Add("<span class='feature-name'>Feature: " + feature.Title + "</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='userstory'>");
            var listOfDescription = feature.Description.Trim().Split("\n");
            foreach (var line in listOfDescription)
                listOfLines.Add("<span>" + line.Trim() + "</span><br />");
            listOfLines.Add("<br />");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        private List<string> GenerateDataScenarioInformation(TestExecutionFeature feature, TestExecutionScenario scenario)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<div>");
            if (!string.IsNullOrEmpty(scenario.Tags))
            {
                listOfLines.Add("<span class='tag-names'>" + GetFormattedTags(scenario.Tags) + "</span>");
                listOfLines.Add("<br />");
            }
            listOfLines.Add("</div>");

            var duration = $"{scenario.Duration.Seconds}s {scenario.Duration.Milliseconds}ms";

            listOfLines.Add("<table>");
            listOfLines.Add("<tbody>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<td colspan='2' class='scenario-name'>" + GetScenarioMarker(scenario) + " Scenario: " + scenario.Title + "</td>");
            listOfLines.Add("<td class='duration'>" + duration + "</td>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</td>");

            foreach (var step in scenario.Steps)
            {
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td class='step-keyword'>" + step.Type + "</td>");
                listOfLines.Add($"<td>" + step.Text + "</td>");
                listOfLines.Add($"</tr>");
            }

            string message = null;
            if (scenario.Error != null)
                message = scenario.Error;
            else if (scenario.Status == TestExecutionStatuses.StepDefinitionPending.ToString())
                message = "Pending Step Definition...";
            else if (scenario.Status == TestExecutionStatuses.UndefinedStep.ToString())
                message = "Undefined Step Definition...";
            else
            {
            }

            if (message != null)
            {
                listOfLines.Add("<tr><td></td></tr>");
                listOfLines.Add("<tr><td></td></tr>");
                listOfLines.Add($"<tr class='step-failed'>");
                listOfLines.Add("<td colspan='3'>");
                listOfLines.Add(message);
                listOfLines.Add("</td>");
                listOfLines.Add("</tr>");
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");

            return listOfLines;
        }

        private List<string> GenerateDataAttachments(TestExecutionScenario scenario)
        {
            var listOfLines = new List<string>();

            if (scenario.Attachments.Count > 0)
            {
                listOfLines.Add("<br />");
                listOfLines.Add("<div class='attachments'>");
                listOfLines.Add("<span class='scenario-name'>Attachments:</span>");
                listOfLines.Add("<ul>");

                foreach (var attachment in scenario.Attachments)
                {
                    var filePath = Path.GetFileName(attachment);
                    listOfLines.Add($"<li><a target='_blank' href='./Attachments/{filePath}'>{filePath}</a></li>");
                }

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        private string GetScenarioMarker(TestExecutionScenario scenario)
        {
            var status = scenario.GetStatus().ToLower();
            if (scenario.IsPassed())
                return $"<span class='color-{status}'>&check;</span>";
            else
                return $"<span class='color-{status}'>&#x2718;</span>";
        }

        private string GetFormattedTags(string value)
        {
            var tags = value.Split(",");

            string formattedTags = null;
            foreach (var tag in tags)
                formattedTags += "@" + tag.Trim() + " ";

            return formattedTags.Trim();
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
    }
}
