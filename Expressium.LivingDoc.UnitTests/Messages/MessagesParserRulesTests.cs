using Expressium.LivingDoc.Messages;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesParserRulesTests
    {
        [Test]
        public void Converting_Feature_Rules()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "rules.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(12));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Uri, Is.EqualTo("samples/rules/rules.feature"));

            Assert.That(feature.Rules[0].Name, Is.EqualTo("A sale cannot happen if the customer does not have enough money"));
            Assert.That(feature.Rules[0].Id, Is.EqualTo("16"));
            Assert.That(feature.Scenarios[0].RuleId, Is.EqualTo("16"));

            Assert.That(feature.Rules[1].Name, Is.EqualTo("a sale cannot happen if there is no stock"));
            Assert.That(feature.Rules[1].Id, Is.EqualTo("23"));
            Assert.That(feature.Scenarios[2].RuleId, Is.EqualTo("23"));
        }
    }
}
