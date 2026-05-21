using Expressium.LivingDoc.Parsers;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserDocStringsTests
    {
        [Test]
        public void Converting_DocStrings_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "doc-strings", "doc-strings.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(3));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Doc strings"));
        }

        [Test]
        public void Converting_DocStrings_StandardDelimiter()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "doc-strings", "doc-strings.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("a doc string with standard delimiter"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(scenario.Examples[0].Steps[0].Name, Is.EqualTo("a doc string:"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void Converting_DocStrings_BackticksDelimiter()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "doc-strings", "doc-strings.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Name, Is.EqualTo("a doc string with backticks delimiter"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(scenario.Examples[0].Steps[0].Name, Is.EqualTo("a doc string:"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void Converting_DocStrings_MediaType()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "doc-strings", "doc-strings.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            var scenario = livingDocProject.Features[0].Scenarios[2];

            Assert.That(scenario.Name, Is.EqualTo("a doc string with media type"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(scenario.Examples[0].Steps[0].Name, Is.EqualTo("a doc string:"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }
    }
}
