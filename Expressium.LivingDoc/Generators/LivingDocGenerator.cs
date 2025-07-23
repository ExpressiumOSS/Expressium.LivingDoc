using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    public class LivingDocGenerator
    {
        private string inputPath;
        private string outputPath;

        public LivingDocGenerator(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }

        public void Execute(bool useNativeFormat = false)
        {
            try
            {
                if (useNativeFormat)
                {
                    var project = ParseLivingDocJsonFile();
                    GenerateDocument(project);
                }
                else
                {
                    var project = ParseCucumberMessagesJsonFile();
                    project.Title = Path.GetFileName(Path.GetFileNameWithoutExtension(outputPath));
                    GenerateDocument(project);
                }
            }
            catch (IOException ex)
            {
                throw new IOException($"IO error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new IOException($"Unexpected error: {ex.Message}");
            }
        }

        internal LivingDocProject ParseCucumberMessagesJsonFile()
        {
            return MessagesConvertor.ConvertToLivingDoc(inputPath);
        }

        internal LivingDocProject ParseLivingDocJsonFile()
        {
            return LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputPath);
        }

        internal void GenerateDocument(LivingDocProject project)
        {
            int orderId = 1;
            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                    scenario.Order = orderId++;
            }

            project.Features = project.Features.OrderBy(f => f.Name).ToList();

            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateHtmlHeader());
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

            var bodyGenerator = new LivingDocBodyGenerator();
            listOfLines.AddRange(bodyGenerator.GenerateBody(project));

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
    }
}
