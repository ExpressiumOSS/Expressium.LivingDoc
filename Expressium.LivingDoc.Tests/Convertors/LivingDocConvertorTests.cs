using System;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Convertors
{
    public class LivingDocConvertorTests
    {
        [Test]
        public void LivingDocConvertor_Execute()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            File.Delete(outputFilePath);

            var livingDocConvertor = new LivingDocConvertor(inputFilePath, outputFilePath, "MyProjectTitle");
            livingDocConvertor.Execute();

            Assert.That(File.Exists(outputFilePath));
        }

        [Test]
        public void LivingDocConvertor_Execute_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            var livingDocConvertor = new LivingDocConvertor(inputFilePath, outputFilePath, "MyProjectTitle");

            var exception = Assert.Throws<IOException>(() => livingDocConvertor.Execute());
            Assert.That(exception.Message.StartsWith("IO error: Could not find file"));
        }

        [Test]
        public void LivingDocConvertor_Execute_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine($"G:\\Output\\dummy.html");

            var livingDocConvertor = new LivingDocConvertor(inputFilePath, outputFilePath, "MyProjectTitle");

            var exception = Assert.Throws<IOException>(() => livingDocConvertor.Execute());
            Assert.That(exception.Message.StartsWith("IO error: Could not find a part of the path"));
        }

        [Test]
        public void LivingDocConvertor_GenerateDocument_Invalid_Input_File()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.html");

            var livingDocConvertor = new LivingDocConvertor(inputFilePath, outputFilePath, "MyProjectTitle");

            var exception = Assert.Throws<ApplicationException>(() => livingDocConvertor.Execute());
            Assert.That(exception.Message.StartsWith("Unexpected error: Object reference not set to an instance of an object."));
        }
    }
}
