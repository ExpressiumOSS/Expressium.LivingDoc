using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataAnalyticsGeneratorPropertiesTests
    {
        private static LivingDocProject CreateProjectWithProperties(params (string Name, string Value)[] properties)
        {
            var project = new LivingDocProject();
            project.Properties.properties = properties
                .Select(p => new LivingDocProperties.Property { name = p.Name, value = p.Value })
                .ToArray();

            return project;
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsProperties_ReturnsEmpty_WhenPropertiesIsNull()
        {
            var project = new LivingDocProject();
            project.Properties = null;

            var generator = new LivingDocDataAnalyticsGenerator(project);

            Assert.That(generator.GenerateDataAnalyticsProperties(), Is.Empty);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsProperties_ReturnsEmpty_WhenPropertyListIsNull()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());

            Assert.That(generator.GenerateDataAnalyticsProperties(), Is.Empty);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsProperties_ReturnsEmpty_WhenPropertyListIsEmpty()
        {
            var generator = new LivingDocDataAnalyticsGenerator(CreateProjectWithProperties());

            Assert.That(generator.GenerateDataAnalyticsProperties(), Is.Empty);
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsProperties_RendersTable()
        {
            var project = CreateProjectWithProperties(("Environment", "Staging"), ("Browser", "Chrome"));

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsProperties();

            Assert.That(listOfLines, Is.EqualTo(new[]
            {
                "<hr>",
                "<!-- Data Analytics Properties -->",
                "<div class='section analytics-properties'>",
                "<span class='chart-name'>Properties</span>",
                "<div class='section'>",
                "<table class='analytics-list'>",
                "<thead>",
                "<tr>",
                "<th>Name</th>",
                "<th>Value</th>",
                "</tr>",
                "</thead>",
                "<tbody>",
                "<tr><td>Environment</td><td>Staging</td></tr>",
                "<tr><td>Browser</td><td>Chrome</td></tr>",
                "</tbody>",
                "</table>",
                "</div>",
                "</div>"
            }));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsProperties_RendersOneRowPerProperty_InOrder()
        {
            var project = CreateProjectWithProperties(("C", "3"), ("A", "1"), ("B", "2"));

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var rows = generator.GenerateDataAnalyticsProperties().Where(l => l.StartsWith("<tr><td>")).ToList();

            Assert.That(rows, Is.EqualTo(new[]
            {
                "<tr><td>C</td><td>3</td></tr>",
                "<tr><td>A</td><td>1</td></tr>",
                "<tr><td>B</td><td>2</td></tr>"
            }));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsProperties_RendersEmptyCells_ForNullNameOrValue()
        {
            var project = CreateProjectWithProperties(("Environment", null), (null, "Orphan"));

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsProperties();

            Assert.That(listOfLines, Does.Contain("<tr><td>Environment</td><td></td></tr>"));
            Assert.That(listOfLines, Does.Contain("<tr><td></td><td>Orphan</td></tr>"));
        }

        [Test]
        [Ignore("Known issue: property names and values are not HTML encoded")]
        public void LivingDocDataAnalyticsGenerator_GenerateDataAnalyticsProperties_HtmlEncodesNameAndValue()
        {
            var project = CreateProjectWithProperties(("<Branch>", "feature/a&b"));

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsProperties();

            Assert.That(listOfLines, Does.Contain("<tr><td>&lt;Branch&gt;</td><td>feature/a&amp;b</td></tr>"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_AllViews_IncludeProperties_WhenPresent()
        {
            var project = CreateProjectWithProperties(("Environment", "Staging"));

            var generator = new LivingDocDataAnalyticsGenerator(project);

            Assert.That(generator.GenerateDataAnalyticsFeaturesView(), Does.Contain("<!-- Data Analytics Properties -->"));
            Assert.That(generator.GenerateDataAnalyticsScenariosView(), Does.Contain("<!-- Data Analytics Properties -->"));
            Assert.That(generator.GenerateDataAnalyticsStepsView(), Does.Contain("<!-- Data Analytics Properties -->"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_AllViews_ExcludeProperties_WhenAbsent()
        {
            var generator = new LivingDocDataAnalyticsGenerator(new LivingDocProject());

            Assert.That(generator.GenerateDataAnalyticsFeaturesView(), Does.Not.Contain("<!-- Data Analytics Properties -->"));
            Assert.That(generator.GenerateDataAnalyticsScenariosView(), Does.Not.Contain("<!-- Data Analytics Properties -->"));
            Assert.That(generator.GenerateDataAnalyticsStepsView(), Does.Not.Contain("<!-- Data Analytics Properties -->"));
        }

        [Test]
        public void LivingDocDataAnalyticsGenerator_FeaturesView_PropertiesAreLastSection()
        {
            var project = CreateProjectWithProperties(("Environment", "Staging"));

            var generator = new LivingDocDataAnalyticsGenerator(project);
            var listOfLines = generator.GenerateDataAnalyticsFeaturesView();

            var propertiesIndex = listOfLines.IndexOf("<!-- Data Analytics Properties -->");
            var durationIndex = listOfLines.IndexOf("<!-- Data Analytics Duration -->");

            Assert.That(propertiesIndex, Is.GreaterThan(durationIndex));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }
    }
}
