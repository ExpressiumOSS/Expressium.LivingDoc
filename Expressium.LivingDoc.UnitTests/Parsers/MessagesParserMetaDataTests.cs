using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserMetaDataTests
    {
        [Test]
        public void Converting_Scenario_Environment_Meta_Data()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.ProtocolVersion, Is.EqualTo("27.0.0"));
            Assert.That(livingDocProject.ImplementationName, Is.EqualTo("fake-cucumber"));
            Assert.That(livingDocProject.ImplementationVersion, Is.EqualTo("18.0.0"));
            Assert.That(livingDocProject.RuntimeName, Is.EqualTo("node.js"));
            Assert.That(livingDocProject.RuntimeVersion, Is.EqualTo("22.7.0"));
            Assert.That(livingDocProject.OsName, Is.EqualTo("darwin"));
            Assert.That(livingDocProject.OsVersion, Is.EqualTo("23.6.0"));
            Assert.That(livingDocProject.CpuName, Is.EqualTo("x64"));
        }
    }
}
