using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocContentGeneratorTests
    {
        [Test]
        public void LivingDocContentGenerator_GenerateHeader()
        {
            var project = new LivingDocProject
            {
                Title = "Test Project",
                Date = new System.DateTime(2024, 6, 1, 13, 5, 37)
            };

            var configuration = new LivingDocConfiguration();
            var generator = new LivingDocContentGenerator(project, configuration);

            var listOfLines = generator.GenerateHeader();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Header Section -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<header>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='project-name'>Test Project</span><br />"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='project-date'>Generated Sat 1. Jun 2024 13:05:37 GMT+2</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</header>"));
        }

        [Test]
        public void LivingDocContentGenerator_GenerateNavigation()
        {
            var project = new LivingDocProject();
            var configuration = new LivingDocConfiguration();

            var generator = new LivingDocContentGenerator(project, configuration);

            var listOfLines = generator.GenerateNavigation();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(14));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Project Navigation Section -->"));
            Assert.That(listOfLines[3], Does.Contain("title='Overview'"));
            Assert.That(listOfLines[5], Does.Contain("title='Features List View'"));
            Assert.That(listOfLines[7], Does.Contain("title='Scenarios List View'"));
            Assert.That(listOfLines[9], Does.Contain("title='Steps List View'"));
            Assert.That(listOfLines[11], Does.Contain("title='Analytics'"));
        }

        [Test]
        public void LivingDocContentGenerator_GenerateNavigation_Configured()
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

            var generator = new LivingDocContentGenerator(project, configuration);

            var listOfLines = generator.GenerateNavigation();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Project Navigation Section -->"));
            Assert.That(listOfLines[3], Does.Contain("title='Overview'"));
            Assert.That(listOfLines[5], Does.Contain("title='Analytics'"));
        }

        [Test]
        public void LivingDocContentGenerator_GenerateFooter()
        {
            var project = new LivingDocProject();
            var configuration = new LivingDocConfiguration();

            var generator = new LivingDocContentGenerator(project, configuration);

            var listOfLines = generator.GenerateFooter();

            var year = System.DateTime.Now.ToString("yyyy");

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Footer Section -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<footer>"));
            Assert.That(listOfLines[2], Does.Contain($"{year}"));
            Assert.That(listOfLines[3], Is.EqualTo("</footer>"));
        }
    }
}
