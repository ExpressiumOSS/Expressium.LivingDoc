using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocProjectGenerator
    {
        private LivingDocProject project;
        private LivingDocConfiguration configuration;

        internal LivingDocProjectGenerator(LivingDocProject project)
        {
            this.project = project;
            this.configuration = new LivingDocConfiguration();
        }

        internal void Generate(string outputPath)
        {
            project.Features = project.Features.OrderBy(f => f.Name).ToList();

            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateHtmlHeader());
            listOfLines.AddRange(GenerateProperties());
            listOfLines.AddRange(GenerateHead());
            listOfLines.AddRange(GenerateBody());
            listOfLines.AddRange(GenerateHtmlFooter());

            SaveHtmlFile(outputPath, listOfLines);
        }

        internal List<string> GenerateHtmlHeader()
        {
            var listOfLines = new List<string>();

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

        internal List<string> GenerateProperties()
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<!-- Expressium LivingDoc Version: {GetVersionId()} -->");

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

        internal List<string> GenerateBody()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocBodyGenerator(project, configuration);
            listOfLines.AddRange(generator.Generate());

            return listOfLines;
        }

        internal List<string> GenerateHtmlFooter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("</html>");

            return listOfLines;
        }

        internal static void SaveHtmlFile(string filePath, List<string> listOfLines)
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

        internal string GetVersionId()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }
    }
}
