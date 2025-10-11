using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataObjectsGeneratorTests
    {
        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeaturesTags()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Tags = new List<string> { "@tag1", "@tag2" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureTags(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='feature-tag-names'>@tag1 @tag2</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeaturesName()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Name = "Feature Name",
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureName(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Name -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='status-dot bgcolor-skipped'></span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='feature-keyword'>Feature: </span><span class='feature-name'>Feature Name</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureDescription()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Description = "Line One\nLine Two\nLine Three"
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureDescription(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Description -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<ul class='feature-description'>"));
            Assert.That(listOfLines[3], Is.EqualTo("<li>Line One</li>"));
            Assert.That(listOfLines[4], Is.EqualTo("<li>Line Two</li>"));
            Assert.That(listOfLines[5], Is.EqualTo("<li>Line Three</li>"));
            Assert.That(listOfLines[6], Is.EqualTo("</ul>"));
            Assert.That(listOfLines[7], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureBackground()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Background = new LivingDocBackground
                {
                    Steps = new List<LivingDocStep>
                    {
                        new LivingDocStep
                        {
                            Keyword = "Given",
                            Name = "I have logged in to the application"
                        }
                    }
                }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureBackground(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(15));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Background -->"));
            Assert.That(listOfLines[8], Is.EqualTo("<span class='step-keyword'>Given</span>"));
            Assert.That(listOfLines[9], Is.EqualTo("<span>I have logged in to the application</span>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureBackgroundSteps()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Background = new LivingDocBackground
                {
                    Steps = new List<LivingDocStep>
                    {
                        new LivingDocStep
                        {
                            Keyword = "Given",
                            Name = "I have logged in to the application"
                        }
                    }
                }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureBackgroundSteps(feature.Background.Steps);

            Assert.That(listOfLines.Count, Is.EqualTo(10));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Background Steps -->"));
            Assert.That(listOfLines[5], Is.EqualTo("<span class='step-keyword'>Given</span>"));
            Assert.That(listOfLines[6], Is.EqualTo("<span>I have logged in to the application</span>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataRuleTags()
        {
            var livingDocProject = new LivingDocProject();

            var rule = new LivingDocRule
            {
                Tags = new List<string> { "@tag5", "@tag6" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataRuleTags(rule);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Rule Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='rule-tag-names'>@tag5 @tag6</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioRule()
        {
            var livingDocProject = new LivingDocProject();

            var rule = new LivingDocRule
            {
                Name = "Orders",
                Tags = new List<string> { "@tag5", "@tag6" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioRule(rule, null);

            Assert.That(listOfLines.Count, Is.EqualTo(13));
            Assert.That(listOfLines[6], Is.EqualTo("<!-- Data Rule Name -->"));
            Assert.That(listOfLines[8], Does.Contain("Rule:"));
            Assert.That(listOfLines[9], Does.Contain("Orders"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioTags()
        {
            var livingDocProject = new LivingDocProject();

            var scenario = new LivingDocScenario
            {
                Tags = new List<string> { "@tag3", "@tag4" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioTags(scenario);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='scenario-tag-names'>@tag3 @tag4</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioName()
        {
            var livingDocProject = new LivingDocProject();

            var scenario = new LivingDocScenario
            {
                Name = "Scenario Name"
            };

            var example = new LivingDocExample
            {
                Duration = new TimeSpan(0, 0, 0, 1, 500)
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioName(scenario, example, "5");

            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Name -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='status-dot bgcolor-skipped'></span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='scenario-keyword'>Scenario: </span>"));
            Assert.That(listOfLines[4], Is.EqualTo("<span class='scenario-name'>Scenario Name</span>"));
            Assert.That(listOfLines[5], Is.EqualTo("<span class='circle-number'>5</span>"));
            Assert.That(listOfLines[6], Is.EqualTo("<span class='scenario-duration'>&nbsp;1s 500ms</span>"));
            Assert.That(listOfLines[7], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioSteps()
        {
            var livingDocProject = new LivingDocProject();

            var steps = new List<LivingDocStep>
            {
                new LivingDocStep
                {
                    Keyword = "Given",
                    Name = "I have logged in to the application"
                }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioSteps(steps);

            Assert.That(listOfLines.Count, Is.EqualTo(10));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Steps -->"));
            Assert.That(listOfLines[5], Is.EqualTo("<span class='step-keyword'>Given</span>"));
            Assert.That(listOfLines[6], Is.EqualTo("<span>I have logged in to the application</span>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioExamples()
        {
            var livingDocProject = new LivingDocProject();

            var examples = new LivingDocExample
            {
                DataTable = new LivingDocDataTable
                {
                    Rows = new List<LivingDocDataTableRow>
                    {
                        new LivingDocDataTableRow
                        {
                            Cells = new List<string> { "username", "password" }
                        },
                    }
                },
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioExamples(examples);

            Assert.That(listOfLines.Count, Is.EqualTo(17));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Examples -->"));
            Assert.That(listOfLines[8], Is.EqualTo("<td>username</td>"));
            Assert.That(listOfLines[10], Is.EqualTo("<td>password</td>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioAttachments()
        {
            var livingDocProject = new LivingDocProject();

            var example = new LivingDocExample
            {
                Attachments = new List<string>
                {
                    "screenshot1.png",
                    "logfile1.txt"
                }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioAttachments(example);

            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Attachments -->"));
            Assert.That(listOfLines[4], Does.Contain("screenshot1.png"));
            Assert.That(listOfLines[5], Does.Contain("logfile1.txt"));
        }
    }
}
