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
        public string HistoryFilePath { get; private set; }

        public ExpressiumFormatter(IFormattersConfigurationProvider configurationProvider, IFormatterLog logger, IFileSystem fileSystem) : base(configurationProvider, logger, fileSystem, "expressium")
        {
        }

        protected override void FinalizeInitialization(string outputPath, IDictionary<string, object> formatterConfiguration, Action<bool> onInitialized)
        {
            base.FinalizeInitialization(outputPath, formatterConfiguration, onInitialized);
            OutputFilePath = outputPath;

            if (formatterConfiguration.ContainsKey("outputFileTitle"))
                OutputFileTitle = formatterConfiguration["outputFileTitle"].ToString();

            if (formatterConfiguration.ContainsKey("historyFilePath"))
                HistoryFilePath = formatterConfiguration["historyFilePath"].ToString();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (!string.IsNullOrWhiteSpace(HistoryFilePath))
            {
                var livingDocConverter = new LivingDocConverter();
                var livingDocProject = livingDocConverter.Convert(OutputFilePath, OutputFileTitle);

                var historyDirectory = Path.GetDirectoryName(Path.GetFullPath(HistoryFilePath));
                if (!Directory.Exists(historyDirectory))
                    Directory.CreateDirectory(historyDirectory);

                var historyFileName = Path.Combine(historyDirectory, livingDocProject.Date.ToString("yyyyMMddHHmmss") + ".ndjson");
                File.Copy(OutputFilePath, historyFileName, true);

                livingDocConverter.MergeHistory(livingDocProject, HistoryFilePath);

                var outputHtmlFilePath = OutputFilePath.Replace(Path.GetExtension(OutputFilePath), ".html");
                livingDocConverter.Generate(livingDocProject, outputHtmlFilePath);
            }
            else
            {
                var outputHtmlFilePath = OutputFilePath.Replace(Path.GetExtension(OutputFilePath), ".html");

                var livingDocConverter = new LivingDocConverter();
                var livingDocProject = livingDocConverter.Convert(OutputFilePath, OutputFileTitle);
                livingDocConverter.Generate(livingDocProject, outputHtmlFilePath);
            }
        }
    }
}
