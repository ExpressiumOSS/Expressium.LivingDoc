using Cucumber.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expressium.CucumberMessages
{
    public class NdjsonMessageReaderSUT : NdjsonMessageReader
    {
        public NdjsonMessageReaderSUT(Stream inputStream) : base(inputStream, (string line) => NdjsonSerializer.Deserialize(line))
        {
        }
    }
}
