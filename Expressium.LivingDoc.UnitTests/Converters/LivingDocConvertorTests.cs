using System;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    public class LivingDocConvertersTests
    {
        [Test]
        public void LivingDocConverter_Generate()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            File.Delete(outputFilePath);

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            Assert.That(File.Exists(outputFilePath));
        }

        [Test]
        public void LivingDocConverter_Convert_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.feature.ndjson");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<IOException>(() => livingDocConverter.Convert(inputFilePath, "MyProjectTitle"));
            Assert.That(exception.Message.StartsWith("IO error: Could not find file"));
        }

        [Test]
        public void LivingDocConverter_Generate_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine($"G:\\Output\\dummy.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");

            var exception = Assert.Throws<IOException>(() => livingDocConverter.Generate(livingDocProject, outputFilePath));
            Assert.That(exception.Message.StartsWith("IO error: Could not find a part of the path"));
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

        [Test]
        public void LivingDocConverters_Import()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            Assert.That(livingDocProject.Features.Count, Is.EqualTo(4));
            Assert.That(livingDocProject.Features[0].Scenarios.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples.Count, Is.EqualTo(1));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps.Count, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocConverters_Export()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "export.json");

            File.Delete(outputFilePath);

            var livingDocConverters = new LivingDocConverter();
            var livingDocProject = livingDocConverters.Import(inputFilePath);
            livingDocConverters.Export(livingDocProject, outputFilePath);

            Assert.That(File.Exists(outputFilePath));
        }
    }
}
