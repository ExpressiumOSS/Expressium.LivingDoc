using Reqnroll.Formatters.Configuration;
using Reqnroll.Formatters.RuntimeSupport;
using Reqnroll.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc.ReqnrollPlugin
{
    public class ExpressiumFormatter : ExpressiumMessageFormatter
    {
        public string OutputFilePath { get; private set; }
        public string OutputFileTitle { get; private set; }

        public ExpressiumFormatter(IFormattersConfigurationProvider configurationProvider, IFormatterLog logger, IFileSystem fileSystem) : base(configurationProvider, logger, fileSystem, "expressium")
        {
        }

        protected override void FinalizeInitialization(string outputPath, IDictionary<string, object> formatterConfiguration, Action<bool> onInitialized)
        {
            base.FinalizeInitialization(outputPath, formatterConfiguration, onInitialized);
            OutputFilePath = outputPath;

            if (formatterConfiguration.ContainsKey("outputFileTitle"))
                OutputFileTitle = formatterConfiguration["outputFileTitle"].ToString();
        }

        public override void Dispose()
        {
            base.Dispose();

            var outputHtmlFilePath = OutputFilePath.Replace(Path.GetExtension(OutputFilePath), ".html");

            var livingDocConverter = new LivingDocConverter();
            livingDocConverter.Generate(OutputFilePath, outputHtmlFilePath, OutputFileTitle);
        }
    }
}
