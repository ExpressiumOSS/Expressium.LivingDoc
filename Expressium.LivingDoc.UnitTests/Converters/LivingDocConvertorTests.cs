using System;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    public class LivingDocConvertersTests
    {
        [Test]
        public void LivingDocConverters_Generate()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            File.Delete(outputFilePath);

            var livingDocConverters = new LivingDocConverter();
            livingDocConverters.Generate(inputFilePath, outputFilePath, "MyProjectTitle");

            Assert.That(File.Exists(outputFilePath));
        }

        [Test]
        public void LivingDocConverters_Generate_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            var livingDocConverters = new LivingDocConverter();

            var exception = Assert.Throws<IOException>(() => livingDocConverters.Generate(inputFilePath, outputFilePath, null));
            Assert.That(exception.Message.StartsWith("IO error: Could not find file"));
        }

        [Test]
        public void LivingDocConverters_Generate_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine($"G:\\Output\\dummy.html");

            var livingDocConverters = new LivingDocConverter();

            var exception = Assert.Throws<IOException>(() => livingDocConverters.Generate(inputFilePath, outputFilePath, null));
            Assert.That(exception.Message.StartsWith("IO error: Could not find a part of the path"));
        }

        [Test]
        public void LivingDocConverters_Generate_Invalid_Input_File()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.html");

            var livingDocConverters = new LivingDocConverter();

            var exception = Assert.Throws<ApplicationException>(() => livingDocConverters.Generate(inputFilePath, outputFilePath, null));
            Assert.That(exception.Message.StartsWith("Unexpected error: Object reference not set to an instance of an object."));
        }

        [Test]
        public void LivingDocConverters_Convert_And_Merge_History()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.feature.ndjson");
            var historyFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "History/coffeeshop*.ndjson");

            var livingDocConverters = new LivingDocConverter();
            var livingDocProject = livingDocConverters.Convert(inputFilePath, "Expressium.Coffeeshop.Web.API.Tests");

            livingDocConverters.MergeHistory(livingDocProject, historyFilePath);

            var livingDocHistories = livingDocProject.Histories;
            Assert.That(livingDocHistories.Count, Is.EqualTo(4));
        }
    }
}
