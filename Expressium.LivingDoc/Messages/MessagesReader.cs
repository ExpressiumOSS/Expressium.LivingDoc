using Cucumber.Messages;
using System.IO;

namespace Expressium.LivingDoc.Messages
{
    public class MessagesReader : NdjsonMessageReader
    {
        public MessagesReader(Stream inputStream) : base(inputStream, (line) => MessagesSerializer.Deserialize(line))
        {
        }
    }
}
