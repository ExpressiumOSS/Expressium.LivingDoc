using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataPropertiesGeneratorTests
    {
        private static LivingDocPropertySection CreateSection(string name, params (string Name, string Value)[] properties)
        {
            var section = new LivingDocPropertySection { Name = name };
            section.Properties = properties
                .Select(p => new LivingDocProperty { Name = p.Name, Value = p.Value })
                .ToList();

            return section;
        }

        private static LivingDocProject CreateProjectWithSections(params LivingDocPropertySection[] sections)
        {
            var project = new LivingDocProject();
            project.Properties.Sections.AddRange(sections);

            return project;
        }

        private static LivingDocProject CreateProjectWithProperties(params (string Name, string Value)[] properties)
        {
            return CreateProjectWithSections(CreateSection("General", properties));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataProjectPropertiesView()
        {
            var project = CreateProjectWithProperties(("Environment", "Staging"));

            var generator = new LivingDocDataPropertiesGenerator(project);
            var listOfLines = generator.GenerateDataProjectPropertiesView();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Properties -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='properties'>"));
            Assert.That(listOfLines, Does.Contain("<span class='page-name' data-testid='page-title'>Properties</span>"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Properties Section -->"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataProjectPropertiesView_TitleBeforeSections()
        {
            var project = CreateProjectWithProperties(("Environment", "Staging"));

            var generator = new LivingDocDataPropertiesGenerator(project);
            var listOfLines = generator.GenerateDataProjectPropertiesView();

            var titleIndex = listOfLines.IndexOf("<span class='page-name' data-testid='page-title'>Properties</span>");
            var sectionIndex = listOfLines.IndexOf("<!-- Data Properties Section -->");

            Assert.That(titleIndex, Is.GreaterThan(1));
            Assert.That(sectionIndex, Is.GreaterThan(titleIndex));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataProjectPropertiesView_IsBalanced()
        {
            var project = CreateProjectWithSections(
                CreateSection("Test Run", ("Run ID", "RUN-1042"), ("Run Name", "Nightly")),
                CreateSection("Environment", ("Environment", "Staging"), ("Browser", "Chrome")));

            var generator = new LivingDocDataPropertiesGenerator(project);
            var listOfLines = generator.GenerateDataProjectPropertiesView();

            var html = string.Join("\n", listOfLines);
            Assert.That(CountOf(html, "<div"), Is.EqualTo(CountOf(html, "</div>")));
            Assert.That(CountOf(html, "<table"), Is.EqualTo(CountOf(html, "</table>")));
            Assert.That(CountOf(html, "<tr>"), Is.EqualTo(CountOf(html, "</tr>")));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataProjectPropertiesView_WithoutSections_HasOnlyTitle()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataProjectPropertiesView();

            Assert.That(listOfLines, Does.Contain("<span class='page-name' data-testid='page-title'>Properties</span>"));
            Assert.That(listOfLines, Does.Not.Contain("<!-- Data Properties Section -->"));
            Assert.That(listOfLines, Does.Not.Contain("<table class='properties-list'>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesTitle()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesTitle();

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<div class='section'>"));
            Assert.That(listOfLines[1], Is.EqualTo("<span class='page-name' data-testid='page-title'>Properties</span>"));
            Assert.That(listOfLines[2], Is.EqualTo("</div>"));
            Assert.That(listOfLines[3], Is.EqualTo("<hr>"));
            Assert.That(listOfLines[4], Is.EqualTo("<hr>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSections_RendersOneSectionPerGroup()
        {
            var project = CreateProjectWithSections(
                CreateSection("Test Run", ("Run ID", "RUN-1042")),
                CreateSection("Environment", ("Environment", "Staging")),
                CreateSection("Source Control", ("Git Branch", "main")));

            var generator = new LivingDocDataPropertiesGenerator(project);
            var listOfLines = generator.GenerateDataPropertiesSections();

            var sectionNames = listOfLines.Where(x => x.StartsWith("<span class='chart-name'>")).ToList();

            Assert.That(listOfLines.Count(x => x == "<!-- Data Properties Section -->"), Is.EqualTo(3));
            Assert.That(listOfLines.Count(x => x == "<table class='properties-list'>"), Is.EqualTo(3));
            Assert.That(sectionNames, Is.EqualTo(new[]
            {
                "<span class='chart-name'>Test Run</span>",
                "<span class='chart-name'>Environment</span>",
                "<span class='chart-name'>Source Control</span>"
            }));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSections_PropertiesStayInTheirSection()
        {
            var project = CreateProjectWithSections(
                CreateSection("Test Run", ("Run ID", "RUN-1042")),
                CreateSection("Environment", ("Browser", "Chrome")));

            var generator = new LivingDocDataPropertiesGenerator(project);
            var listOfLines = generator.GenerateDataPropertiesSections();

            var testRunIndex = listOfLines.IndexOf("<span class='chart-name'>Test Run</span>");
            var runIdIndex = listOfLines.IndexOf("<tr><td>Run ID</td><td>RUN-1042</td></tr>");
            var environmentIndex = listOfLines.IndexOf("<span class='chart-name'>Environment</span>");
            var browserIndex = listOfLines.IndexOf("<tr><td>Browser</td><td>Chrome</td></tr>");

            Assert.That(runIdIndex, Is.GreaterThan(testRunIndex));
            Assert.That(runIdIndex, Is.LessThan(environmentIndex));
            Assert.That(browserIndex, Is.GreaterThan(environmentIndex));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSections_WithoutSections_IsEmpty()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesSections();

            Assert.That(listOfLines, Is.Empty);
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSections_SkipsEmptySections()
        {
            var project = new LivingDocProject();
            project.Properties.Sections.Add(new LivingDocPropertySection { Name = "Empty" });
            project.Properties.Sections.Add(CreateSection("Environment", ("Environment", "Staging")));

            var generator = new LivingDocDataPropertiesGenerator(project);
            var listOfLines = generator.GenerateDataPropertiesSections();

            Assert.That(listOfLines.Count(x => x == "<!-- Data Properties Section -->"), Is.EqualTo(1));
            Assert.That(listOfLines, Does.Contain("<span class='chart-name'>Environment</span>"));
            Assert.That(listOfLines, Does.Not.Contain("<span class='chart-name'>Empty</span>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSection_Structure()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesSection(CreateSection("Environment", ("Environment", "Staging")));

            Assert.That(listOfLines[0], Is.EqualTo("<hr>"));
            Assert.That(listOfLines[1], Is.EqualTo("<!-- Data Properties Section -->"));
            Assert.That(listOfLines[2], Is.EqualTo("<div class='section properties-section'>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='chart-name'>Environment</span>"));
            Assert.That(listOfLines, Does.Contain("<table class='properties-list'>"));
            Assert.That(listOfLines, Does.Contain("<th width='50%'>Name</th>"));
            Assert.That(listOfLines, Does.Contain("<th width='50%'>Value</th>"));
            Assert.That(listOfLines, Does.Contain("<tbody>"));
            Assert.That(listOfLines, Does.Contain("</tbody>"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSection_WithoutProperties_HasNoRows()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesSection(new LivingDocPropertySection { Name = "Empty" });

            var tbodyIndex = listOfLines.IndexOf("<tbody>");
            var tbodyEndIndex = listOfLines.IndexOf("</tbody>");

            Assert.That(tbodyEndIndex, Is.EqualTo(tbodyIndex + 1));
            Assert.That(listOfLines, Has.None.StartsWith("<tr><td>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSection_RendersOneRowPerProperty()
        {
            var section = CreateSection("Test Run",
                ("Run ID", "RUN-1042"),
                ("Environment", "Staging"),
                ("Browser", "Chrome"));

            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesSection(section);

            var rows = listOfLines.Where(x => x.StartsWith("<tr><td>")).ToList();

            Assert.That(rows.Count, Is.EqualTo(3));
            Assert.That(rows[0], Is.EqualTo("<tr><td>Run ID</td><td>RUN-1042</td></tr>"));
            Assert.That(rows[1], Is.EqualTo("<tr><td>Environment</td><td>Staging</td></tr>"));
            Assert.That(rows[2], Is.EqualTo("<tr><td>Browser</td><td>Chrome</td></tr>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSection_RowsAreInsideTableBody()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesSection(CreateSection("General", ("Environment", "Staging")));

            var tbodyIndex = listOfLines.IndexOf("<tbody>");
            var rowIndex = listOfLines.IndexOf("<tr><td>Environment</td><td>Staging</td></tr>");
            var tbodyEndIndex = listOfLines.IndexOf("</tbody>");

            Assert.That(rowIndex, Is.GreaterThan(tbodyIndex));
            Assert.That(rowIndex, Is.LessThan(tbodyEndIndex));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSection_HandlesNullAndEmptyValues()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesSection(CreateSection("General", ("Empty", ""), ("Missing", null)));

            Assert.That(listOfLines, Does.Contain("<tr><td>Empty</td><td></td></tr>"));
            Assert.That(listOfLines, Does.Contain("<tr><td>Missing</td><td></td></tr>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSection_KeepsDuplicateNames()
        {
            var generator = new LivingDocDataPropertiesGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateDataPropertiesSection(CreateSection("General", ("Tag", "Smoke"), ("Tag", "Regression")));

            Assert.That(listOfLines, Does.Contain("<tr><td>Tag</td><td>Smoke</td></tr>"));
            Assert.That(listOfLines, Does.Contain("<tr><td>Tag</td><td>Regression</td></tr>"));
        }

        [Test]
        public void LivingDocDataPropertiesGenerator_GenerateDataPropertiesSections_KeepsDuplicateSectionNames()
        {
            var project = CreateProjectWithSections(
                CreateSection("General", ("Run ID", "RUN-1042")),
                CreateSection("General", ("Browser", "Chrome")));

            var generator = new LivingDocDataPropertiesGenerator(project);
            var listOfLines = generator.GenerateDataPropertiesSections();

            Assert.That(listOfLines.Count(x => x == "<span class='chart-name'>General</span>"), Is.EqualTo(2));
        }

        private static int CountOf(string text, string value)
        {
            var count = 0;
            var index = 0;

            while ((index = text.IndexOf(value, index, System.StringComparison.Ordinal)) >= 0)
            {
                count++;
                index += value.Length;
            }

            return count;
        }
    }
}
