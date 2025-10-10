using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataOverviewGeneratorTests
    {
        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewHeaderFolder()
        {
            var livingDocProject = new LivingDocProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewHeaderFolder("\\Root Folder");

            Assert.That(listOfLines.Count, Is.EqualTo(10));
            Assert.That(listOfLines[3], Does.Contain("Root Folder"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewFolder()
        {
            var livingDocProject = new LivingDocProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewFolder("\\Root\\SubFolder", 1);

            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[4], Does.Contain("SubFolder"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewFeature()
        {
            var livingDocProject = GetProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewFeature(livingDocProject.Features[0], 1);

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[4], Does.Contain("bgcolor-passed"));
            Assert.That(listOfLines[5], Does.Contain("Login Feature"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewScenario()
        {
            var livingDocProject = GetProject();

            var generator = new LivingDocDataOverviewGenerator(livingDocProject);
            var listOfLines = generator.GenerateOverviewScenario(livingDocProject.Features[0], livingDocProject.Features[0].Scenarios[0], 1);

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[4], Does.Contain("bgcolor-passed"));
            Assert.That(listOfLines[5], Does.Contain("Successful User Login with Valid Credentials"));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("\\Root", "Root")]
        [TestCase("\\Root\\", "")]
        [TestCase("\\Root\\SubFolder", "SubFolder")]
        [TestCase("\\Root\\Folder\\SubFolder", "SubFolder")]
        public void LivingDocDataOverviewGenerator_GenerateOverviewHeaderFolder(string value, string expected)
        {
            var result = LivingDocDataOverviewGenerator.GetFolderName(value);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_No_Locators()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth(null);
            Assert.That(folderDepth, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_One_Locator()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth("Features");
            Assert.That(folderDepth, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_Two_Locators()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth("Features\\Login");
            Assert.That(folderDepth, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_Tree_Locators()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth("Features\\Login\\Exp");
            Assert.That(folderDepth, Is.EqualTo(3));
        }

        private LivingDocProject GetProject()
        {
            var livingDocProject = new LivingDocProject();

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
