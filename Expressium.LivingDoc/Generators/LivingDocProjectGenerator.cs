using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocProjectGenerator
    {
        private LivingDocProject project;

        internal LivingDocProjectGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal void Generate(string outputPath)
        {
            project.Features = project.Features.OrderBy(f => f.Name).ToList();

            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateHtmlHeader());
            listOfLines.AddRange(GenerateProperties());
            listOfLines.AddRange(GenerateHead());
            listOfLines.AddRange(GenerateBodyHeader());
            listOfLines.AddRange(GenerateContent());
            listOfLines.AddRange(GenerateData());
            listOfLines.AddRange(GenerateBodyFooter());
            listOfLines.AddRange(GenerateHtmlFooter());

            SaveHtmlFile(outputPath, listOfLines);
        }

        internal List<string> GenerateHtmlHeader()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!DOCTYPE html>");
            listOfLines.Add("<html lang='en'>");

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

            listOfLines.Add($"<!-- {project.OsVersion} -->");
            listOfLines.Add($"<!-- {project.GetApplicationName()} {project.GetApplicationVersion()} -->");
            listOfLines.Add($"<!-- {project.ImplementationName} {project.ImplementationVersion} -->");

            return listOfLines;
        }

        internal List<string> GenerateStyles()
        {
            if (project.ExperimentFlag)
                return Resources.StylesFacelift.Split(Environment.NewLine).ToList();
            else
                return Resources.Styles.Split(Environment.NewLine).ToList();
        }

        internal List<string> GenerateScripts()
        {
            return Resources.Scripts.Split(Environment.NewLine).ToList();
        }

        internal List<string> GenerateBodyHeader()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<body onload=\"loadViewMode('project-view','Overview'); loadAnalytics(); onloadFilter()\">");

            return listOfLines;
        }

        internal List<string> GenerateContent()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocContentGenerator(project);
            listOfLines.AddRange(generator.GenerateHeader());
            listOfLines.AddRange(generator.GenerateNavigation());
            listOfLines.AddRange(generator.GenerateSplitter());
            listOfLines.AddRange(generator.GenerateFooter());

            return listOfLines;
        }

        internal List<string> GenerateData()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<data style='display: none;'>");

            var generator = new LivingDocDataGenerator(project);
            listOfLines.AddRange(generator.GenerateDataOverview());
            listOfLines.AddRange(generator.GenerateDataListViews());
            listOfLines.AddRange(generator.GenerateDataObjects());
            listOfLines.AddRange(generator.GenerateDataAnalytics());

            listOfLines.Add("</data>");

            return listOfLines;
        }

        internal List<string> GenerateBodyFooter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("</body>");

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
            try
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
            catch (IOException ex)
            {
                throw new IOException($"IO error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Unexpected error: {ex.Message}", ex);
            }
        }
    }
}
