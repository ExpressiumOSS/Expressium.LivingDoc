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
            var feature = new LivingDocFeature
            {
                Tags = new List<string> { "@tag1", "@tag2" }
            };

            var livingDocDataObjectsGenerator = new LivingDocDataObjectsGenerator();
            var listOfLines = livingDocDataObjectsGenerator.GenerateDataFeatureTags(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='tag-names'>@tag1 @tag2</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeaturesName()
        {
            var feature = new LivingDocFeature
            {
                Name = "Feature Name",
            };

            var livingDocDataObjectsGenerator = new LivingDocDataObjectsGenerator();
            var listOfLines = livingDocDataObjectsGenerator.GenerateDataFeatureName(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Name -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='status-dot bgcolor-skipped'></span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='feature-keyword'>Feature: </span><span class='feature-name'>Feature Name</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataRuleTags()
        {
            var rule = new LivingDocRule
            {
                Tags = new List<string> { "@tag5", "@tag6" }
            };

            var livingDocDataObjectsGenerator = new LivingDocDataObjectsGenerator();
            var listOfLines = livingDocDataObjectsGenerator.GenerateDataRuleTags(rule);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Rule Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='tag-names'>@tag5 @tag6</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioTags()
        {
            var scenario = new LivingDocScenario
            {
                Tags = new List<string> { "@tag3", "@tag4" }
            };

            var livingDocDataObjectsGenerator = new LivingDocDataObjectsGenerator();
            var listOfLines = livingDocDataObjectsGenerator.GenerateDataScenarioTags(scenario);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='tag-names'>@tag3 @tag4</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioName()
        {
            var scenario = new LivingDocScenario
            {
                Name = "Scenario Name"
            };

            var example = new LivingDocExample
            {
                Duration = new TimeSpan(0, 0, 0, 1, 500)
            };

            var livingDocDataObjectsGenerator = new LivingDocDataObjectsGenerator();
            var listOfLines = livingDocDataObjectsGenerator.GenerateDataScenarioName(scenario, example, "5");

            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Name -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='status-dot bgcolor-skipped'></span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='scenario-keyword'>Scenario: </span>"));
            Assert.That(listOfLines[4], Is.EqualTo("<span class='scenario-name'>Scenario Name</span>"));
            Assert.That(listOfLines[5], Is.EqualTo("<span class='circle-number'>5</span>"));
            Assert.That(listOfLines[6], Is.EqualTo("<span class='duration'>&nbsp;1s 500ms</span>"));
            Assert.That(listOfLines[7], Is.EqualTo("</div>"));
        }
    }
}
