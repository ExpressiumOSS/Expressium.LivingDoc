using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    public class LivingDocConvertorMergeTests
    {
        [Test]
        public void LivingDocConverters_Merge_Parallel_Test_Runs()
        {
            var inputFilePathOne = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsOne.ndjson");
            var inputFilePathTwo = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsTwo.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "ParallelTestRuns.html");

            File.Delete(outputFilePath);

            var messagesParserMaster = new MessagesParser();
            var livingDocProjectOne = messagesParserMaster.ConvertToLivingDoc(inputFilePathOne);

            var messagesParserSlave = new MessagesParser();
            var livingDocProjectTwo = messagesParserSlave.ConvertToLivingDoc(inputFilePathTwo);

            livingDocProjectOne.Merge(livingDocProjectTwo);

            var livingDocProjectGenerator = new LivingDocProjectGenerator(livingDocProjectOne);
            livingDocProjectGenerator.Generate(outputFilePath);

            Assert.That(File.Exists(outputFilePath));

            Assert.That(livingDocProjectOne.GetNumberOfFeatures(), Is.EqualTo(2));
            Assert.That(livingDocProjectOne.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProjectOne.GetNumberOfSteps(), Is.EqualTo(7));
            Assert.That(livingDocProjectOne.GetDuration(), Is.EqualTo("9s 417ms"));
        }
    }
}
