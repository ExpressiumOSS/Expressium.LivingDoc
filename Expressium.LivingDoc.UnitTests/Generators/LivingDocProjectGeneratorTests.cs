using AngleSharp.Html.Parser;
using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocProjectGeneratorTests
    {
        [Test]
        public void LivingDocProjectGenerator_GenerateContent()
        {
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

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateContent();

            Assert.That(listOfLines.Count, Is.GreaterThan(80));
        }

        [Test]
        public void LivingDocProjectGenerator_GenerateData()
        {
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

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateData();

            Assert.That(listOfLines.Count, Is.GreaterThan(300));
        }

        [Test]
        public void LivingDocProjectGenerator_SaveHtmlFile()
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

            LivingDocProjectGenerator.SaveHtmlFile(outputFilePath, inputLines);

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
