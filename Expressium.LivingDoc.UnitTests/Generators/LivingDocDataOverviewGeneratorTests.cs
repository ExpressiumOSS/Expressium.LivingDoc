using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataOverviewGeneratorTests
    {
        private LivingDocProject CreateLivingDocProject()
        {
            var livingDocProject = new LivingDocProject
            {
                Title = "Test Project"
            };

            var feature = new LivingDocFeature
            {
                Name = "Login Feature",
                Scenarios = new List<LivingDocScenario>
                {
                    new LivingDocScenario
                    {
                        Name = "Successful User Login with Valid Credentials",
                        Examples = new List<LivingDocExample>
                        {
                            new LivingDocExample
                            {
                                Steps = new List<LivingDocStep>
                                {
                                    new LivingDocStep { Keyword = "Given", Name = "I have logged in with valid user credentials", Status = "Passed" }
                                }
                            }
                        }
                    }
                }
            };

            livingDocProject.Features.Add(feature);

            return livingDocProject;
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataOverviewGenerator(project);
            var listOfLines = generator.GenerateDataOverview();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Overview -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='project-view'>"));
            Assert.That(listOfLines, Does.Contain("<table id='table-grid' class='tree-view'>"));
            Assert.That(listOfLines, Does.Contain("<tbody id='table-list'>"));
            Assert.That(listOfLines, Does.Contain("</tbody>"));
            Assert.That(listOfLines, Does.Contain("</table>"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_With_Features()
        {
            var generator = new LivingDocDataOverviewGenerator(CreateLivingDocProject());
            var listOfLines = generator.GenerateDataOverview();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Overview -->"));
            Assert.That(listOfLines, Has.Some.Contains("Login Feature"));
            Assert.That(listOfLines, Has.Some.Contains("Successful User Login with Valid Credentials"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewHeaderFolder()
        {
            var livingDocProject = new LivingDocProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewHeaderFolder("\\Root Folder");

            Assert.That(listOfLines.Count, Is.EqualTo(10));
            Assert.That(listOfLines[0], Is.EqualTo("<tr>"));
            Assert.That(listOfLines[1], Is.EqualTo("<td class='grid-folder'>\ud83d\udcc2</td>"));
            Assert.That(listOfLines[3], Does.Contain("Root Folder"));
            Assert.That(listOfLines[6], Does.Contain("loadExpandAll()"));
            Assert.That(listOfLines[7], Does.Contain("loadCollapseAll()"));
            Assert.That(listOfLines[9], Is.EqualTo("</tr>"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewFolder()
        {
            var livingDocProject = new LivingDocProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewFolder("\\Root\\SubFolder", 1);

            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Does.Contain("data-role='folder'"));
            Assert.That(listOfLines[0], Does.Contain("data-name='"));
            Assert.That(listOfLines[2], Is.EqualTo("<td class='grid-folder'>\ud83d\udcc2</td>"));
            Assert.That(listOfLines[4], Does.Contain("SubFolder"));
            Assert.That(listOfLines[4], Does.Contain("grid-folder-name"));
            Assert.That(listOfLines[7], Is.EqualTo("</tr>"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewFeature()
        {
            var livingDocProject = CreateLivingDocProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewFeature(livingDocProject.Features[0], 1);

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[0], Does.Contain("data-role='feature'"));
            Assert.That(listOfLines[0], Does.Contain("data-featureid='"));
            Assert.That(listOfLines[0], Does.Contain("loadFeature(this)"));
            Assert.That(listOfLines[2], Does.Contain("loadCollapse(this)"));
            Assert.That(listOfLines[4], Does.Contain("bgcolor-passed"));
            Assert.That(listOfLines[5], Does.Contain("Login Feature"));
            Assert.That(listOfLines[8], Is.EqualTo("</tr>"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewScenario()
        {
            var livingDocProject = CreateLivingDocProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewScenario(livingDocProject.Features[0], livingDocProject.Features[0].Scenarios[0], 1);

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[0], Does.Contain("data-role='scenario'"));
            Assert.That(listOfLines[0], Does.Contain("data-featureid='"));
            Assert.That(listOfLines[0], Does.Contain("data-scenarioid='"));
            Assert.That(listOfLines[0], Does.Contain("loadScenario(this)"));
            Assert.That(listOfLines[4], Does.Contain("bgcolor-passed"));
            Assert.That(listOfLines[5], Does.Contain("Successful User Login with Valid Credentials"));
            Assert.That(listOfLines[8], Is.EqualTo("</tr>"));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("\\Root", "Root")]
        [TestCase("\\Root\\", "")]
        [TestCase("\\Root\\SubFolder", "SubFolder")]
        [TestCase("\\Root\\Folder\\SubFolder", "SubFolder")]
        public void LivingDocDataOverviewGenerator_GetFolderName(string value, string expected)
        {
            var result = LivingDocDataOverviewGenerator.GetFolderName(value);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("Features", 1)]
        [TestCase("Features\\Login", 2)]
        [TestCase("Features\\Login\\Exp", 3)]
        public void LivingDocDataOverviewGenerator_GetFolderDepth(string value, int expected)
        {
            var result = LivingDocDataOverviewGenerator.GetFolderDepth(value);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
