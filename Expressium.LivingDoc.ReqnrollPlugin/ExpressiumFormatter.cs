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
        public string HistoryPath { get; private set; }
        public string PropertiesFilePath { get; private set; }

        public ExpressiumFormatter(IFormattersConfigurationProvider configurationProvider, IFormatterLog logger, IFileSystem fileSystem) : base(configurationProvider, logger, fileSystem, "expressium")
        {
        }

        protected override void FinalizeInitialization(string outputPath, IDictionary<string, object> formatterConfiguration, Action<bool> onInitialized)
        {
            base.FinalizeInitialization(outputPath, formatterConfiguration, onInitialized);
            OutputFilePath = outputPath;

            if (formatterConfiguration.ContainsKey("outputFileTitle"))
                OutputFileTitle = formatterConfiguration["outputFileTitle"].ToString();

            if (formatterConfiguration.ContainsKey("historyPath"))
                HistoryPath = formatterConfiguration["historyPath"].ToString();

            if (formatterConfiguration.ContainsKey("propertiesFilePath"))
                PropertiesFilePath = formatterConfiguration["propertiesFilePath"].ToString();
        }

        public override void Dispose()
        {
            base.Dispose();

            var outputHtmlFilePath = Path.ChangeExtension(OutputFilePath, ".html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(OutputFilePath, OutputFileTitle, PropertiesFilePath);

            if (!string.IsNullOrWhiteSpace(HistoryPath))
            {
                var historyDirectory = Path.GetDirectoryName(Path.GetFullPath(HistoryPath));
                if (!Directory.Exists(historyDirectory))
                    Directory.CreateDirectory(historyDirectory);

                var historyFileName = Path.Combine(historyDirectory, livingDocProject.Date.ToString("yyyyMMddHHmmss") + ".ndjson");
                File.Copy(OutputFilePath, historyFileName, true);

                livingDocConverter.MergeHistory(livingDocProject, HistoryPath);
            }

            livingDocConverter.Generate(livingDocProject, outputHtmlFilePath);
        }
    }
}
