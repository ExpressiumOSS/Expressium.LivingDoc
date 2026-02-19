using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Parsers;
using System;
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
        /// Imports a single native LivingDoc Json file to a LivingDocProject object.
        /// </summary>
        /// <param name="inputPath"></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public LivingDocProject Import(string inputPath)
        {
            try
            {
                return LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputPath);
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
        /// Exports a LivingDocProject object to single native LivingDoc Json file.
        /// </summary>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public void Export(LivingDocProject livingDocProject, string outputPath)
        {
            try
            {
                LivingDocSerializer.SerializeAsJson(outputPath, livingDocProject);
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
        /// Converts a Cucumber Messages NDJSON file to a LivingDocProject object.
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="title"></param>
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
        /// Generates a LivingDoc test report from an existing LivingDocProject object.
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
        /// Merge a Cucumber Messages NDJSON file into an exiting LivingDoc object.
        /// </summary>
        /// <param name="livingDocProject"></param> 
        /// <param name="inputPath"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public void MergeProject(LivingDocProject livingDocProject, string inputPath)
        {
            try
            {
                var messagesParser = new MessagesParser();
                var livingDocProjectSlave = messagesParser.ConvertToLivingDoc(inputPath);
                livingDocProject.Merge(livingDocProjectSlave);
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
        /// Merge historical data from Cucumber Messages NDJSON files into an exiting LivingDoc object.   
        /// </summary>
        /// <param name="livingDocProject"></param>
        /// <param name="historyPath"></param>
        public void MergeHistory(LivingDocProject livingDocProject, string historyPath)
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

                var livingDocConverterSlave = new LivingDocConverter();
                var livingDocProjectSlave = livingDocConverterSlave.Convert(file, null);
                livingDocProject.MergeHistory(livingDocProjectSlave);

                if (livingDocProject.Histories.Count >= historyLimit)
                    break;
            }
        }
    }
}
