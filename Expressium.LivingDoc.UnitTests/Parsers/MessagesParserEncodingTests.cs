using AngleSharp.Html.Parser;
using Expressium.LivingDoc.Parsers;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;

namespace Expressium.LivingDoc.UnitTests.Parsers
{
    /// <summary>
    /// All Gherkin text and test results are HTML encoded once by the parser,
    /// so the generators can write them to the report as is.
    /// </summary>
    internal class MessagesParserEncodingTests
    {
        private string tempDirectory;

        [SetUp]
        public void SetUp()
        {
            tempDirectory = Path.Combine(Path.GetTempPath(), "MessagesParserEncodingTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }

        private static string GetSamplePath(string name)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "CCK", "Samples", name, name + ".ndjson");
        }

        private string CreateModifiedSample(string name, Action<JsonObject> modifyEnvelope)
        {
            var lines = File.ReadAllLines(GetSamplePath(name))
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var envelope = JsonNode.Parse(line).AsObject();
                    modifyEnvelope(envelope);
                    return envelope.ToJsonString();
                });

            var filePath = Path.Combine(tempDirectory, name + ".ndjson");
            File.WriteAllLines(filePath, lines);
            return filePath;
        }

        private static JsonObject GetFirstScenario(JsonObject envelope)
        {
            return envelope["gherkinDocument"]["feature"]["children"].AsArray()
                .Select(child => child["scenario"])
                .First(scenario => scenario != null)
                .AsObject();
        }

        private string CreateMinimalWithSpecialCharacters()
        {
            return CreateModifiedSample("minimal", envelope =>
            {
                if (envelope["gherkinDocument"] != null)
                {
                    var feature = envelope["gherkinDocument"]["feature"];
                    feature["name"] = "Tom's <Feature> & Co";
                    feature["description"] = "Uses <b>bold</b> & \"quotes\"";

                    var scenario = GetFirstScenario(envelope);
                    scenario["name"] = "User's <script>alert(1)</script> login";
                    scenario["description"] = "It's <i>important</i>";
                    scenario["tags"].AsArray().Add(new JsonObject
                    {
                        ["location"] = new JsonObject { ["line"] = 8, ["column"] = 3 },
                        ["name"] = "@team<a&b>",
                        ["id"] = "tag-1"
                    });
                    scenario["steps"][0]["text"] = "I have <42> cukes & 'more'";
                }

                if (envelope["pickle"] != null)
                    envelope["pickle"]["steps"][0]["text"] = "I have <42> cukes & 'more'";
            });
        }

        [Test]
        public void MessagesParser_EncodesFeatureNameAndDescription()
        {
            var livingDocProject = new MessagesParser().ConvertToLivingDoc(CreateMinimalWithSpecialCharacters());
            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("Tom&#39;s &lt;Feature&gt; &amp; Co"));
            Assert.That(feature.Description, Is.EqualTo("Uses &lt;b&gt;bold&lt;/b&gt; &amp; &quot;quotes&quot;"));
        }

        [Test]
        public void MessagesParser_EncodesScenarioNameDescriptionAndTags()
        {
            var livingDocProject = new MessagesParser().ConvertToLivingDoc(CreateMinimalWithSpecialCharacters());
            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("User&#39;s &lt;script&gt;alert(1)&lt;/script&gt; login"));
            Assert.That(scenario.Description, Is.EqualTo("It&#39;s &lt;i&gt;important&lt;/i&gt;"));
            Assert.That(scenario.Tags, Is.EqualTo(new[] { "@team&lt;a&amp;b&gt;" }));
        }

        [Test]
        public void MessagesParser_EncodesStepText()
        {
            var livingDocProject = new MessagesParser().ConvertToLivingDoc(CreateMinimalWithSpecialCharacters());
            var step = livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0];

            Assert.That(step.Name, Is.EqualTo("I have &lt;42&gt; cukes &amp; &#39;more&#39;"));
            Assert.That(step.Status, Is.EqualTo("Passed"));
        }

        [Test]
        public void MessagesParser_PlainText_IsUnchanged()
        {
            var livingDocProject = new MessagesParser().ConvertToLivingDoc(GetSamplePath("minimal"));
            var feature = livingDocProject.Features[0];

            Assert.That(feature.Name, Is.EqualTo("minimal"));
            Assert.That(feature.Scenarios[0].Name, Is.EqualTo("cukes"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have 42 cukes in my belly"));
        }

        [Test]
        public void MessagesParser_EncodesDataTableCells()
        {
            var inputFilePath = CreateModifiedSample("data-tables", envelope =>
            {
                if (envelope["gherkinDocument"] == null)
                    return;

                var step = GetFirstScenario(envelope)["steps"].AsArray().First(s => s["dataTable"] != null);
                step["dataTable"]["rows"][0]["cells"][0]["value"] = "<a & b>";
            });

            var livingDocProject = new MessagesParser().ConvertToLivingDoc(inputFilePath);
            var step = livingDocProject.Features[0].Scenarios[0].Examples[0].Steps.First(s => s.DataTable.Rows.Count > 0);

            Assert.That(step.DataTable.Rows[0].Cells[0], Is.EqualTo("&lt;a &amp; b&gt;"));
            Assert.That(step.DataTable.Rows[0].Cells[1], Is.EqualTo("b"));
        }

        [Test]
        public void MessagesParser_EncodesRuleNameAndDescription()
        {
            var inputFilePath = CreateModifiedSample("rules", envelope =>
            {
                if (envelope["gherkinDocument"] == null)
                    return;

                var rule = envelope["gherkinDocument"]["feature"]["children"].AsArray()
                    .Select(child => child["rule"])
                    .First(r => r != null);

                rule["name"] = "Customer's <rule>";
                rule["description"] = "A & B";
            });

            var livingDocProject = new MessagesParser().ConvertToLivingDoc(inputFilePath);
            var rule = livingDocProject.Features[0].Rules[0];

            Assert.That(rule.Name, Is.EqualTo("Customer&#39;s &lt;rule&gt;"));
            Assert.That(rule.Description, Is.EqualTo("A &amp; B"));
        }

        [Test]
        public void MessagesParser_EncodesExceptionTypeAndStackTrace()
        {
            var inputFilePath = CreateModifiedSample("stack-traces", envelope =>
            {
                var exception = envelope["testStepFinished"]?["testStepResult"]?["exception"];
                if (exception == null)
                    return;

                exception["type"] = "System.Collections.Generic.KeyNotFoundException<string>";
                exception["stackTrace"] = "at Dictionary<string, int>.get_Item()\nat Steps.When_I_pay()";
            });

            var livingDocProject = new MessagesParser().ConvertToLivingDoc(inputFilePath);
            var step = livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0];

            Assert.That(step.ExceptionType, Is.EqualTo("System.Collections.Generic.KeyNotFoundException&lt;string&gt;"));
            Assert.That(step.ExceptionStackTrace, Is.EqualTo("at Dictionary&lt;string, int&gt;.get_Item()\nat Steps.When_I_pay()"));
        }

        [Test]
        public void MessagesParser_EncodedNames_StillMatchWhenMergingHistory()
        {
            var inputFilePath = CreateMinimalWithSpecialCharacters();

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "Title");
            var previousRun = livingDocConverter.Convert(inputFilePath, null);
            previousRun.Date = previousRun.Date.AddDays(-1);

            livingDocProject.MergeHistory(previousRun);

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].History.Count, Is.EqualTo(1));
        }

        [Test]
        public void MessagesParser_EncodesFeatureUri()
        {
            var inputFilePath = CreateModifiedSample("minimal", envelope =>
            {
                if (envelope["gherkinDocument"] != null)
                    envelope["gherkinDocument"]["uri"] = "Features/Tom's & <Team>/minimal.feature";
            });

            var livingDocProject = new MessagesParser().ConvertToLivingDoc(inputFilePath);
            var feature = livingDocProject.Features[0];

            Assert.That(feature.Uri, Is.EqualTo("Features/Tom&#39;s &amp; &lt;Team&gt;/minimal.feature"));
            Assert.That(feature.GetFolder(), Is.EqualTo("Features\\Tom&#39;s &amp; &lt;Team&gt;"));
        }

        [Test]
        public void LivingDocConverter_Convert_EncodesTitle()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(GetSamplePath("minimal"), "Tom's <Report> & Co");

            Assert.That(livingDocProject.Title, Is.EqualTo("Tom&#39;s &lt;Report&gt; &amp; Co"));
        }

        [Test]
        public void LivingDocConverter_Convert_WithoutTitle_KeepsDefaultTitle()
        {
            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(GetSamplePath("minimal"), null);

            Assert.That(livingDocProject.Title, Is.EqualTo("Expressium LivingDoc"));
        }

        [Test]
        public void LivingDocConverter_Generate_SpecialCharactersInTitleAndUri_RenderAsText()
        {
            var inputFilePath = CreateModifiedSample("minimal", envelope =>
            {
                if (envelope["gherkinDocument"] != null)
                    envelope["gherkinDocument"]["uri"] = "Features/Tom's & <Team>/minimal.feature";
            });
            var outputFilePath = Path.Combine(tempDirectory, "title.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath, "Tom's <Report> & Co");
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var document = new HtmlParser().ParseDocument(File.ReadAllText(outputFilePath));

            Assert.That(document.QuerySelector("header .project-name").TextContent, Is.EqualTo("Tom's <Report> & Co"));
            Assert.That(document.QuerySelectorAll("Report"), Is.Empty);

            var folderNames = document.QuerySelectorAll("#project-view tr[data-role='folder']").Select(r => r.GetAttribute("data-name"));
            Assert.That(folderNames, Is.EqualTo(new[] { "Features", "Features\\Tom's & <Team>" }));

            var folderRow = document.QuerySelectorAll("#project-view tr[data-role='folder']").Last();
            Assert.That(folderRow.TextContent, Does.Contain("Tom's & <Team>"));

            var scenarioRow = document.QuerySelectorAll("#project-view tr[data-role='scenario']").Single();
            Assert.That(scenarioRow.GetAttribute("data-tags"), Does.Contain("Features/Tom's & <Team>/minimal.feature"));
            Assert.That(scenarioRow.GetAttribute("onclick"), Is.EqualTo("loadScenario(this);"));
        }

        [Test]
        public void LivingDocConverter_Generate_SpecialCharacters_RenderAsText()
        {
            var outputFilePath = Path.Combine(tempDirectory, "encoding.html");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(CreateMinimalWithSpecialCharacters(), "Title");
            livingDocConverter.Generate(livingDocProject, outputFilePath);

            var document = new HtmlParser().ParseDocument(File.ReadAllText(outputFilePath));

            // No markup is injected by names or step text...
            Assert.That(document.QuerySelectorAll("script").Any(s => s.TextContent.Contains("alert(1)")), Is.False);
            Assert.That(document.QuerySelectorAll("b").Any(b => b.TextContent == "bold"), Is.False);

            // Apostrophes no longer break the single quoted data-tags attribute...
            var scenarioRow = document.QuerySelectorAll("#project-view tr[data-role='scenario']").Single();
            Assert.That(scenarioRow.GetAttribute("data-tags"), Does.Contain("Tom's <Feature> & Co"));
            Assert.That(scenarioRow.GetAttribute("data-tags"), Does.Contain("@team<a&b>"));
            Assert.That(scenarioRow.GetAttribute("onclick"), Is.EqualTo("loadScenario(this);"));

            // Text is displayed as written in the feature file...
            Assert.That(scenarioRow.TextContent, Does.Contain("User's <script>alert(1)</script> login"));
            Assert.That(document.Body.TextContent, Does.Contain("I have <42> cukes & 'more'"));
        }
    }
}
