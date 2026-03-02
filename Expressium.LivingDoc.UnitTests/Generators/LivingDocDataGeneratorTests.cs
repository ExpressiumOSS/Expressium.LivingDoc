using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataGeneratorTests
    {
        [Test]
        public void LivingDocDataGenerator_GenerateDataListViews()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);

            var listOfLines = generator.GenerateDataListViews();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(54));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataAnalytics()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);

            var listOfLines = generator.GenerateDataAnalytics();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(219));
        }
    }
}
