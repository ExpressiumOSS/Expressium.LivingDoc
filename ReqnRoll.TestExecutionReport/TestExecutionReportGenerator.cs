using ReqnRoll.TestExecution;
using System;
using System.Collections.Generic;
using System.IO;

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
            Console.WriteLine("Generating ReqnRoll Test Execution Report...");
            Console.WriteLine("FilePath: " + filePath);
            Console.WriteLine("OutputPath: " + outputPath);
            Console.WriteLine("");

            Console.WriteLine("Parsing Test Execution Output File...");
            var executionContext = ExecutionUtilities.DeserializeAsJson<TestExecution.ExecutionContext>(filePath);

            Console.WriteLine("Creating Test Execution Output Directory...");
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);

            List<string> listOfLines = new List<string>();

            listOfLines.AddRange(GenerateStyles());
            listOfLines.Add("<body>");
            listOfLines.AddRange(GenerateHeader());

            listOfLines.Add("<h1>Summary</h1>");

            listOfLines.AddRange(GenerateFeaturesOverview(executionContext));
            listOfLines.AddRange(GenerateFeatureScenariosOveview(executionContext));

            listOfLines.AddRange(GenerateFooter());
            listOfLines.Add("</body>");

            Console.WriteLine("Generating Test Execution Report HTML Page...");
            var htmlFilePath = Path.Combine(outputPath, "LivingDoc.html");
            SaveListOfLinesToFile(htmlFilePath, listOfLines);
        }

        private List<string> GenerateHeader()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add($"<header>");
            listOfLines.Add($"<div>ReqnRoll Test Report</div>");
            listOfLines.Add($"</header>");

            return listOfLines;
        }

        private List<string> GenerateFooter()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add($"<footer>");
            listOfLines.Add($"<div>©2025 Expressium All Rights Reserved</div>");
            listOfLines.Add($"</footer>");

            return listOfLines;
        }

        internal List<string> GenerateStyles()
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<head>");

            listOfLines.Add("<style>");
            listOfLines.Add("body { font-family: Arial; font-size: 15px; }");
            listOfLines.Add("header { font-size: 20px; padding: 10px; text-align: center; background-color: lightgray; }");
            listOfLines.Add("footer { font-size: 20px; padding: 10px; text-align: center; background-color: lightgray; }");
            listOfLines.Add("h1 { margin-bottom: 4px; }");
            listOfLines.Add("h2 { margin-bottom: 4px; }");
            listOfLines.Add("td { padding: 4px; }");
            listOfLines.Add("th { background-color: #2471A3; color: white; padding: 4px; }");
            listOfLines.Add("</style>");

            listOfLines.Add($"</head>");

            return listOfLines;
        }

        private List<string> GenerateFeaturesOverview(TestExecution.ExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<h2>Overview</h2>");

            listOfLines.Add("<table border='1'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th>Feature</th>");
            listOfLines.Add("<th>Scenario</th>");
            listOfLines.Add("<th>Status</th>");
            listOfLines.Add("<th>Duration</th>");
            listOfLines.Add("</thead>");
            listOfLines.Add("</tr>");
            listOfLines.Add("<tbody>");

            foreach (var feature in executionContext.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<tr>");
                    listOfLines.Add("<td>" + feature.Title + "</td>");
                    listOfLines.Add("<td>" + scenario.Title + "</td>");
                    listOfLines.Add("<td>" + scenario.Status + "</td>");
                    listOfLines.Add("<td>" + scenario.Duration + "</td>");
                    listOfLines.Add("</tr>");
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");

            return listOfLines;
        }

        private List<string> GenerateFeatureScenariosOveview(TestExecution.ExecutionContext executionContext)
        {
            List<string> listOfLines = new List<string>();

            foreach (var feature in executionContext.Features)
            {
                listOfLines.Add("<br />");
                listOfLines.Add("<hr>");
                listOfLines.Add("<h2>Feature: " + feature.Title + "</h2>");
                listOfLines.Add("<pre>");
                listOfLines.Add(feature.Description);
                listOfLines.Add("</pre>");

                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<p>");
                    listOfLines.Add("@" + string.Join(" @", scenario.Tags.Split(",")) + "<br />");
                    listOfLines.Add("Scenario: " + scenario.Title + "<br />");

                    foreach (var step in scenario.Steps)
                    {
                        listOfLines.Add("&nbsp;&nbsp;" + step.Type + " " + step.Text + "<br />");
                        if (step.Error != null)
                        {
                            listOfLines.Add("<i>&nbsp;&nbsp;&nbsp;&nbsp;Error: " + step.Error + "</i><br />");
                        }
                    }

                    listOfLines.Add("</p>");
                }
            }

            return listOfLines;
        }

        private static void SaveListOfLinesToFile(string filePath, List<string> listOfLines)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                foreach (var line in listOfLines)
                    streamWriter.WriteLine(line);
            }

            Console.WriteLine(filePath);
        }
    }
}
