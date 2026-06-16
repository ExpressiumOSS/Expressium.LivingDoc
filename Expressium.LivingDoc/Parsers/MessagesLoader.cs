using System.IO;

namespace Expressium.LivingDoc.Parsers
{
    internal class MessagesLoader
    {
        internal CucumberMessages LoadCucumberMessages(string filePath)
        {
            var messages = new CucumberMessages();

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                var enumerator = new MessagesReader(fileStream).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var envelope = enumerator.Current;

                    if (envelope.Meta != null)
                        messages.Metas.Add(envelope.Meta);

                    if (envelope.GherkinDocument != null)
                        messages.GherkinDocuments.Add(envelope.GherkinDocument);

                    if (envelope.Pickle != null)
                        messages.Pickles.Add(envelope.Pickle);

                    if (envelope.TestCase != null)
                        messages.TestCases.Add(envelope.TestCase);

                    if (envelope.TestStepFinished != null)
                        messages.TestStepFinished.Add(envelope.TestStepFinished);

                    if (envelope.TestCaseStarted != null)
                        messages.TestCaseStarted.Add(envelope.TestCaseStarted);

                    if (envelope.TestCaseFinished != null)
                        messages.TestCaseFinished.Add(envelope.TestCaseFinished);

                    if (envelope.TestRunStarted != null)
                        messages.TestRunStarted.Add(envelope.TestRunStarted);

                    if (envelope.TestRunFinished != null)
                        messages.TestRunFinished.Add(envelope.TestRunFinished);

                    if (envelope.Attachment != null)
                        messages.Attachments.Add(envelope.Attachment);

                    if (envelope.Hook != null)
                        messages.Hooks.Add(envelope.Hook);
                }
            }

            return messages;
        }
    }
}
