using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc.UnitTests
{
    [TestFixture]
    public class LivingDocGeneratorTests
    {
        private string tempFilePath;

        [SetUp]
        public void SetUp()
        {
            tempFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        [Test]
        public void AssignUniqueIdentifier_AssignsIdsAndIndexesCorrectly()
        {
            // Arrange
            var project = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature
                    {
                        Scenarios = new List<LivingDocScenario>
                        {
                            new LivingDocScenario(),
                            new LivingDocScenario()
                        }
                    },
                    new LivingDocFeature
                    {
                        Scenarios = new List<LivingDocScenario>
                        {
                            new LivingDocScenario()
                        }
                    }
                }
            };

            var generator = new LivingDocGenerator("dummyInput", "dummyOutput");

            // Act
            generator.AssignUniqueIdentifier(project);

            // Assert
            int expectedIndex = 1;
            foreach (var feature in project.Features)
            {
                Assert.That(string.IsNullOrWhiteSpace(feature.Id), Is.False, "Feature ID should not be null or empty.");

                foreach (var scenario in feature.Scenarios)
                {
                    Assert.That(string.IsNullOrWhiteSpace(scenario.Id), Is.False, "Scenario ID should not be null or empty.");
                    Assert.That(expectedIndex++, Is.EqualTo(scenario.Index), "Scenario index mismatch.");
                }
            }
        }

        [Test]
        public void GenerateHtmlReport_GeneratesNonEmptyHtmlFile()
        {
            // Arrange
            var project = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature
                    {
                        Scenarios = new List<LivingDocScenario>
                        {
                            new LivingDocScenario { Name = "Sample Scenario" }
                        }
                    }
                }
            };

            var generator = new LivingDocGenerator("dummyInput", tempFilePath);

            // Ensure output path is writable
            Assert.That(Path.IsPathRooted(tempFilePath), Is.True, "Temp file path must be absolute.");

            // Act
            generator.GenerateHtmlReport(project);

            // Assert
            Assert.That(File.Exists(tempFilePath), Is.True, "HTML file was not created.");
            var content = File.ReadAllText(tempFilePath);
            Assert.That(content, Is.Not.Empty, "HTML file is empty.");
            Assert.That(content.Contains("<html>"), Is.True, "HTML content should include root <html> tag.");
        }
    }
}
