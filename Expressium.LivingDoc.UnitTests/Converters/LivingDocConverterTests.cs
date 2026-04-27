using System.IO;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    public class LivingDocConverterTests
    {
        [Test]
        public void LivingDocConverter_Import()
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
        public void LivingDocConverter_Import_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.json");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<IOException>(() => livingDocConverter.Import(inputFilePath));
            Assert.That(exception.Message, Does.StartWith("IO error:"));
        }

        [Test]
        public void LivingDocConverter_Export()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "export.json");

            File.Delete(outputFilePath);

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);
            livingDocConverter.Export(livingDocProject, outputFilePath);

            Assert.That(File.Exists(outputFilePath));

            var reloadedProject = livingDocConverter.Import(outputFilePath);
            Assert.That(reloadedProject.Features.Count, Is.EqualTo(livingDocProject.Features.Count));
        }

        [Test]
        public void LivingDocConverter_Export_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "NonExistent", "Subfolder", "export.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);

            var exception = Assert.Throws<IOException>(() => livingDocConverter.Export(livingDocProject, outputFilePath));
            Assert.That(exception.Message, Does.StartWith("IO error:"));
        }

        [Test]
        public void LivingDocConverter_Convert()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");

            Assert.That(livingDocProject, Is.Not.Null);
            Assert.That(livingDocProject.Title, Is.EqualTo("MyProjectTitle"));
            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
        }

        [Test]
        public void LivingDocConverter_Convert_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.feature.ndjson");

            var livingDocConverter = new LivingDocConverter();

            var exception = Assert.Throws<IOException>(() => livingDocConverter.Convert(inputFilePath, "MyProjectTitle"));
            Assert.That(exception.Message, Does.StartWith("IO error:"));
        }

        [Test]
        public void LivingDocConverter_Convert_NullTitle()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, null);

            Assert.That(livingDocProject, Is.Not.Null);
            Assert.That(livingDocProject.Title, Is.EqualTo("Expressium LivingDoc"));
        }

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

            var htmlFile = File.ReadAllText(outputFilePath);
            Assert.That(htmlFile, Does.Contain(livingDocProject.Title));
        }

        [Test]
        public void LivingDocConverter_Generate_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "NonExistent", "Subfolder", "dummy.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");

            var exception = Assert.Throws<IOException>(() => livingDocConverter.Generate(livingDocProject, outputFilePath));
            Assert.That(exception.Message, Does.StartWith("IO error:"));
        }

        [Test]
        public void LivingDocConverter_MergeProject()
        {
            var inputFilePathOne = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsOne.ndjson");
            var inputFilePathTwo = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsTwo.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePathOne, "MyProjectTitle");
            livingDocConverter.MergeProject(livingDocProject, inputFilePathTwo);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(7));
            Assert.That(livingDocProject.GetDuration(), Is.EqualTo("9s 417ms"));
        }

        [Test]
        public void LivingDocConverter_MergeHistory()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.feature.ndjson");
            var historyPath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "History/coffeeshop*.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "Expressium.Coffeeshop.Web.API.Tests");

            livingDocConverter.MergeHistory(livingDocProject, historyPath);

            var livingDocHistories = livingDocProject.History.Scenarios;
            Assert.That(livingDocHistories.Count, Is.EqualTo(4));
        }
    }
}
