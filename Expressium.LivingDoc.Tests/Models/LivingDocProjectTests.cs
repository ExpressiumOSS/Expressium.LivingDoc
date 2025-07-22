using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocProjectTests
    {
        [Test]
        public void LivingDocProject_Number_Of_Objects()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocProject = LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(6));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(17));

            Assert.That(livingDocProject.GetNumberOfFailedFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteFeatures(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfPassedFeatures(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfSkippedFeatures(), Is.EqualTo(0));

            Assert.That(livingDocProject.GetNumberOfFailedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfPassedScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSkippedScenarios(), Is.EqualTo(1));

            Assert.That(livingDocProject.GetNumberOfFailedSteps(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteSteps(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfPassedSteps(), Is.EqualTo(12));
            Assert.That(livingDocProject.GetNumberOfSkippedSteps(), Is.EqualTo(3));

            Assert.That(livingDocProject.GetDate(), Is.Not.Null);

            Assert.That(livingDocProject.GetDuration(), Is.EqualTo("57s 203ms"));
        }
    }
}
