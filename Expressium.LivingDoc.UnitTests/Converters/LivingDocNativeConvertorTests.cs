using System;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    public class LivingDocNativeConvertersTests
    {
        [Test]
        public void LivingDocNativeConverters_Execute()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            File.Delete(outputFilePath);

            var livingDocNativeConverters = new LivingDocNativeConverter(inputFilePath, outputFilePath);
            livingDocNativeConverters.Execute();

            Assert.That(File.Exists(outputFilePath));
        }

        [Test]
        public void LivingDocNativeConverters_Execute_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            var livingDocNativeConverters = new LivingDocNativeConverter(inputFilePath, outputFilePath);

            var exception = Assert.Throws<IOException>(() => livingDocNativeConverters.Execute());
            Assert.That(exception.Message.StartsWith("IO error: Could not find file"));
        }

        [Test]
        public void LivingDocNativeConverters_Execute_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");
            var outputFilePath = Path.Combine($"G:\\Output\\dummy.html");

            var livingDocNativeConverters = new LivingDocNativeConverter(inputFilePath, outputFilePath);

            var exception = Assert.Throws<IOException>(() => livingDocNativeConverters.Execute());
            Assert.That(exception.Message.StartsWith("IO error: Could not find a part of the path"));
        }

        [Test]
        public void LivingDocNativeConverters_Execute_Invalid_Input_File()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.html");

            var livingDocNativeConverters = new LivingDocNativeConverter(inputFilePath, outputFilePath);

            var exception = Assert.Throws<ApplicationException>(() => livingDocNativeConverters.Execute());
            Assert.That(exception.Message.StartsWith("Unexpected error: The JSON value could not be converted"));
        }
    }
}
