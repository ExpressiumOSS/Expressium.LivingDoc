using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocBodyGeneratorTests
    {
        [Test]
        public void LivingDocGenerator_GenerateDocument()
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
            var listOfLines = bodyGenerator.GenerateBody(project);

            Assert.That(listOfLines.Count, Is.EqualTo(420));
        }
    }
}
