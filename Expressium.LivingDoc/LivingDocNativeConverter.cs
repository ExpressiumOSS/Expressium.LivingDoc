using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.IO;

namespace Expressium.LivingDoc
{
    public class LivingDocNativeConverter
    {
        private string inputPath;
        private string outputPath;

        public LivingDocNativeConverter(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }

        public void Execute()
        {
            try
            {
                var livingDocProject = LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputPath);
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
