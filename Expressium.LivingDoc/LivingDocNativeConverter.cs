using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Parsers;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Generates a LivingDoc test report from multiple LivingDoc Json files.    
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
                var livingDocProjectMaster = LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputPaths[0]);
                if (!string.IsNullOrEmpty(title))
                    livingDocProjectMaster.Title = title;

                for (int i = 1; i < inputPaths.Count; i++)
                {
                    var livingDocProjectSlave = LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputPaths[i]);
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

        /// <summary>
        /// Saves a LivingDoc project as a LivingDoc Json file.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="outputPath"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public void Save(LivingDocProject project, string outputPath)
        {
            try
            {
                LivingDocSerializer.SerializeAsJson(outputPath, project);
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
