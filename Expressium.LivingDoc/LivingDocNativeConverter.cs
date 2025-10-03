using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.IO;

namespace Expressium.LivingDoc
{
    public class LivingDocNativeConverter
    {
        public LivingDocNativeConverter()
        {
        }

        public LivingDocProject Convert(string inputPath)
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

        public void Generate(string inputPath, string outputPath)
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
