using Expressium.LivingDoc.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    /// <summary>
    /// Documents how LivingDocConverter reports errors to its callers:
    /// - IOException derived errors are re-thrown as a plain IOException with an "IO error: " prefix (inner exception is not kept).
    /// - All other errors are wrapped in an ApplicationException with an "Unexpected error: " prefix and the original as InnerException.
    /// </summary>
    internal class LivingDocConverterErrorHandlingTests
    {
        private string tempDirectory;
        private string minimalFilePath;

        [SetUp]
        public void SetUp()
        {
            tempDirectory = Path.Combine(Path.GetTempPath(), "LivingDocErrorTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);

            minimalFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "minimal", "minimal.ndjson");
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }

        private string CreateFile(string fileName, string content)
        {
            var filePath = Path.Combine(tempDirectory, fileName);
            File.WriteAllText(filePath, content);
            return filePath;
        }

        private string CreateMinimalWithout(string fileName, string envelopeType)
        {
            var lines = File.ReadAllLines(minimalFilePath)
                .Where(line => !line.StartsWith("{\"" + envelopeType + "\""));

            var filePath = Path.Combine(tempDirectory, fileName);
            File.WriteAllLines(filePath, lines);
            return filePath;
        }

        // Import...

        [Test]
        public void LivingDocConverter_Import_MissingFile_ThrowsIOException_WithoutInnerException()
        {
            var inputFilePath = Path.Combine(tempDirectory, "missing.json");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<IOException>(() => livingDocConverter.Import(inputFilePath));
            Assert.That(exception.GetType(), Is.EqualTo(typeof(IOException)));
            Assert.That(exception.Message, Does.StartWith("IO error: "));
            Assert.That(exception.Message, Does.Contain("missing.json"));
            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public void LivingDocConverter_Import_MalformedJson_ThrowsApplicationException()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.json");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Import(inputFilePath));
            Assert.That(exception.Message, Does.StartWith("Unexpected error: "));
            Assert.That(exception.InnerException, Is.InstanceOf<JsonException>());
        }

        [Test]
        public void LivingDocConverter_Import_EmptyFile_ThrowsApplicationException()
        {
            var inputFilePath = CreateFile("empty.json", string.Empty);

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Import(inputFilePath));
            Assert.That(exception.Message, Does.StartWith("Unexpected error: "));
            Assert.That(exception.InnerException, Is.InstanceOf<JsonException>());
        }

        [Test]
        public void LivingDocConverter_Import_JsonNullLiteral_ReturnsNull()
        {
            // A file containing only 'null' is accepted and returns no project...
            var inputFilePath = CreateFile("null.json", "null");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);

            Assert.That(livingDocProject, Is.Null);
        }

        [Test]
        public void LivingDocConverter_Import_CucumberMessagesFile_ThrowsApplicationException()
        {
            // Passing an NDJSON file to Import (instead of Convert) is reported as a JSON error...
            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Import(minimalFilePath));
            Assert.That(exception.InnerException, Is.InstanceOf<JsonException>());
        }

        // Convert...

        [Test]
        public void LivingDocConverter_Convert_EmptyFile_ThrowsApplicationException_NoMeta()
        {
            var inputFilePath = CreateFile("empty.ndjson", string.Empty);

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Convert(inputFilePath, "Title"));
            Assert.That(exception.Message, Is.EqualTo("Unexpected error: No Meta message found in the Cucumber messages file. The file may be empty or malformed."));
            Assert.That(exception.InnerException, Is.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void LivingDocConverter_Convert_MissingTestRunStarted_ThrowsApplicationException()
        {
            var inputFilePath = CreateMinimalWithout("no-run-started.ndjson", "testRunStarted");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Convert(inputFilePath, "Title"));
            Assert.That(exception.Message, Is.EqualTo("Unexpected error: No TestRunStarted message found. The test run may not have started or the file is incomplete."));
            Assert.That(exception.InnerException, Is.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void LivingDocConverter_Convert_MissingTestRunFinished_ThrowsApplicationException()
        {
            var inputFilePath = CreateMinimalWithout("no-run-finished.ndjson", "testRunFinished");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Convert(inputFilePath, "Title"));
            Assert.That(exception.Message, Is.EqualTo("Unexpected error: No TestRunFinished message found. The test run may have been interrupted before completing."));
            Assert.That(exception.InnerException, Is.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void LivingDocConverter_Convert_InvalidGherkinDocument_ThrowsApplicationException()
        {
            // invalid.feature.ndjson contains a gherkinDocument with 'features' instead of 'feature'.
            // The parser does not validate this and fails with a generic NullReferenceException message...
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.feature.ndjson");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Convert(inputFilePath, "Title"));
            Assert.That(exception.Message, Is.EqualTo("Unexpected error: Object reference not set to an instance of an object."));
            Assert.That(exception.InnerException, Is.InstanceOf<NullReferenceException>());
        }

        [Test]
        public void LivingDocConverter_Convert_InterruptedTestRun_ThrowsApplicationException()
        {
            // A run with meta and gherkin documents only - the test run never started...
            var lines = File.ReadAllLines(minimalFilePath)
                .Where(line => line.StartsWith("{\"meta\"") || line.StartsWith("{\"source\"") || line.StartsWith("{\"gherkinDocument\"") || line.StartsWith("{\"pickle\""));
            var inputFilePath = Path.Combine(tempDirectory, "interrupted.ndjson");
            File.WriteAllLines(inputFilePath, lines);

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Convert(inputFilePath, "Title"));
            Assert.That(exception.Message, Does.StartWith("Unexpected error: No TestRunStarted message found."));
        }

        [Test]
        public void LivingDocConverter_Convert_MalformedLine_ThrowsApplicationException()
        {
            var lines = File.ReadAllLines(minimalFilePath).ToList();
            lines.Insert(1, "{ this is not json");
            var inputFilePath = Path.Combine(tempDirectory, "malformed.ndjson");
            File.WriteAllLines(inputFilePath, lines);

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Convert(inputFilePath, "Title"));
            Assert.That(exception.Message, Does.StartWith("Unexpected error: "));
            Assert.That(exception.InnerException, Is.Not.Null);
        }

        [Test]
        public void LivingDocConverter_Convert_NativeJsonFile_ThrowsApplicationException()
        {
            // Passing a native LivingDoc JSON file to Convert (instead of Import)...
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Convert(inputFilePath, "Title"));
            Assert.That(exception.Message, Does.StartWith("Unexpected error: "));
        }

        // MergeProject...

        [Test]
        public void LivingDocConverter_MergeProject_MissingFile_ThrowsIOException()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(minimalFilePath, "Title");

            var exception = Assert.Throws<IOException>(() => livingDocConverter.MergeProject(livingDocProject, Path.Combine(tempDirectory, "missing.ndjson")));
            Assert.That(exception.Message, Does.StartWith("IO error: "));
        }

        [Test]
        public void LivingDocConverter_MergeProject_MalformedFile_ThrowsApplicationException_AndLeavesMasterUnchanged()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(minimalFilePath, "Title");
            var numberOfFeatures = livingDocProject.GetNumberOfFeatures();
            var duration = livingDocProject.Duration;

            var slaveFilePath = CreateFile("empty.ndjson", string.Empty);

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.MergeProject(livingDocProject, slaveFilePath));
            Assert.That(exception.Message, Does.Contain("No Meta message found"));

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(numberOfFeatures));
            Assert.That(livingDocProject.Duration, Is.EqualTo(duration));
        }

        [Test]
        public void LivingDocConverter_MergeProject_NullProject_ThrowsApplicationException()
        {
            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.MergeProject(null, minimalFilePath));
            Assert.That(exception.InnerException, Is.InstanceOf<NullReferenceException>());
        }

        // MergeHistory...

        [Test]
        public void LivingDocConverter_MergeHistory_NoMatchingFiles_AddsNoHistory()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(minimalFilePath, "Title");

            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "nothing.ndjson"));

            Assert.That(livingDocProject.History.Features, Is.Empty);
            Assert.That(livingDocProject.History.Scenarios, Is.Empty);
            Assert.That(livingDocProject.History.Steps, Is.Empty);
            Assert.That(livingDocProject.HasHistory(), Is.False);
        }

        [Test]
        public void LivingDocConverter_MergeHistory_MissingDirectory_ThrowsDirectoryNotFoundException()
        {
            // MergeHistory has no error wrapping - the raw exception reaches the caller...
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(minimalFilePath, "Title");

            var historyPath = Path.Combine(tempDirectory, "NonExistent", "history.ndjson");

            Assert.Throws<DirectoryNotFoundException>(() => livingDocConverter.MergeHistory(livingDocProject, historyPath));
        }

        [Test]
        public void LivingDocConverter_MergeHistory_MalformedHistoryFile_ThrowsApplicationException()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(minimalFilePath, "Title");

            CreateFile("history1.ndjson", string.Empty);

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "history.ndjson")));
            Assert.That(exception.Message, Does.Contain("No Meta message found"));
        }

        // Generate / Export...

        [Test]
        public void LivingDocConverter_Generate_NullProject_ThrowsApplicationException()
        {
            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverter.Generate(null, Path.Combine(tempDirectory, "report.html")));
            Assert.That(exception.Message, Does.StartWith("Unexpected error: "));
            Assert.That(exception.InnerException, Is.InstanceOf<NullReferenceException>());
            Assert.That(File.Exists(Path.Combine(tempDirectory, "report.html")), Is.False);
        }

        [Test]
        public void LivingDocConverter_Generate_EmptyProject_ProducesReport()
        {
            var outputFilePath = Path.Combine(tempDirectory, "empty.html");

            var livingDocConverter = new LivingDocConverter();
            livingDocConverter.Generate(new LivingDocProject { Title = "Empty Project" }, outputFilePath);

            Assert.That(File.Exists(outputFilePath));
            Assert.That(File.ReadAllText(outputFilePath), Does.Contain("Empty Project"));
        }

        [Test]
        public void LivingDocConverter_Export_NullProject_WritesNullLiteral()
        {
            var outputFilePath = Path.Combine(tempDirectory, "null-export.json");

            var livingDocConverter = new LivingDocConverter();
            livingDocConverter.Export(null, outputFilePath);

            Assert.That(File.ReadAllText(outputFilePath), Is.EqualTo("null"));
        }
    }
}
