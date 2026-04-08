using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocRuleTests
    {
        [Test]
        public void LivingDocRule_GetTags()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "rules.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.Rules[0].GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.Rules[0].Name, Is.EqualTo("A sale cannot happen if the customer does not have enough money"));

            Assert.That(livingDocFeature.Rules[1].GetTags(), Is.EqualTo("@some-tag"));
            Assert.That(livingDocFeature.Rules[1].Name, Is.EqualTo("a sale cannot happen if there is no stock"));
        }

        // NEW
        [Test]
        public void LivingDocRule_Constructor_InitialisesTagsAsEmptyList()
        {
            var rule = new LivingDocRule();

            Assert.That(rule.Tags, Is.Not.Null);
            Assert.That(rule.Tags, Is.Empty);
        }

        [Test]
        public void LivingDocRule_Constructor_PropertiesDefaultToNull()
        {
            var rule = new LivingDocRule();

            Assert.That(rule.Id, Is.Null);
            Assert.That(rule.Name, Is.Null);
            Assert.That(rule.Description, Is.Null);
            Assert.That(rule.Keyword, Is.Null);
        }

        [Test]
        public void LivingDocRule_Properties_CanBeAssigned()
        {
            var rule = new LivingDocRule
            {
                Id = "rule-001",
                Name = "A sale cannot happen without stock",
                Description = "Stock validation rule",
                Keyword = "Rule"
            };

            Assert.That(rule.Id, Is.EqualTo("rule-001"));
            Assert.That(rule.Name, Is.EqualTo("A sale cannot happen without stock"));
            Assert.That(rule.Description, Is.EqualTo("Stock validation rule"));
            Assert.That(rule.Keyword, Is.EqualTo("Rule"));
        }

        [Test]
        public void LivingDocRule_GetTags_ReturnsSpaceSeparatedTags()
        {
            var rule = new LivingDocRule();
            rule.Tags.Add("@smoke");
            rule.Tags.Add("@regression");

            Assert.That(rule.GetTags(), Is.EqualTo("@smoke @regression"));
        }

        [Test]
        public void LivingDocRule_GetTags_ReturnsEmptyStringWhenNoTags()
        {
            var rule = new LivingDocRule();

            Assert.That(rule.GetTags(), Is.EqualTo(""));
        }
    }
}
