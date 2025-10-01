using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Parsers;
using System;
using System.IO;

namespace Expressium.LivingDoc
{
    public class LivingDocConverter
    {
        private string inputPath;
        private string outputPath;
        private string title;

        public LivingDocConverter(string inputPath, string outputPath, string title)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            this.title = title;
        }

        public void Execute()
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
    }
}
