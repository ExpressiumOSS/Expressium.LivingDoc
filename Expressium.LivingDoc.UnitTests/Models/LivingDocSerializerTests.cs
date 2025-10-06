using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocSerializerTests
    {
        [Test]
        public void LivingDoc_Project_DeserializeAsJson()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(6));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(17));
        }

        [Test]
        public void LivingDoc_Project_SerializeAsJson()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native-output.json");

            File.Delete(outputFilePath);

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);
            LivingDocSerializer.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(File.Exists(outputFilePath));
        }
    }
}
