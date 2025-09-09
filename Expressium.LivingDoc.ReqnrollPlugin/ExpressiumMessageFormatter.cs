using Io.Cucumber.Messages.Types;
using Reqnroll.Formatters;
using Reqnroll.Formatters.Configuration;
using Reqnroll.Formatters.PayloadProcessing;
using Reqnroll.Formatters.RuntimeSupport;
using Reqnroll.Utils;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expressium.LivingDoc.ReqnrollPlugin
{
    public class ExpressiumMessageFormatter : FileWritingFormatterBase
    {
        private readonly byte[] _newLineBytes = Encoding.UTF8.GetBytes(Environment.NewLine);

        public ExpressiumMessageFormatter(IFormattersConfigurationProvider configurationProvider, IFormatterLog logger, IFileSystem fileSystem, string name) : base(configurationProvider, logger, fileSystem, name, ".ndjson", "reqnroll_report.ndjson")
        {
        }

        protected override void OnTargetFileStreamInitialized(Stream targetFileStream)
        {
        }

        protected override void OnTargetFileStreamDisposing()
        {
        }

        protected override async Task WriteToFile(Envelope envelope, CancellationToken cancellationToken)
        {
            if (TargetFileStream != null)
            {
                await NdjsonSerializer.SerializeToStreamAsync(TargetFileStream, envelope);

                if (envelope.TestRunFinished == null)
                    await TargetFileStream.WriteAsync(_newLineBytes, 0, _newLineBytes.Length, cancellationToken);
            }
        }
    }
}