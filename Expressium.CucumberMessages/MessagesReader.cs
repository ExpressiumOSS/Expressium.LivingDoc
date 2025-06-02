using Cucumber.Messages;
using System.IO;

namespace Expressium.CucumberMessages
{
    public class MessagesReader : NdjsonMessageReader
    {
        public MessagesReader(Stream inputStream) : base(inputStream, (string line) => MessagesSerializer.Deserialize(line))
        {
        }
    }
}
