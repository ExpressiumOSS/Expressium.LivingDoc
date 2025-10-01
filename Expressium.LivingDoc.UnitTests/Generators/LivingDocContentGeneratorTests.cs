using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocContentGeneratorTests
    {
        [Test]
        public void LivingDocContentGenerator_Generate()
        {
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

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateContent();

            Assert.That(listOfLines.Count, Is.GreaterThan(50));
        }
    }
}
