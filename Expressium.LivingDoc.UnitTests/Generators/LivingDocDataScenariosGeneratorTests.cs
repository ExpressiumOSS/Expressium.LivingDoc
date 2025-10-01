using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataScenariosGeneratorTests
    {
        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataRuleTags()
        {
            var livingDocProject = new LivingDocProject();

            var rule = new LivingDocRule
            {
                Tags = new List<string> { "@tag5", "@tag6" }
            };

            var livingDocDataScenariosGenerator = new LivingDocDataScenariosGenerator(livingDocProject);
            var listOfLines = livingDocDataScenariosGenerator.GenerateDataRuleTags(rule);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Rule Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='tag-names'>@tag5 @tag6</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioTags()
        {
            var livingDocProject = new LivingDocProject();

            var scenario = new LivingDocScenario
            {
                Tags = new List<string> { "@tag3", "@tag4" }
            };

            var livingDocDataScenariosGenerator = new LivingDocDataScenariosGenerator(livingDocProject);
            var listOfLines = livingDocDataScenariosGenerator.GenerateDataScenarioTags(scenario);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='tag-names'>@tag3 @tag4</span>"));
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

            var livingDocDataScenariosGenerator = new LivingDocDataScenariosGenerator(livingDocProject);
            var listOfLines = livingDocDataScenariosGenerator.GenerateDataScenarioName(scenario, example, "5");

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
    }
}
