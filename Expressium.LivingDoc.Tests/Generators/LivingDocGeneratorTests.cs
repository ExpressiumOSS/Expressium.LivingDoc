using AngleSharp.Html.Parser;
using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocGeneratorTests
    {
        [Test]
        public void LivingDocGenerator_GenerateDocument()
        {
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            File.Delete(outputFilePath);

            var project = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature
                    {
                        Scenarios = new List<LivingDocScenario>
                        {
                            new LivingDocScenario { Name = "Sample Scenario" }
                        }
                    }
                }
            };

            var generator = new LivingDocGenerator("dummyInput", outputFilePath);
            generator.GenerateDocument(project);

            Assert.That(File.Exists(outputFilePath));
        }

        [Test]
        public void LivingDocGenerator_GenerateDocument_Invalid_Input_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "unknown.json");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "dummy.html");

            var generator = new LivingDocGenerator(inputFilePath, outputFilePath);

            var exception = Assert.Throws<IOException>(() => generator.Execute());
            Assert.That(exception.Message.StartsWith("IO error: Could not find file"));
        }

        [Test]
        public void LivingDocGenerator_GenerateDocument_Invalid_Output_Path()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");
            var outputFilePath = Path.Combine($"G:\\Output\\dummy.html");

            var generator = new LivingDocGenerator(inputFilePath, outputFilePath);

            var exception = Assert.Throws<IOException>(() => generator.Execute(true));
            Assert.That(exception.Message.StartsWith("IO error: Could not find a part of the path"));
        }

        [Test]
        public void LivingDocGenerator_GenerateDocument_Invalid_Input_File()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.feature.ndjson");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.html");

            var generator = new LivingDocGenerator(inputFilePath, outputFilePath);

            var exception = Assert.Throws<IOException>(() => generator.Execute());
            Assert.That(exception.Message.StartsWith("Unexpected error: Object reference not set to an instance of an object."));
        }

        [Test]
        public void LivingDocGenerator_SaveHtmlFile()
        {
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "savehtmlfile.html");

            File.Delete(outputFilePath);

            var inputLines = new List<string>
            {
                "<html>",
                "<body>",
                "<p>Hello, world!</p>",
                "</body>",
                "</html>"
            };

            LivingDocGenerator.SaveHtmlFile(outputFilePath, inputLines);

            Assert.That(File.Exists(outputFilePath), Is.True, "The HTML file should be created.");

            var savedContent = File.ReadAllText(outputFilePath);
            Assert.That(savedContent, Does.Contain("<p>Hello, world!</p>"), "The saved content should include the paragraph.");

            var parser = new HtmlParser();
            var document = parser.ParseDocument(savedContent);

            Assert.That(document.Body != null);
            Assert.That(document.Body.InnerHtml.Trim(), Does.Contain("Hello, world!"));
        }
    }
}
