using Expressium.LivingDoc.Parsers;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesParserMultipleFeaturesTests
    {
        [Test]
        public void Converting_MultipleFeatures_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "multiple-features", "multiple-features.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(9));
        }

        [Test]
        public void Converting_MultipleFeatures_FeatureNames()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "multiple-features", "multiple-features.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.Features[0].Name, Is.EqualTo("First feature"));
            Assert.That(livingDocProject.Features[1].Name, Is.EqualTo("Second feature"));
            Assert.That(livingDocProject.Features[2].Name, Is.EqualTo("Third feature"));
        }

        [Test]
        public void Converting_MultipleFeatures_FeatureUris()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "multiple-features", "multiple-features.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.Features[0].Uri, Is.EqualTo("samples/multiple-features/multiple-features-1.feature"));
            Assert.That(livingDocProject.Features[1].Uri, Is.EqualTo("samples/multiple-features/multiple-features-2.feature"));
            Assert.That(livingDocProject.Features[2].Uri, Is.EqualTo("samples/multiple-features/multiple-features-3.feature"));
        }

        [Test]
        public void Converting_MultipleFeatures_ScenarioNames()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "multiple-features", "multiple-features.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.Features[0].Scenarios[0].Name, Is.EqualTo("First scenario"));
            Assert.That(livingDocProject.Features[1].Scenarios[0].Name, Is.EqualTo("First scenario"));
            Assert.That(livingDocProject.Features[2].Scenarios[0].Name, Is.EqualTo("First scenario"));
        }

        [Test]
        public void Converting_MultipleFeaturesReversed_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "multiple-features-reversed", "multiple-features-reversed.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(9));
        }

        [Test]
        public void Converting_MultipleFeaturesReversed_FeatureNames()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", "multiple-features-reversed", "multiple-features-reversed.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.Features[0].Name, Is.EqualTo("First feature"));
            Assert.That(livingDocProject.Features[1].Name, Is.EqualTo("Second feature"));
            Assert.That(livingDocProject.Features[2].Name, Is.EqualTo("Third feature"));
        }
    }
}
