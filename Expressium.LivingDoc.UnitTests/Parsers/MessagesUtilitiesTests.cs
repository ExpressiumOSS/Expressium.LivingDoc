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

        [TestCase("plain text", "plain text")]
        [TestCase("Tom's feature", "Tom&#39;s feature")]
        [TestCase("<b>bold</b>", "&lt;b&gt;bold&lt;/b&gt;")]
        [TestCase("A & B", "A &amp; B")]
        [TestCase("\"quoted\"", "&quot;quoted&quot;")]
        [TestCase("line 1\nline 2", "line 1\nline 2")]
        public void HtmlEncode_EncodesSpecialCharacters(string input, string expected)
        {
            Assert.That(input.HtmlEncode(), Is.EqualTo(expected));
        }

        [Test]
        public void HtmlEncode_HandlesEmptyAndNull()
        {
            Assert.That(((string)null).HtmlEncode(), Is.Null);
            Assert.That(string.Empty.HtmlEncode(), Is.Empty);
        }

        [Test]
        public void HtmlEncode_AlreadyEncodedText_IsEncodedAgain()
        {
            // Encoding must only happen once - in the parser...
            Assert.That("&amp;".HtmlEncode(), Is.EqualTo("&amp;amp;"));
        }
    }
}