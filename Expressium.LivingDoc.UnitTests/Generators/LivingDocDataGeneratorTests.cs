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
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Features View -->"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataAnalytics()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);

            var listOfLines = generator.GenerateDataAnalytics();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(219));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Duration -->"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataOverview()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);
            var listOfLines = generator.GenerateDataOverview();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines, Does.Contain("<!-- Data Overview -->"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataObjects()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);
            var listOfLines = generator.GenerateDataObjects();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(0));
        }
    }
}
