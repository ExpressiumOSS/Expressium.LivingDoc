using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.IO;

namespace Expressium.LivingDoc
{
    public class LivingDocNativeConvertor
    {
        private string inputPath;
        private string outputPath;

        public LivingDocNativeConvertor(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }

        public void Execute()
        {
            try
            {
                var livingDocProject = LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputPath);
                var livingDocProjectGenerator = new LivingDocProjectGenerator();
                livingDocProjectGenerator.Generate(livingDocProject, outputPath);
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
