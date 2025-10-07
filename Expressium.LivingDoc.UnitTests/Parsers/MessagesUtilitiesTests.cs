using Expressium.LivingDoc.Parsers;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    internal class MessagesUtilitiesTests
    {
        [Test]
        public void CapitalizeWords_CapitalizesEachWord()
        {
            var input = "hello world";
            var result = input.CapitalizeWords();
            Assert.That(result, Is.EqualTo("Hello World"));
        }

        [Test]
        public void CapitalizeWords_HandlesEmptyAndNull()
        {
            Assert.That(((string)null).CapitalizeWords(), Is.Null);
            Assert.That(string.Empty.CapitalizeWords(), Is.Empty);
        }

        [Test]
        public void CapitalizeWords_HandlesSingleWord()
        {
            Assert.That("test".CapitalizeWords(), Is.EqualTo("Test"));
        }
    }
}