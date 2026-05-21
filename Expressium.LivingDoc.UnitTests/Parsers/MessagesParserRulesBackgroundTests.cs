using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserRulesBackgroundTests
    {
        [Test]
        public void Converting_Feature_Rules()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "rules-backgrounds", "rules-backgrounds.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(10));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Rules[0].Name, Is.EqualTo(""));
            Assert.That(feature.Rules[0].Description.Trim(), Is.EqualTo(""));
        }
    }
}