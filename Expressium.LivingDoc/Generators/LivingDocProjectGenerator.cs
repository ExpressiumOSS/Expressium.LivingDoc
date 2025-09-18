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
        internal void Generate(LivingDocProject project, string outputPath)
        {
            project.Features = project.Features.OrderBy(f => f.Name).ToList();

            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateHtmlHeader());
            listOfLines.AddRange(GenerateProperties());
            listOfLines.AddRange(GenerateHead());
            listOfLines.AddRange(GenerateBody(project));
            listOfLines.AddRange(GenerateHtmlFooter());

            var htmlFilePath = outputPath;
            SaveHtmlFile(htmlFilePath, listOfLines);
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

        internal List<string> GenerateBody(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocBodyGenerator();
            listOfLines.AddRange(generator.Generate(project));

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
