using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataGeneratorTests
    {
        private LivingDocProject CreateLivingDocProject()
        {
            var project = new LivingDocProject
            {
                Title = "Test Project"
            };

            var feature = new LivingDocFeature
            {
                Tags = new List<string> { "@Login" },
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
                                Duration = new TimeSpan(0, 3, 45),
                                Steps = new List<LivingDocStep>
                                {
                                    new LivingDocStep { Keyword = "Given", Name = "I have logged in with valid user credentials", Status = "Passed" }
                                }
                            }
                        }
                    }
                }
            };

            project.Features.Add(feature);

            return project;
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataOverview()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);
            var listOfLines = generator.GenerateDataOverview();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Overview -->"));
            Assert.That(listOfLines, Does.Contain("<div id='project-view'>"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataOverview_With_Features()
        {
            var generator = new LivingDocDataGenerator(CreateLivingDocProject());
            var listOfLines = generator.GenerateDataOverview();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Overview -->"));
            Assert.That(listOfLines, Has.Some.Contains("Login Feature"));
            Assert.That(listOfLines, Has.Some.Contains("Successful User Login with Valid Credentials"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataListViews()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);
            var listOfLines = generator.GenerateDataListViews();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(55));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Features View -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Scenarios View -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Steps View -->"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataListViews_With_Features()
        {
            var generator = new LivingDocDataGenerator(CreateLivingDocProject());
            var listOfLines = generator.GenerateDataListViews();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Features View -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Scenarios View -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Steps View -->"));
            Assert.That(listOfLines, Has.Some.Contains("Login Feature"));
            Assert.That(listOfLines, Has.Some.Contains("Successful User Login with Valid Credentials"));
            Assert.That(listOfLines, Has.Some.Contains("Given I have logged in with valid user credentials"));
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

        [Test]
        public void LivingDocDataGenerator_GenerateDataObjects_With_Features()
        {
            var generator = new LivingDocDataGenerator(CreateLivingDocProject());
            var listOfLines = generator.GenerateDataObjects();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.GreaterThan(0));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Feature Section -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Scenario -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Scenario Section -->"));
            Assert.That(listOfLines, Has.Some.Contains("Login Feature"));
            Assert.That(listOfLines, Has.Some.Contains("Successful User Login with Valid Credentials"));
            Assert.That(listOfLines, Has.Some.Contains("Given"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataAnalytics()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);
            var listOfLines = generator.GenerateDataAnalytics();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Analytics -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='analytics-features'>"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics Duration -->"));
            Assert.That(listOfLines, Has.Some.Contains("id='analytics-features'"));
            Assert.That(listOfLines, Has.Some.Contains("id='analytics-scenarios'"));
            Assert.That(listOfLines, Has.Some.Contains("id='analytics-steps'"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataProperties_Without_Properties()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocDataGenerator(project);
            var listOfLines = generator.GenerateDataProperties();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Properties -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='properties'>"));
            Assert.That(listOfLines, Does.Not.Contain("<!-- Data Properties Section -->"));
            Assert.That(listOfLines, Does.Not.Contain("<table class='properties-list'>"));
            Assert.That(listOfLines, Has.None.StartsWith("<tr><td>"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataGenerator_GenerateDataProperties_With_Properties()
        {
            var project = CreateLivingDocProject();
            var section = new LivingDocPropertySection { Name = "Environment" };
            section.Properties.Add(new LivingDocProperty { Name = "Operating System", Value = "Windows 10" });
            section.Properties.Add(new LivingDocProperty { Name = "Browser", Value = "Chrome" });
            project.Properties.Sections.Add(section);

            var generator = new LivingDocDataGenerator(project);
            var listOfLines = generator.GenerateDataProperties();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Properties -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div id='properties'>"));
            Assert.That(listOfLines, Does.Contain("<tr><td>Operating System</td><td>Windows 10</td></tr>"));
            Assert.That(listOfLines, Does.Contain("<tr><td>Browser</td><td>Chrome</td></tr>"));
            Assert.That(listOfLines, Does.Contain("<span class='chart-name'>Environment</span>"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }
    }
}
