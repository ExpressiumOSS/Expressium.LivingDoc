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
        public void LivingDocProjectGenerator_GenerateHtmlHeader()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateHtmlHeader();

            Assert.That(listOfLines.Count, Is.EqualTo(2));
            Assert.That(listOfLines[0], Is.EqualTo("<!DOCTYPE html>"));
            Assert.That(listOfLines[1], Is.EqualTo("<html lang='en'>"));
        }

        [Test]
        public void LivingDocProjectGenerator_GenerateHtmlFooter()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateHtmlFooter();

            Assert.That(listOfLines.Count, Is.EqualTo(1));
            Assert.That(listOfLines[0], Is.EqualTo("</html>"));
        }

        [Test]
        public void LivingDocProjectGenerator_GenerateBodyHeader()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateBodyHeader();

            Assert.That(listOfLines.Count, Is.EqualTo(1));
            Assert.That(listOfLines[0], Does.Contain("<body"));
            Assert.That(listOfLines[0], Does.Contain("onload"));
        }

        [Test]
        public void LivingDocProjectGenerator_GenerateBodyFooter()
        {
            var project = new LivingDocProject();

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateBodyFooter();

            Assert.That(listOfLines.Count, Is.EqualTo(1));
            Assert.That(listOfLines[0], Is.EqualTo("</body>"));
        }

        [Test]
        public void LivingDocProjectGenerator_GenerateProperties()
        {
            var project = new LivingDocProject
            {
                OsVersion = "Windows 11",
                ImplementationName = "ReqnRoll",
                ImplementationVersion = "2.0.0"
            };

            var generator = new LivingDocProjectGenerator(project);
            var listOfLines = generator.GenerateProperties();

            Assert.That(listOfLines.Count, Is.EqualTo(3));
            Assert.That(listOfLines[0], Does.Contain("Windows 11"));
            Assert.That(listOfLines[1], Does.Contain("Expressium LivingDoc"));
            Assert.That(listOfLines[2], Does.Contain("ReqnRoll"));
            Assert.That(listOfLines[2], Does.Contain("2.0.0"));
        }

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

            Assert.That(listOfLines.Count, Is.GreaterThanOrEqualTo(80));
            Assert.That(listOfLines, Does.Contain("<!-- Header Section -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Project Navigation Section -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Footer Section -->"));
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
            Assert.That(listOfLines, Does.Contain("<!-- Data Overview -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Features View -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Analytics -->"));
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

        [Test]
        public void LivingDocProjectGenerator_SaveHtmlFile_Invalid_Output_Path()
        {
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "NonExistent", "Subfolder", "output.html");

            var inputLines = new List<string> { "<html></html>" };

            Assert.Throws<IOException>(() => LivingDocProjectGenerator.SaveHtmlFile(outputFilePath, inputLines));
        }
    }
}
