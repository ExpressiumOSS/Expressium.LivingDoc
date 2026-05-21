using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserMetaDataTests
    {
        [Test]
        public void Converting_Scenario_Environment_Meta_Data()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "minimal", "minimal.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.ProtocolVersion, Is.EqualTo("31.1.0"));
            Assert.That(livingDocProject.ImplementationName, Is.EqualTo("fake-cucumber"));
            Assert.That(livingDocProject.ImplementationVersion, Is.EqualTo("123.45.6"));
            Assert.That(livingDocProject.RuntimeName, Is.EqualTo("Node.js"));
            Assert.That(livingDocProject.RuntimeVersion, Is.EqualTo("24.4.1"));
            Assert.That(livingDocProject.OsName, Is.EqualTo("darwin"));
            Assert.That(livingDocProject.OsVersion, Is.EqualTo("24.5.0"));
            Assert.That(livingDocProject.CpuName, Is.EqualTo("arm64"));
        }
    }
}
