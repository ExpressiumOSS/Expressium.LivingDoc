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

        private bool includeNavigation = false;

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
            var project = TestExecutionUtilities.DeserializeAsJson<TestExecutionProject>(filePath);

            Console.WriteLine("Creating Test Execution Output Directories...");
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);
            Directory.CreateDirectory(Path.Combine(outputPath, "Attachments"));

            // Sort list of Features by Tags...
            project.OrderByTags();

            // Assign Unique Identifier to all Scenarios...
            foreach (var feature in project.Features)
            {
                feature.Id = Guid.NewGuid().ToString();

                foreach (var scenario in feature.Scenarios)
                    scenario.Id = Guid.NewGuid().ToString();
            }

            // Copy Attachments to Output Directory...
            foreach (var feature in project.Features)
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
            GenerateTestExecutionReport(project);

            Console.WriteLine("Generating Test Execution Report Completed");
            Console.WriteLine("");
        }

        internal void GenerateTestExecutionReport(TestExecutionProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.AddRange(GenerateHtmlHeader());
            listOfLines.AddRange(GenerateHead());
            listOfLines.AddRange(GenerateBodyHeader());
            listOfLines.AddRange(GenerateHeader(project));
            listOfLines.AddRange(GenerateNavigation(project));
            listOfLines.AddRange(GenerateContent(project));
            listOfLines.AddRange(GenerateFooter(project));
            listOfLines.AddRange(GenerateData(project));
            listOfLines.AddRange(GenerateBodyFooter());
            listOfLines.AddRange(GenerateHtmlFooter());

            var htmlFilePath = Path.Combine(outputPath, "LivingDoc.html");
            SaveListOfLinesToFile(htmlFilePath, listOfLines);
        }

        internal List<string> GenerateHtmlHeader()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!DOCTYPE html>");
            listOfLines.Add("<html>");

            return listOfLines;
        }

        internal List<string> GenerateHtmlFooter()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("</html>");

            return listOfLines;
        }

        internal List<string> GenerateBodyHeader()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<body onload=\"loadAnalytics('analytics'); loadViewmode('treeview');\">");

            return listOfLines;
        }

        internal List<string> GenerateBodyFooter()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("</body>");

            return listOfLines;
        }

        internal static void SaveListOfLinesToFile(string filePath, List<string> listOfLines)
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

        internal List<string> GenerateHead()
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<head>");

            listOfLines.AddRange(GenerateHeads());
            listOfLines.AddRange(GenerateStyles());
            listOfLines.AddRange(GenerateScripts());

            listOfLines.Add($"</head>");

            return listOfLines;
        }

        internal List<string> GenerateHeads()
        {
            return Resources.Heads.Split(Environment.NewLine).ToList();
        }

        internal List<string> GenerateStyles()
        {
            return Resources.Styles.Split(Environment.NewLine).ToList();
        }

        internal List<string> GenerateScripts()
        {
            return Resources.Scripts.Split(Environment.NewLine).ToList();
        }

        internal List<string> GenerateHeader(TestExecutionProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Header Section -->");
            listOfLines.Add("<header>");
            listOfLines.Add("<span class='project-name'>" + project.Title + "</span><br />");
            listOfLines.Add("<span class='project-date'>generated " + project.GetExecutionTime() + "</span>");
            listOfLines.Add("</header>");

            return listOfLines;
        }

        internal List<string> GenerateNavigation(TestExecutionProject project)
        {
            List<string> listOfLines = new List<string>();

            if (includeNavigation)
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

        internal List<string> GenerateContent(TestExecutionProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Content Wrapper Section -->");
            listOfLines.Add("<div id='content-wrapper'>");

            listOfLines.Add("<!-- Left Content Section -->");
            listOfLines.Add("<div id='left-section' class='bg-light p-3'>");
            listOfLines.AddRange(GenerateScenarioPreFilters(project));
            listOfLines.AddRange(GenerateScenarioFilter(project));
            listOfLines.Add("<div id='scenario-view'></div>");
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

        internal List<string> GenerateScenarioPreFilters(TestExecutionProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features PreFilters Section -->");
            listOfLines.Add("<div class='section prefilters'>");

            listOfLines.Add("<button title='Preset Filter with Passed' class='color-passed' onclick='presetScenarios(\"passed\")'>Passed</button>");
            listOfLines.Add("<button title='Preset Filter with Incomplete' class='color-incomplete' onclick='presetScenarios(\"incomplete\")'>Incomplete</button>");
            listOfLines.Add("<button title='Preset Filter with Failed' class='color-failed' onclick='presetScenarios(\"failed\")'>Failed</button>");
            listOfLines.Add("<button title='Preset Filter with Skipped' class='color-skipped' onclick='presetScenarios(\"skipped\")'>Skipped</button>");
            listOfLines.Add("<button title='Clear Filter' onclick='presetScenarios(\"\")'>Clear</button>");
            listOfLines.Add("<button id='viewmode' name='treeview' title='Toggle View Mode' onclick='toggleViewmode()'>&#9783;</button>");
            
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateScenarioFilter(TestExecutionProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features Filter Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<input class='filter' onkeyup='filterScenarios()' id='scenario-filter' type='text' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFooter(TestExecutionProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Footer Section -->");
            listOfLines.Add("<footer>");
            listOfLines.Add("©2025 Expressium All Rights Reserved");
            listOfLines.Add("</footer>");

            return listOfLines;
        }

        internal List<string> GenerateData(TestExecutionProject project)
        {
            var listOfLines = new List<string>();

            var dataGenerator = new TestExecutionReportDataGenerator();

            listOfLines.AddRange(dataGenerator.GenerateProjectDataListViewSections(project));
            listOfLines.AddRange(dataGenerator.GenerateProjectDataTreeViewSections(project));
            listOfLines.AddRange(dataGenerator.GenerateFeatureDataSections(project));
            listOfLines.AddRange(dataGenerator.GenerateScenarioDataSections(project));
            listOfLines.AddRange(dataGenerator.GenerateProjectDataAnalyticsSection(project));

            return listOfLines;
        }
    }
}

// HTML Colors
// https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=4682B4

// HTML Symbols
// https://www.toptal.com/designers/htmlarrows/symbols