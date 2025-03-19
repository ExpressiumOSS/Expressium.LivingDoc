using Cucumber.Messages;
using System.IO;

namespace Expressium.CucumberMessages
{
    public class NdjsonMessageReaderSUT : NdjsonMessageReader
    {
        public NdjsonMessageReaderSUT(Stream inputStream) : base(inputStream, (string line) => NdjsonSerializer.Deserialize(line))
        {
        }
    }
}
