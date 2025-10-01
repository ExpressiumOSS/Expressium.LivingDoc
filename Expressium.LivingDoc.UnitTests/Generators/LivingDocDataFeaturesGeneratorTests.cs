using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataFeaturesGeneratorTests
    {
        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeaturesTags()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Tags = new List<string> { "@tag1", "@tag2" }
            };

            var livingDocDataFeaturesGenerator = new LivingDocDataFeaturesGenerator(livingDocProject);
            var listOfLines = livingDocDataFeaturesGenerator.GenerateDataFeatureTags(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(4));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='tag-names'>@tag1 @tag2</span>"));
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

            var livingDocDataFeaturesGenerator = new LivingDocDataFeaturesGenerator(livingDocProject);
            var listOfLines = livingDocDataFeaturesGenerator.GenerateDataFeatureName(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Name -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='status-dot bgcolor-skipped'></span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='feature-keyword'>Feature: </span><span class='feature-name'>Feature Name</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</div>"));
        }
    }
}
