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

        /// <summary>
        /// Converts a single native LivingDoc Json file to a LivingDocProject object.
        /// </summary>
        /// <param name="inputPath"></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
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

        /// <summary>
        /// Generates a LivingDoc test report from a single LivingDoc Json file.
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
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
