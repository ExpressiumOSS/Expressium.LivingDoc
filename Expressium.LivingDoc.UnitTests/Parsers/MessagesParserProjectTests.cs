using Expressium.LivingDoc.Parsers;
using System;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserProjectTests
    {
        [Test]
        public void Converting_Minimal_Project()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.Title, Is.EqualTo("Expressium LivingDoc"));
            Assert.That(livingDocProject.Date, Is.Not.EqualTo(default(DateTime)));
            Assert.That(livingDocProject.Duration, Is.GreaterThanOrEqualTo(TimeSpan.Zero));
        }
    }
}
