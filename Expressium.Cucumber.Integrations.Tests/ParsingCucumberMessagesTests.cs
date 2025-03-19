using Expressium.LivingDoc;
using System.IO;

namespace Expressium.Cucumber.Integrations.Tests
{
    public class ParsingCucumberMessagesTests
    {
        [Test]
        public void ReadingCucumberMessagesNdjsonFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");

            int numberOfEnvelopes = 0;
            int numberOfScenarios = 0;
            int numberOfSteps = 0;

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                var enumerator = new NdjsonMessageReaderSUT(fileStream).GetEnumerator();
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

            Assert.That(numberOfEnvelopes, Is.EqualTo(100));
            Assert.That(numberOfScenarios, Is.EqualTo(2));
            Assert.That(numberOfSteps, Is.EqualTo(6));
        }

        [Test]
        public void ConvertingCucumberMessagesNdjsonFile()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.json");

            var livingDocProject = CucumberConvertor.ConvertToLivingDoc(inputFilePath);
            LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
        }
    }
}