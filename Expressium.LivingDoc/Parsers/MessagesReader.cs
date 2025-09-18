using Cucumber.Messages;
using System.IO;

namespace Expressium.LivingDoc.Parsers
{
    internal class MessagesReader : NdjsonMessageReader
    {
        internal MessagesReader(Stream inputStream) : base(inputStream, (line) => MessagesSerializer.Deserialize(line))
        {
        }
    }
}
