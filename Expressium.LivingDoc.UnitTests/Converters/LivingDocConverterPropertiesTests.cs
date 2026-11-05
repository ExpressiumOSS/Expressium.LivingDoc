using Expressium.LivingDoc.Models;
using System;
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

        private static LivingDocPropertySection CreateSection(string name, string propertyName, string propertyValue)
        {
            var section = new LivingDocPropertySection { Name = name };
            section.Properties.Add(new LivingDocProperty { Name = propertyName, Value = propertyValue });
            return section;
        }

        [Test]
        public void LivingDocProject_Constructor_InitializesEmptyProperties()
        {
            var livingDocProject = new LivingDocProject();

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
            Assert.That(livingDocProject.HasProperties(), Is.False);
        }

        [Test]
        public void LivingDocPropertySection_Constructor_InitializesEmptyProperties()
        {
            var section = new LivingDocPropertySection();

            Assert.That(section.Name, Is.Null);
            Assert.That(section.Properties, Is.Not.Null);
            Assert.That(section.Properties, Is.Empty);
        }

        [Test]
        public void LivingDocProject_HasProperties_WithPopulatedSection_IsTrue()
        {
            var livingDocProject = new LivingDocProject();
            livingDocProject.Properties.Sections.Add(CreateSection("Environment", "Environment", "Staging"));

            Assert.That(livingDocProject.HasProperties(), Is.True);
        }

        [Test]
        public void LivingDocProject_HasProperties_WithOnlyEmptySections_IsFalse()
        {
            var livingDocProject = new LivingDocProject();
            livingDocProject.Properties.Sections.Add(new LivingDocPropertySection { Name = "Empty" });

            Assert.That(livingDocProject.HasProperties(), Is.False);
        }

        [Test]
        public void LivingDocConverter_Convert_WithPropertiesPath_LoadsAllSections()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);

            var sections = livingDocProject.Properties.Sections;
            Assert.That(sections, Is.Not.Null);
            Assert.That(sections.Count, Is.EqualTo(2));
            Assert.That(sections[0].Name, Is.EqualTo("Environment"));
            Assert.That(sections[0].Properties.Count, Is.EqualTo(4));
            Assert.That(sections[0].Properties[0].Name, Is.EqualTo("Environment"));
            Assert.That(sections[0].Properties[0].Value, Is.EqualTo("Staging"));
            Assert.That(sections[0].Properties[1].Name, Is.EqualTo("Application Version"));
            Assert.That(sections[0].Properties[1].Value, Is.EqualTo("2.5.0"));
            Assert.That(sections[1].Name, Is.EqualTo("Test Run"));
            Assert.That(sections[1].Properties.Count, Is.EqualTo(4));
            Assert.That(sections[1].Properties[0].Name, Is.EqualTo("Run ID"));
            Assert.That(sections[1].Properties[0].Value, Is.EqualTo("RUN-1042"));
            Assert.That(sections[1].Properties[1].Name, Is.EqualTo("Run Name"));
            Assert.That(sections[1].Properties[1].Value, Is.EqualTo("Nightly Regression"));
            Assert.That(sections.Sum(s => s.Properties.Count), Is.EqualTo(8));
            Assert.That(livingDocProject.HasProperties(), Is.True);
        }

        [Test]
        public void LivingDocConverter_Convert_WithPropertiesPath_PreservesFileOrder()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);

            var sectionNames = livingDocProject.Properties.Sections.Select(s => s.Name).ToArray();
            Assert.That(sectionNames, Is.EqualTo(new[] { "Environment", "Test Run" }));

            var propertyNames = livingDocProject.Properties.Sections.SelectMany(s => s.Properties).Select(p => p.Name).ToArray();
            Assert.That(propertyNames, Is.EqualTo(new[]
            {
                "Environment", "Application Version", "Build ID", "Operating System",
                "Run ID", "Run Name", "Triggered By", "Git Commit"
            }));
        }

        [Test]
        public void LivingDocConverter_Convert_WithoutPropertiesPath_HasNoProperties()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
            Assert.That(livingDocProject.HasProperties(), Is.False);
        }

        [TestCase(null)]
        [TestCase("")]
        public void LivingDocConverter_Convert_WithNullOrEmptyPropertiesPath_HasNoProperties(string path)
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", path);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
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
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
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
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithEmptySectionsArray_HasEmptyProperties()
        {
            var emptyPath = Path.Combine(tempDirectory, "empty-properties.json");
            File.WriteAllText(emptyPath, "{ \"sections\": [] }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", emptyPath);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithNullSections_HasEmptyProperties()
        {
            var nullPath = Path.Combine(tempDirectory, "null-sections.json");
            File.WriteAllText(nullPath, "{ \"sections\": null }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", nullPath);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithSectionWithoutProperties_LoadsEmptySection()
        {
            var sectionPath = Path.Combine(tempDirectory, "section-without-properties.json");
            File.WriteAllText(sectionPath, "{ \"sections\": [ { \"name\": \"Environment\" } ] }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", sectionPath);

            Assert.That(livingDocProject.Properties.Sections.Count, Is.EqualTo(1));
            Assert.That(livingDocProject.Properties.Sections[0].Name, Is.EqualTo("Environment"));
            Assert.That(livingDocProject.Properties.Sections[0].Properties, Is.Empty);
            Assert.That(livingDocProject.HasProperties(), Is.False);
        }

        [Test]
        public void LivingDocConverter_Convert_WithPascalCasePropertyNames_LoadsProperties()
        {
            // Deserialization is case insensitive...
            var pascalCasePath = Path.Combine(tempDirectory, "pascal-properties.json");
            File.WriteAllText(pascalCasePath, "{ \"Sections\": [ { \"Name\": \"Environment\", \"Properties\": [ { \"Name\": \"Environment\", \"Value\": \"Production\" } ] } ] }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", pascalCasePath);

            Assert.That(livingDocProject.Properties.Sections.Count, Is.EqualTo(1));
            Assert.That(livingDocProject.Properties.Sections[0].Name, Is.EqualTo("Environment"));
            Assert.That(livingDocProject.Properties.Sections[0].Properties.Count, Is.EqualTo(1));
            Assert.That(livingDocProject.Properties.Sections[0].Properties[0].Name, Is.EqualTo("Environment"));
            Assert.That(livingDocProject.Properties.Sections[0].Properties[0].Value, Is.EqualTo("Production"));
        }

        [Test]
        public void LivingDocConverter_Convert_WithUnrelatedJsonFile_HasNoProperties()
        {
            // A valid JSON object without any sections...
            var unrelatedPath = Path.Combine(tempDirectory, "unrelated.json");
            File.WriteAllText(unrelatedPath, "{ \"environment\": \"Staging\" }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", unrelatedPath);

            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Convert_WithLegacyFlatPropertiesArray_HasNoProperties()
        {
            // The former flat [ { "name": ..., "value": ... } ] format is no longer supported...
            var legacyPath = Path.Combine(tempDirectory, "legacy-flat-properties.json");
            File.WriteAllText(legacyPath, "[ { \"name\": \"Environment\", \"value\": \"Staging\" } ]");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", legacyPath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.GreaterThan(0));
            Assert.That(livingDocProject.Properties, Is.Not.Null);
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
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
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
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
            Assert.That(livingDocProject.Properties.Sections.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.Properties.Sections[1].Properties[0].Value, Is.EqualTo("RUN-1042"));
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
            Assert.That(livingDocProject.Properties.Sections, Is.Empty);
        }

        [Test]
        public void LivingDocProject_Merge_DoesNotOverwriteMasterProperties()
        {
            var master = new LivingDocProject();
            master.Properties.Sections.Add(CreateSection("Environment", "Environment", "Staging"));

            var slave = new LivingDocProject();
            slave.Properties.Sections.Add(CreateSection("Environment", "Environment", "Production"));

            master.Merge(slave);

            Assert.That(master.Properties.Sections.Count, Is.EqualTo(1));
            Assert.That(master.Properties.Sections[0].Properties.Count, Is.EqualTo(1));
            Assert.That(master.Properties.Sections[0].Properties[0].Value, Is.EqualTo("Staging"));
        }

        [Test]
        public void LivingDocProject_Merge_DoesNotAddSlavePropertiesToEmptyMaster()
        {
            var master = new LivingDocProject();

            var slave = new LivingDocProject();
            slave.Properties.Sections.Add(CreateSection("Environment", "Environment", "Production"));

            master.Merge(slave);

            Assert.That(master.Properties.Sections, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_MergeHistory_KeepsMasterProperties()
        {
            var historyPath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "History", "coffeeshop.ndjson");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.MergeHistory(livingDocProject, historyPath);

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(4));
            Assert.That(livingDocProject.Properties.Sections.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.Properties.Sections.Sum(s => s.Properties.Count), Is.EqualTo(8));
        }

        [Test]
        public void LivingDocConverter_Export_Import_RoundTripsProperties()
        {
            var outputFilePath = Path.Combine(tempDirectory, "export-properties.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.Export(livingDocProject, outputFilePath);

            var reloadedProject = livingDocConverter.Import(outputFilePath);

            Assert.That(reloadedProject.Properties.Sections.Count, Is.EqualTo(2));
            Assert.That(reloadedProject.Properties.Sections[1].Name, Is.EqualTo("Test Run"));
            Assert.That(reloadedProject.Properties.Sections[1].Properties[3].Name, Is.EqualTo("Git Commit"));
            Assert.That(reloadedProject.Properties.Sections[1].Properties[3].Value, Is.EqualTo("a3f92b1"));
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
            Assert.That(reloadedProject.Properties.Sections, Is.Empty);
        }

        [Test]
        public void LivingDocConverter_Generate_WithProperties_HtmlContainsPropertiesSections()
        {
            var outputFilePath = Path.Combine(tempDirectory, "properties.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var html = File.ReadAllText(outputFilePath);
            Assert.That(html, Does.Contain("id=\"properties\""));
            Assert.That(html, Does.Contain("<!-- Data Properties -->"));
            Assert.That(html, Does.Contain("<!-- Data Properties Section -->"));
            Assert.That(html, Does.Contain("class=\"section properties-section\""));
            Assert.That(html, Does.Contain("class=\"properties-list\""));
            Assert.That(html, Does.Contain("Test Run"));
            Assert.That(html, Does.Contain("Application Version"));
            Assert.That(html, Does.Contain("build-458"));
            Assert.That(html, Does.Contain("Nightly Regression"));
            Assert.That(html, Does.Contain("CI/CD Pipeline"));
        }

        [Test]
        public void LivingDocConverter_Generate_WithProperties_HtmlContainsOneTablePerSection()
        {
            var outputFilePath = Path.Combine(tempDirectory, "properties-sections.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var html = File.ReadAllText(outputFilePath);
            var numberOfTables = html.Split(new[] { "class=\"properties-list\"" }, StringSplitOptions.None).Length - 1;
            Assert.That(numberOfTables, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocConverter_Generate_WithProperties_HtmlContainsPropertiesNavigationLink()
        {
            var outputFilePath = Path.Combine(tempDirectory, "properties-navigation.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", propertiesFilePath);
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var html = File.ReadAllText(outputFilePath);
            Assert.That(html, Does.Contain("onclick=\"loadProperties()\""));
        }

        [Test]
        public void LivingDocConverter_Generate_WithoutProperties_HtmlHasNoPropertiesSection()
        {
            var outputFilePath = Path.Combine(tempDirectory, "no-properties.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle");
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var html = File.ReadAllText(outputFilePath);
            Assert.That(html, Does.Not.Contain("onclick=\"loadProperties()\""));
            Assert.That(html, Does.Not.Contain("<!-- Data Properties Section -->"));
            Assert.That(html, Does.Not.Contain("Nightly Regression"));
            Assert.That(html, Does.Not.Contain("CI/CD Pipeline"));
        }

        [Test]
        public void LivingDocConverter_Convert_WithSpecialCharacterProperties_EncodesOnLoad()
        {
            var specialPath = Path.Combine(tempDirectory, "special-load-properties.json");
            File.WriteAllText(specialPath, "{ \"sections\": [ { \"name\": \"<i>Build</i> & Run\", \"properties\": [ { \"name\": \"<b>Owner</b>\", \"value\": \"<script>alert('x')</script> & more\" } ] } ] }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", specialPath);

            var section = livingDocProject.Properties.Sections[0];
            Assert.That(section.Name, Is.EqualTo("&lt;i&gt;Build&lt;/i&gt; &amp; Run"));
            Assert.That(section.Properties[0].Name, Is.EqualTo("&lt;b&gt;Owner&lt;/b&gt;"));
            Assert.That(section.Properties[0].Value, Is.EqualTo("&lt;script&gt;alert(&#39;x&#39;)&lt;/script&gt; &amp; more"));
        }

        [Test]
        public void LivingDocConverter_Convert_WithNullPropertyValue_KeepsNull()
        {
            var nullValuePath = Path.Combine(tempDirectory, "null-value-properties.json");
            File.WriteAllText(nullValuePath, "{ \"sections\": [ { \"name\": \"Environment\", \"properties\": [ { \"name\": \"Browser\", \"value\": null } ] } ] }");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", nullValuePath);

            Assert.That(livingDocProject.Properties.Sections[0].Properties[0].Name, Is.EqualTo("Browser"));
            Assert.That(livingDocProject.Properties.Sections[0].Properties[0].Value, Is.Null);
        }

        [Test]
        public void LivingDocConverter_Generate_WithSpecialCharacterProperties_HtmlIsEncoded()
        {
            var specialPath = Path.Combine(tempDirectory, "special-properties.json");
            File.WriteAllText(specialPath, "{ \"sections\": [ { \"name\": \"<i>Build</i>\", \"properties\": [ { \"name\": \"<b>Owner</b>\", \"value\": \"<script>alert('x')</script>\" } ] } ] }");

            var outputFilePath = Path.Combine(tempDirectory, "special-properties.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "MyProjectTitle", specialPath);
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var html = File.ReadAllText(outputFilePath);
            Assert.That(html, Does.Not.Contain("<script>alert('x')</script>"));
            Assert.That(html, Does.Contain("&lt;script&gt;"));
            Assert.That(html, Does.Contain("&lt;b&gt;Owner&lt;/b&gt;"));
            Assert.That(html, Does.Contain("&lt;i&gt;Build&lt;/i&gt;"));
        }
    }
}
