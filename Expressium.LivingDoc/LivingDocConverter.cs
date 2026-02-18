using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc
{
    public class LivingDocConverter
    {
        public LivingDocConverter()
        {
        }

        /// <summary>
        /// Converts a single Cucumber Messages NdJson file to a LivingDocProject object.
        /// </summary>
        /// <param name="inputPath"></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public LivingDocProject Convert(string inputPath, string title)
        {
            try
            {
                var messagesParser = new MessagesParser();
                var livingDocProject = messagesParser.ConvertToLivingDoc(inputPath);
                if (!string.IsNullOrEmpty(title))
                    livingDocProject.Title = title;
                return livingDocProject;
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

        /// <summary>
        /// Generates a LivingDoc test report from an existing LivingDoc project.
        /// </summary>
        /// <param name="livingDocProject"></param>
        /// <param name="outputPath"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public void Generate(LivingDocProject livingDocProject, string outputPath)
        {
            try
            {
                var livingDocProjectGenerator = new LivingDocProjectGenerator(livingDocProject);
                livingDocProjectGenerator.Generate(outputPath);
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

        /// <summary>
        /// Generates a LivingDoc test report from a single Cucumber Messages NdJson file.
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="title"></param>
        /// <param name="historyPath"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public void Generate(string inputPath, string outputPath, string title, string historyPath = null)
        {
            try
            {
                var messagesParser = new MessagesParser();
                var livingDocProject = messagesParser.ConvertToLivingDoc(inputPath);
                if (!string.IsNullOrEmpty(title))
                    livingDocProject.Title = title;

                if (!string.IsNullOrEmpty(historyPath))
                    MergeHistory(livingDocProject, historyPath);

                var livingDocProjectGenerator = new LivingDocProjectGenerator(livingDocProject);
                livingDocProjectGenerator.Generate(outputPath);
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

        /// <summary>
        /// Generates a LivingDoc test report from multiple Cucumber Messages NdJson files.    
        /// </summary>
        /// <param name="inputPaths"></param>
        /// <param name="outputPath"></param>
        /// <param name="title"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public void Generate(List<string> inputPaths, string outputPath, string title)
        {
            try
            {
                var messagesParser = new MessagesParser();
                var livingDocProjectMaster = messagesParser.ConvertToLivingDoc(inputPaths[0]);
                if (!string.IsNullOrEmpty(title))
                    livingDocProjectMaster.Title = title;

                for (int i = 1; i < inputPaths.Count; i++)
                {
                    var messagesParserSlave = new MessagesParser();
                    var livingDocProjectSlave = messagesParserSlave.ConvertToLivingDoc(inputPaths[i]);
                    livingDocProjectMaster.Merge(livingDocProjectSlave);
                }

                var livingDocProjectGenerator = new LivingDocProjectGenerator(livingDocProjectMaster);
                livingDocProjectGenerator.Generate(outputPath);
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

        private void MergeHistory(LivingDocProject livingDocProject, string historyPath)
        {
            Console.WriteLine("  Merging History...");

            var historyDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(historyPath)));
            var historyFiles = Path.GetFileNameWithoutExtension(historyPath) + "*.ndjson";

            var files = Directory
                    .GetFiles(historyDirectory, historyFiles, SearchOption.AllDirectories)
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .ToArray();

            var historyLimit = 4;

            foreach (var file in files)
            {
                var historyFile = file.Replace(Path.GetDirectoryName(Path.GetFullPath(historyPath)), ".");
                Console.WriteLine("    " + historyFile.ToString() + "...");

                string reportUrl = historyFile.Replace("ndjson", "html");
                if (!File.Exists(reportUrl))
                    reportUrl = null;

                var livingDocConverterSlave = new LivingDocConverter();
                var livingDocProjectSlave = livingDocConverterSlave.Convert(file, null);
                livingDocProject.MergeHistory(livingDocProjectSlave, null);

                if (livingDocProject.Histories.Count >= historyLimit)
                    break;
            }
        }
    }
}
