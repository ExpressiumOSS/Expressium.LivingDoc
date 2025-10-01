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

            var configuration = new LivingDocConfiguration();
            var generator = new LivingDocDataGenerator(project, configuration);

            var listOfLines = generator.GenerateDataListViews();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(54));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataListViews_Configured()
        {
            var project = new LivingDocProject();

            var configuration = new LivingDocConfiguration()
            {
                Overview = false,
                FeaturesListView = true,
                ScenariosListView = true,
                StepsListView = false,
                EditorView = false
            };

            var generator = new LivingDocDataGenerator(project, configuration);

            var listOfLines = generator.GenerateDataListViews();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(37));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataAnalytics()
        {
            var project = new LivingDocProject();

            var configuration = new LivingDocConfiguration();
            var generator = new LivingDocDataGenerator(project, configuration);

            var listOfLines = generator.GenerateDataAnalytics();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(219));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataAnalytics_Configured()
        {
            var project = new LivingDocProject();

            var configuration = new LivingDocConfiguration()
            {
                Overview = false,
                FeaturesListView = false,
                ScenariosListView = false,
                StepsListView = false,
                EditorView = false
            };

            var generator = new LivingDocDataGenerator(project, configuration);

            var listOfLines = generator.GenerateDataAnalytics();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataEditor()
        {
            var project = new LivingDocProject();

            var configuration = new LivingDocConfiguration();
            var generator = new LivingDocDataGenerator(project, configuration);

            var listOfLines = generator.GenerateDataEditor();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataEditor_Configured()
        {
            var project = new LivingDocProject();

            var configuration = new LivingDocConfiguration()
            {
                EditorView = true
            };

            var generator = new LivingDocDataGenerator(project, configuration);

            var listOfLines = generator.GenerateDataEditor();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(29));
        }
    }
}
