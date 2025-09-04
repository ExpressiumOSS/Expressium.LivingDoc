using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocBodyGeneratorTests
    {
        [Test]
        public void LivingDocBodyGenerator_Generate()
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

            var bodyGenerator = new LivingDocBodyGenerator();
            var listOfLines = bodyGenerator.Generate(project);

            Assert.That(listOfLines.Count, Is.GreaterThan(400));
        }
    }
}
