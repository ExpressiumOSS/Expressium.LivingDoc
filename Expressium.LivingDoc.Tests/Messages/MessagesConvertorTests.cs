using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorTests
    {
        [Test]
        public void Converting_Example_Tables_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.json");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(6));
        }

        [Test]
        public void Converting_Rules_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "rules.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "rules.feature.json");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(12));
        }

        [Test]
        public void Converting_Data_Table_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "data-tables.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "data-tables.feature.json");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));
        }

        [Test]
        public void Converting_Minimal_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.json");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));
        }

        //[Test]
        //public void Converting_Skipped_Feature()
        //{
        //    var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "skipped.feature.ndjson");
        //    var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "skipped.feature.json");

        //    var livingDocProject = CucumberConvertor.ConvertToLivingDoc(inputFilePath);
        //    LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

        //    Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
        //    Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
        //    Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));
        //}
    }
}
