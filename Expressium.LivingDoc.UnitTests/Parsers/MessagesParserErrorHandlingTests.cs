using Expressium.LivingDoc.Parsers;
using System;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    /// <summary>
    /// Documents the raw exceptions raised by the parser before LivingDocConverter wraps them.
    /// </summary>
    internal class MessagesParserErrorHandlingTests
    {
        private string tempDirectory;
        private string minimalFilePath;

        [SetUp]
        public void SetUp()
        {
            tempDirectory = Path.Combine(Path.GetTempPath(), "MessagesParserErrorTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);

            minimalFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "minimal", "minimal.ndjson");
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }

        private string CreateMinimalWithout(params string[] envelopeTypes)
        {
            var lines = File.ReadAllLines(minimalFilePath)
                .Where(line => !envelopeTypes.Any(type => line.StartsWith("{\"" + type + "\"")));

            var filePath = Path.Combine(tempDirectory, Guid.NewGuid().ToString("N") + ".ndjson");
            File.WriteAllLines(filePath, lines);
            return filePath;
        }

        [Test]
        public void MessagesParser_MissingFile_ThrowsFileNotFoundException()
        {
            var messagesParser = new MessagesParser();

            Assert.Throws<FileNotFoundException>(() => messagesParser.ConvertToLivingDoc(Path.Combine(tempDirectory, "missing.ndjson")));
        }

        [Test]
        public void MessagesParser_EmptyFile_ThrowsInvalidOperationException_NoMeta()
        {
            var inputFilePath = Path.Combine(tempDirectory, "empty.ndjson");
            File.WriteAllText(inputFilePath, string.Empty);

            var messagesParser = new MessagesParser();

            var exception = Assert.Throws<InvalidOperationException>(() => messagesParser.ConvertToLivingDoc(inputFilePath));
            Assert.That(exception.Message, Is.EqualTo("No Meta message found in the Cucumber messages file. The file may be empty or malformed."));
        }

        [Test]
        public void MessagesParser_MissingMeta_ThrowsInvalidOperationException()
        {
            var inputFilePath = CreateMinimalWithout("meta");

            var messagesParser = new MessagesParser();

            var exception = Assert.Throws<InvalidOperationException>(() => messagesParser.ConvertToLivingDoc(inputFilePath));
            Assert.That(exception.Message, Does.StartWith("No Meta message found"));
        }

        [Test]
        public void MessagesParser_MissingTestRunStarted_ThrowsInvalidOperationException()
        {
            var inputFilePath = CreateMinimalWithout("testRunStarted");

            var messagesParser = new MessagesParser();

            var exception = Assert.Throws<InvalidOperationException>(() => messagesParser.ConvertToLivingDoc(inputFilePath));
            Assert.That(exception.Message, Is.EqualTo("No TestRunStarted message found. The test run may not have started or the file is incomplete."));
        }

        [Test]
        public void MessagesParser_MissingTestRunFinished_ThrowsInvalidOperationException()
        {
            var inputFilePath = CreateMinimalWithout("testRunFinished");

            var messagesParser = new MessagesParser();

            var exception = Assert.Throws<InvalidOperationException>(() => messagesParser.ConvertToLivingDoc(inputFilePath));
            Assert.That(exception.Message, Is.EqualTo("No TestRunFinished message found. The test run may have been interrupted before completing."));
        }

        [Test]
        public void MessagesParser_OnlyGherkinDocuments_ThrowsInvalidOperationException()
        {
            // A run that never started executing tests...
            var inputFilePath = CreateMinimalWithout("testRunStarted", "testCase", "testCaseStarted", "testStepStarted", "testStepFinished", "testCaseFinished", "testRunFinished");

            var messagesParser = new MessagesParser();

            Assert.Throws<InvalidOperationException>(() => messagesParser.ConvertToLivingDoc(inputFilePath));
        }

        [Test]
        public void MessagesParser_MissingTestCaseFinished_StillConverts_WithZeroDuration()
        {
            // An interrupted scenario: steps are reported, but the scenario never finished...
            var inputFilePath = CreateMinimalWithout("testCaseFinished");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var example = livingDocProject.Features[0].Scenarios[0].Examples[0];
            Assert.That(example.Steps[0].Status, Is.EqualTo("Passed"));
            Assert.That(example.Duration, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void MessagesParser_MissingTestStepFinished_StepIsSkipped()
        {
            var inputFilePath = CreateMinimalWithout("testStepFinished");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.Null);
            Assert.That(scenario.GetStatus(), Is.EqualTo("Skipped"));
        }

        [Test]
        public void MessagesParser_MalformedLine_Throws()
        {
            var lines = File.ReadAllLines(minimalFilePath).ToList();
            lines.Insert(1, "{ this is not json");
            var inputFilePath = Path.Combine(tempDirectory, "malformed.ndjson");
            File.WriteAllLines(inputFilePath, lines);

            var messagesParser = new MessagesParser();

            Assert.That(() => messagesParser.ConvertToLivingDoc(inputFilePath), Throws.Exception);
        }
    }
}
