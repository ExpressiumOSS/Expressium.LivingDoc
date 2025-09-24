using Io.Cucumber.Messages.Types;
using Reqnroll.Formatters;
using Reqnroll.Formatters.Configuration;
using Reqnroll.Formatters.RuntimeSupport;
using Reqnroll.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Expressium.LivingDoc.ReqnrollPlugin
{
    public class ExpressiumFormatter : FormatterBase
    {
        public string OutputFilePath { get; private set; }
        public string OutputFileTitle { get; private set; }

        private LivingDocEnvelopeStreamConverter _converter;
        private List<Envelope> envelopes = new();

        public ExpressiumFormatter(IFormattersConfigurationProvider configurationProvider, IFormatterLog logger, IFileSystem fileSystem) : base(configurationProvider, logger, "expressium")
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void LaunchInner(IDictionary<string, object> formatterConfiguration, Action<bool> onAfterInitialization)
        {
            string outputFilePath = string.Empty;
            if (formatterConfiguration.TryGetValue("outputFilePath", out var outputPathElement))
            {
                outputFilePath = outputPathElement?.ToString() ?? string.Empty; // Ensure null-coalescing to handle possible null values.
            }
            OutputFilePath = outputFilePath;
            var outputHtmlFilePath = Path.GetFileNameWithoutExtension(OutputFilePath) + ".html";

            if (formatterConfiguration.ContainsKey("outputFileTitle"))
                OutputFileTitle = formatterConfiguration["outputFileTitle"].ToString();

            _converter = new LivingDocEnvelopeStreamConverter(outputHtmlFilePath, OutputFileTitle);
            onAfterInitialization(true);
        }

        protected override async Task ConsumeAndFormatMessagesBackgroundTask(CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var message in PostedMessages.Reader.ReadAllAsync(cancellationToken))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Logger.WriteMessage($"Formatter {Name} has been cancelled.");
                        break;
                    }
                    envelopes.Add(message);
                }
            }
            catch (OperationCanceledException)
            {
                Logger.WriteMessage($"Formatter {Name} has been cancelled.");
            }
            catch (System.Exception e)
            {
                Logger.WriteMessage($"Formatter {Name} threw an exception: {e.Message}. No further messages will be processed."
                        + Environment.NewLine
                        + e.ToString());
                throw;
            }
            finally
            {
                Closed = true;
                try
                {
                    _converter.Execute(envelopes);
                }
                catch (System.Exception e)
                {
                    Logger.WriteMessage($"Formatter {Name} file stream flush threw an exception: {e.Message}."
                        + Environment.NewLine
                        + e.ToString());
                }
            }
        }
    }
}
