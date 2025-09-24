using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Parsers;
using Io.Cucumber.Messages.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc
{
    public class LivingDocEnvelopeStreamConverter
    {
        private string outputPath;
        private string title;

        public LivingDocEnvelopeStreamConverter(string outputPath, string title)
        {
            this.outputPath = outputPath;
            this.title = title;
        }

        public void Execute(IEnumerable<Envelope> envelopes)
        {
            try
            {
                var builder = new LivingDocProjectBuilder();
                var livingDocProject = builder.ConvertToLivingDoc(envelopes);
                if (!string.IsNullOrEmpty(title))
                    livingDocProject.Title = title;
                var livingDocProjectGenerator = new LivingDocProjectGenerator();
                livingDocProjectGenerator.Generate(livingDocProject, outputPath);
            }
            catch (IOException ex)
            {
                throw new IOException($"IO error: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Unexpected error: {ex.Message}", ex);
            }
        }
    }
}
