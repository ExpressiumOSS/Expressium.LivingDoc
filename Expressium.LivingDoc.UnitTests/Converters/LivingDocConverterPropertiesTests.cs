using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    internal class LivingDocConverterPropertiesTests
    {
        private string inputFilePath;
        private string propertiesFilePath;
        private string tempDirectory;

        [SetUp]
        public void SetUp()
        {
            inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.ndjson");
            propertiesFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "Properties.json");

            tempDirectory = Path.Combine(Path.GetTempPath(), "LivingDocPropertiesTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }

        [Test]
        public void LivingDocProject_Constructor_InitializesEmptyProperties()
        {
            var livingDocProject = new LivingDocProject();

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithPropertiesPath_LoadsAllProperties()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);

            var properties = livingDocProject.Properties;
            Assert.That(properties, Is.Not.Null);
            Assert.That(properties.Count, Is.EqualTo(10));
            Assert.That(properties[0].Name, Is.EqualTo("Run ID"));
            Assert.That(properties[0].Value, Is.EqualTo("RUN-1042"));
            Assert.That(properties[1].Name, Is.EqualTo("Run Name"));
            Assert.That(properties[1].Value, Is.EqualTo("Nightly Regression"));
            Assert.That(properties[9].Name, Is.EqualTo("Custom Property"));
            Assert.That(properties[9].Value, Is.EqualTo("Any value"));
        }

        [Test]
        public void LivingDocConverter_Convert_WithPropertiesPath_PreservesFileOrder()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);

            var names = livingDocProject.Properties.Select(p => p.Name).ToArray();
            Assert.That(names, Is.EqualTo(new[]
            {
                "Run ID", "Run Name", "Environment", "Browser", "Operating System",
                "Build Version", "Git Branch", "Git Commit", "Triggered By", "Custom Property"
            }));
        }

        [Test]
        public void LivingDocConverter_Convert_WithoutPropertiesPath_HasNoProperties()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        public void LivingDocConverter_Convert_WithNullOrEmptyPropertiesPath_HasNoProperties(string path)
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", path);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithMissingPropertiesFile_IsIgnored()
        {
            var missingPath = Path.Combine(tempDirectory, "missing-properties.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", missingPath);

            Assert.That(livingDocProject, Is.Not.Null);
            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.GreaterThan(0));
            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithMalformedPropertiesFile_IsIgnored()
        {
            // invalid.json is not valid JSON - the error is swallowed and the report is still produced...
            var malformedPath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "invalid.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", malformedPath);

            Assert.That(livingDocProject, Is.Not.Null);
            Assert.That(livingDocProject.Title, Is.EqualTo("MyProjectTitle"));
            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.GreaterThan(0));
            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithEmptyPropertiesArray_HasEmptyProperties()
        {
            var emptyPath = Path.Combine(tempDirectory, "empty-properties.json");
            File.WriteAllText(emptyPath, "[]");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", emptyPath);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithPascalCasePropertyNames_LoadsProperties()
        {
            // Deserialization is case insensitive...
            var pascalCasePath = Path.Combine(tempDirectory, "pascal-properties.json");
            File.WriteAllText(pascalCasePath, "[ { \"Name\": \"Environment\", \"Value\": \"Production\" } ]");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", pascalCasePath);

            Assert.That(livingDocProject.Properties.Count, Is.EqualTo(1));
            Assert.That(livingDocProject.Properties[0].Name, Is.EqualTo("Environment"));
            Assert.That(livingDocProject.Properties[0].Value, Is.EqualTo("Production"));
        }

        [Test]
        public void LivingDocConverter_Convert_WithUnrelatedJsonFile_HasNoProperties()
        {
            // A valid JSON object instead of the expected array...
            var unrelatedPath = Path.Combine(tempDirectory, "unrelated.json");
            File.WriteAllText(unrelatedPath, "{ \"environment\": \"Staging\" }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", unrelatedPath);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithLegacyWrappedPropertiesFile_HasNoProperties()
        {
            // The former { "properties": [...] } wrapper format is no longer supported...
            var legacyPath = Path.Combine(tempDirectory, "legacy-properties.json");
            File.WriteAllText(legacyPath, "{ \"properties\": [ { \"name\": \"Environment\", \"value\": \"Staging\" } ] }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", legacyPath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.GreaterThan(0));
            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_MergeProject_KeepsMasterProperties()
        {
            var inputFilePathOne = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsOne.ndjson");
            var inputFilePathTwo = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsTwo.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePathOne, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.MergeProject(livingDocProject, inputFilePathTwo);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(2));
            Assert.That(livingDocProject.Properties.Count, Is.EqualTo(10));
            Assert.That(livingDocProject.Properties[0].Value, Is.EqualTo("RUN-1042"));
        }

        [Test]
        public void LivingDocConverter_MergeProject_SlaveDoesNotAddProperties()
        {
            var inputFilePathOne = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsOne.ndjson");
            var inputFilePathTwo = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "parallelTestRunsTwo.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePathOne, "MyProjectTitle");
            livingDocConverter.MergeProject(livingDocProject, inputFilePathTwo);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocProject_Merge_DoesNotOverwriteMasterProperties()
        {
            var master = new LivingDocProject();
            master.Properties.Add(new LivingDocProperty { Name = "Environment", Value = "Staging" });

            var slave = new LivingDocProject();
            slave.Properties.Add(new LivingDocProperty { Name = "Environment", Value = "Production" });

            master.Merge(slave);

            Assert.That(master.Properties.Count, Is.EqualTo(1));
            Assert.That(master.Properties[0].Value, Is.EqualTo("Staging"));
        }

        [Test]
        public void LivingDocProject_Merge_DoesNotAddSlavePropertiesToEmptyMaster()
        {
            var master = new LivingDocProject();

            var slave = new LivingDocProject();
            slave.Properties = new List<LivingDocProperty> { new LivingDocProperty { Name = "Environment", Value = "Production" } };

            master.Merge(slave);

            Assert.That(master.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_MergeHistory_KeepsMasterProperties()
        {
            var historyPath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "History", "coffeeshop.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.MergeHistory(livingDocProject, historyPath);

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(4));
            Assert.That(livingDocProject.Properties.Count, Is.EqualTo(10));
        }

        [Test]
        public void LivingDocConverter_Export_Import_RoundTripsProperties()
        {
            var outputFilePath = Path.Combine(tempDirectory, "export-properties.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.Export(livingDocProject, outputFilePath);

            var reloadedProject = livingDocConverter.Import(outputFilePath);

            Assert.That(reloadedProject.Properties.Count, Is.EqualTo(10));
            Assert.That(reloadedProject.Properties[7].Name, Is.EqualTo("Git Commit"));
            Assert.That(reloadedProject.Properties[7].Value, Is.EqualTo("a3f92b1"));
        }

        [Test]
        public void LivingDocConverter_Export_Import_WithoutProperties_RoundTripsEmptyProperties()
        {
            var outputFilePath = Path.Combine(tempDirectory, "export-no-properties.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");
            livingDocConverter.Export(livingDocProject, outputFilePath);

            var reloadedProject = livingDocConverter.Import(outputFilePath);

            Assert.That(reloadedProject.Properties, Is.Not.Null);
            Assert.That(reloadedProject.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Generate_WithProperties_HtmlContainsPropertiesSection()
        {
            var outputFilePath = Path.Combine(tempDirectory, "properties.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var html = File.ReadAllText(outputFilePath);
            Assert.That(html, Does.Contain("class=\"section analytics-properties\""));
            Assert.That(html, Does.Contain("Nightly Regression"));
            Assert.That(html, Does.Contain("CI/CD Pipeline"));
        }

        [Test]
        public void LivingDocConverter_Generate_WithoutProperties_HtmlHasNoPropertiesSection()
        {
            var outputFilePath = Path.Combine(tempDirectory, "no-properties.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var html = File.ReadAllText(outputFilePath);
            Assert.That(html, Does.Not.Contain("class=\"section analytics-properties\""));
            Assert.That(html, Does.Not.Contain("Nightly Regression"));
        }
    }
}
