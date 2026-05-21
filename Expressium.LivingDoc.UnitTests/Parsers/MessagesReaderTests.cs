using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    public class MessagesReaderTests
    {
        [Test]
        public void Reading_Cucumber_Messages_File()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "examples-tables", "examples-tables.ndjson");

            int numberOfEnvelopes = 0;
            int numberOfScenarios = 0;
            int numberOfSteps = 0;

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                var enumerator = new MessagesReader(fileStream).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    numberOfEnvelopes++;

                    var envelope = enumerator.Current;
                    if (envelope.GherkinDocument != null)
                    {
                        foreach (var children in envelope.GherkinDocument.Feature.Children)
                        {
                            if (children.Scenario != null)
                            {
                                numberOfScenarios++;

                                foreach (var step in children.Scenario.Steps)
                                {
                                    numberOfSteps++;
                                }
                            }
                        }
                    }
                }
            }

            Assert.That(numberOfEnvelopes, Is.EqualTo(80));
            Assert.That(numberOfScenarios, Is.EqualTo(2));
            Assert.That(numberOfSteps, Is.EqualTo(6));
        }
    }
}