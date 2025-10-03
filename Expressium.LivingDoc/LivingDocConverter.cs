using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Parsers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc
{
    public class LivingDocConverter
    {
        public LivingDocConverter()
        {
        }

        public LivingDocProject Convert(string inputPath)
        {
            try
            {
                var messagesParser = new MessagesParser();
                return messagesParser.ConvertToLivingDoc(inputPath);
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

        public void Generate(string inputPath, string outputPath, string title)
        {
            try
            {
                var messagesParser = new MessagesParser();
                var livingDocProject = messagesParser.ConvertToLivingDoc(inputPath);
                if (!string.IsNullOrEmpty(title))
                    livingDocProject.Title = title;
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
    }
}
