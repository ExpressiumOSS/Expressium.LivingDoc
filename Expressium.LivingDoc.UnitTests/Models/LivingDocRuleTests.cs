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

            Assert.That(livingDocFeature.Rules[0].GetTags, Is.EqualTo(""));
            Assert.That(livingDocFeature.Rules[0].Name, Is.EqualTo("A sale cannot happen if the customer does not have enough money"));

            Assert.That(livingDocFeature.Rules[1].GetTags, Is.EqualTo("@some-tag"));
            Assert.That(livingDocFeature.Rules[1].Name, Is.EqualTo("a sale cannot happen if there is no stock"));
        }
    }
}
