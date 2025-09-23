using Expressium.LivingDoc.Models;
using Io.Cucumber.Messages.Types;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Expressium.LivingDoc.Parsers
{
    internal class MessagesParser
    {

        public MessagesParser()
        {
        }

        internal LivingDocProject ConvertToLivingDoc(string filePath)
        {
            var builder = new LivingDocProjectBuilder();
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                var envelopes = new MessagesReader(fileStream);
                return builder.ConvertToLivingDoc(envelopes);
            }
        }
    }
}
