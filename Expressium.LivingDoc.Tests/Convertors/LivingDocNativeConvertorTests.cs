using System;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Convertors
{
    public class LivingDocNativeConvertorTests
    {
        [Test]
        public void LivingDocNativeConvertor_Execute()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            File.Delete(outputFilePath);

            var livingDocNativeConvertor = new LivingDocNativeConvertor(inputFilePath, outputFilePath);
            livingDocNativeConvertor.Execute();

            Assert.That(File.Exists(outputFilePath));
        }

        [Test]
        public void LivingDocNativeConvertor_Execute_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            var livingDocNativeConvertor = new LivingDocNativeConvertor(inputFilePath, outputFilePath);

            var exception = Assert.Throws<IOException>(() => livingDocNativeConvertor.Execute());
            Assert.That(exception.Message.StartsWith("IO error: Could not find file"));
        }

        [Test]
        public void LivingDocNativeConvertor_Execute_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");
            var outputFilePath = Path.Combine($"G:\\Output\\dummy.html");

            var livingDocNativeConvertor = new LivingDocNativeConvertor(inputFilePath, outputFilePath);

            var exception = Assert.Throws<IOException>(() => livingDocNativeConvertor.Execute());
            Assert.That(exception.Message.StartsWith("IO error: Could not find a part of the path"));
        }

        [Test]
        public void LivingDocNativeConvertor_Execute_Invalid_Input_File()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.html");

            var livingDocNativeConvertor = new LivingDocNativeConvertor(inputFilePath, outputFilePath);

            var exception = Assert.Throws<ApplicationException>(() => livingDocNativeConvertor.Execute());
            Assert.That(exception.Message.StartsWith("Unexpected error: The JSON value could not be converted"));
        }
    }
}
