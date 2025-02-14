using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Expressium.TestExecution;
using Expressium.LivingDoc.Extensions;
using Expressium.LivingDoc.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc
{
    public class LivingDocGenerator
    {
        private string filePath;
        private string outputPath;

        public LivingDocGenerator(string filePath, string outputPath)
        {
            this.filePath = filePath;
            this.outputPath = outputPath;
        }

        public void Execute()
        {
            Console.WriteLine("");
            Console.WriteLine("Generating Test Execution Report...");
            Console.WriteLine("FilePath: " + filePath);
            Console.WriteLine("OutputPath: " + outputPath);
            Console.WriteLine("");

            Console.WriteLine("Parsing Test Execution JSON File...");
            var project = LivingDocUtilities.DeserializeAsJson<LivingDocProject>(filePath);

            Console.WriteLine("Creating Test Execution Output Directories...");
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);
            Directory.CreateDirectory(Path.Combine(outputPath, "Attachments"));

            Console.WriteLine("Copy Attachments to Output Directory...");
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

            Console.WriteLine("Assign Unique Identifier to Features & Scenarios...");
            int indexId = 1;
            foreach (var feature in project.Features)
            {
                feature.Id = Guid.NewGuid().ToString();

                foreach (var scenario in feature.Scenarios)
                {
                    scenario.Id = Guid.NewGuid().ToString();
                    scenario.Index = indexId++;
                }
            }

            //Console.WriteLine("Sort Features & Scenarios by Tags...");
            //project.OrderByTags();

            Console.WriteLine("Generating Test Execution HTML Report...");
            GenerateHtmlReport(project);

            Console.WriteLine("Generating Test Execution Report Completed");
            Console.WriteLine("");
        }

        internal void GenerateHtmlReport(LivingDocProject project)
        {
            List<string> listOfLines = new List<string>();

            var bodyGenerator = new LivingDocBodyGenerator();

            listOfLines.AddRange(GenerateHtmlHeader());
            listOfLines.AddRange(GenerateHead());
            listOfLines.AddRange(bodyGenerator.GenerateBody(project));
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

        internal List<string> GenerateHtmlFooter()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("</html>");

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
    }
}
