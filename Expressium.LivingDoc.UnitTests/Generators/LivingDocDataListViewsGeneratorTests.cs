using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataListViewsGeneratorTests
    {
        [Test]
        public void LivingDocDataListViewsGenerator_GenerateDataFeaturesListView()
        {
            var generator = new LivingDocDataListViewsGenerator(CreateLivingDocProject());
            var listOfLines = generator.GenerateDataFeaturesListView();

            Assert.That(listOfLines.Count, Is.EqualTo(27));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Features View -->"));
            Assert.That(listOfLines[15], Does.Contain("@Login"));
            Assert.That(listOfLines[17], Does.Contain("Login"));
            Assert.That(listOfLines[18], Does.Contain("0001"));
            Assert.That(listOfLines[19], Does.Contain("100%"));
            Assert.That(listOfLines[20], Does.Contain("3min 45s"));
            Assert.That(listOfLines[21], Does.Contain("Passed"));
        }

        [Test]
        public void LivingDocDataListViewsGenerator_GenerateDataScenariosListView()
        {
            var generator = new LivingDocDataListViewsGenerator(CreateLivingDocProject());
            var listOfLines = generator.GenerateDataScenariosListView();

            Assert.That(listOfLines.Count, Is.EqualTo(25));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenarios View -->"));
            Assert.That(listOfLines[14], Does.Contain("@Login"));
            Assert.That(listOfLines[16], Does.Contain("Successful User Login with Valid Credentials"));
            Assert.That(listOfLines[17], Does.Contain("0000"));
            Assert.That(listOfLines[18], Does.Contain("3min 45s"));
            Assert.That(listOfLines[19], Does.Contain("Passed"));
        }

        [Test]
        public void LivingDocDataListViewsGenerator_GenerateDataStepsListView()
        {
            var generator = new LivingDocDataListViewsGenerator(CreateLivingDocProject());
            var listOfLines = generator.GenerateDataStepsListView();

            Assert.That(listOfLines.Count, Is.EqualTo(23));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Steps View -->"));
            Assert.That(listOfLines[13], Does.Contain("@Login"));
            Assert.That(listOfLines[15], Does.Contain("Given I have logged in with valid user credentials"));
            Assert.That(listOfLines[16], Does.Contain("0001"));
            Assert.That(listOfLines[17], Does.Contain("Passed"));
        }

        private LivingDocProject CreateLivingDocProject()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Tags = new List<string> { "@Login" },
                Name = "Login",
                Scenarios = new List<LivingDocScenario>
                {
                    new LivingDocScenario
                    {
                        Name = "Successful User Login with Valid Credentials",
                        Examples = new List<LivingDocExample>
                        {
                            new LivingDocExample
                            {
                                Duration = new TimeSpan(0,3,45),
                                Steps = new List<LivingDocStep>
                                {
                                    new LivingDocStep { Keyword = "Given", Name = "I have logged in with valid user credentials", Status= "Passed" },
                                }
                            }
                        }
                    }
                }
            };

            livingDocProject.Features.Add(feature);

            return livingDocProject;
        }
    }
}
