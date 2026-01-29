using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserCoffeeshopTests
    {
        [Test]
        public void Converting_Scenario_Coffeeshop()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var livingDocFeature = livingDocProject.Features[2];
            Assert.That(livingDocFeature.Name, Is.EqualTo("Orders"));

            var livingDocScenario = livingDocFeature.Scenarios[0];
            Assert.That(livingDocScenario.Name, Is.EqualTo("Ordering Coffee Confirmation Notification"));
            Assert.That(livingDocScenario.HasDataTable(), Is.True);

            Assert.That(livingDocScenario.Examples[0].GetDuration(), Is.EqualTo("4s 393ms"));
            Assert.That(livingDocScenario.Examples[0].Attachments[0], Does.Contain("SantaRitaCerradoMineiro $99"));

            Assert.That(livingDocScenario.Examples[1].GetDuration(), Is.EqualTo("5s 628ms"));
            Assert.That(livingDocScenario.Examples[1].Attachments[0], Does.Contain("LaSoledadAntioquia $77"));
        }
    }
}
