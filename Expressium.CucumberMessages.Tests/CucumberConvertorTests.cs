using Expressium.LivingDoc;
using System.IO;

namespace Expressium.CucumberMessages.Tests
{
    internal class CucumberConvertorTests
    {
        [Test]
        public void Converting_Example_Tables_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.json");

            var livingDocProject = CucumberConvertor.ConvertToLivingDoc(inputFilePath);
            LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
        }

        [Test]
        public void Converting_Rules_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "rules.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "rules.feature.json");

            var livingDocProject = CucumberConvertor.ConvertToLivingDoc(inputFilePath);
            LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
        }
    }
}
