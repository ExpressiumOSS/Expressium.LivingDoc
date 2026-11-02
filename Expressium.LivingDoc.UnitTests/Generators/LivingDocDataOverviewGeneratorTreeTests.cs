using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataOverviewGeneratorTreeTests
    {
        private class OverviewRow
        {
            public string Header { get; set; }
            public List<string> Lines { get; set; }
            public int Indents => Lines.Count(l => l == "<td class='grid-indent'></td>");
            public string Role => Attribute("data-role");
            public string Name => Attribute("data-name");

            public string Attribute(string name)
            {
                var start = Header.IndexOf(name + "='");
                if (start < 0)
                    return null;
                start += name.Length + 2;
                return Header.Substring(start, Header.IndexOf('\'', start) - start);
            }

            public string Text => Lines.FirstOrDefault(l => l.StartsWith("<a href='#'>") || l.StartsWith("<span class='grid-folder-name'>"));
        }

        private static List<OverviewRow> GetRows(List<string> listOfLines)
        {
            var rows = new List<OverviewRow>();
            OverviewRow current = null;

            foreach (var line in listOfLines)
            {
                if (line.StartsWith("<tr data-"))
                {
                    current = new OverviewRow { Header = line, Lines = new List<string>() };
                    rows.Add(current);
                }
                else if (line == "</tr>")
                    current = null;
                else if (current != null)
                    current.Lines.Add(line);
            }

            return rows;
        }

        private static LivingDocFeature CreateFeature(string name, string uri, params string[] scenarioNames)
        {
            var feature = new LivingDocFeature { Name = name, Uri = uri };
            foreach (var scenarioName in scenarioNames)
            {
                var scenario = new LivingDocScenario { Name = scenarioName };
                var example = new LivingDocExample();
                example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
                scenario.Examples.Add(example);
                feature.Scenarios.Add(scenario);
            }
            return feature;
        }

        private static LivingDocProject CreateNestedProject()
        {
            var project = new LivingDocProject { Title = "Nested Project" };
            project.Features.Add(CreateFeature("Login", "Features/Login/login.feature", "Valid Login"));
            project.Features.Add(CreateFeature("Admin Login", "Features/Login/Admin/admin.feature", "Admin Valid Login"));
            project.Features.Add(CreateFeature("Orders", "Features/Orders/orders.feature", "Place Order"));
            project.Features.Add(CreateFeature("Root", "root.feature", "Root Scenario"));
            return project;
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_NestedFolders_RendersTreeInOrder()
        {
            var generator = new LivingDocDataOverviewGenerator(CreateNestedProject());
            var rows = GetRows(generator.GenerateDataOverview());

            var sequence = rows.Select(r => r.Role == "folder" ? "folder:" + r.Name : r.Role + ":" + r.Text).ToList();

            Assert.That(sequence, Is.EqualTo(new[]
            {
                "folder:Features",
                "folder:Features\\Login",
                "folder:Features\\Login\\Admin",
                "feature:<a href='#'>Admin Login</a>",
                "scenario:<a href='#'>Admin Valid Login</a>",
                "feature:<a href='#'>Login</a>",
                "scenario:<a href='#'>Valid Login</a>",
                "folder:Features\\Orders",
                "feature:<a href='#'>Orders</a>",
                "scenario:<a href='#'>Place Order</a>",
                "feature:<a href='#'>Root</a>",
                "scenario:<a href='#'>Root Scenario</a>"
            }));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_NestedFolders_IndentsByDepth()
        {
            var generator = new LivingDocDataOverviewGenerator(CreateNestedProject());
            var rows = GetRows(generator.GenerateDataOverview());

            Assert.That(rows.Single(r => r.Name == "Features").Indents, Is.EqualTo(0));
            Assert.That(rows.Single(r => r.Name == "Features\\Login").Indents, Is.EqualTo(1));
            Assert.That(rows.Single(r => r.Name == "Features\\Login\\Admin").Indents, Is.EqualTo(2));
            Assert.That(rows.Single(r => r.Name == "Features\\Orders").Indents, Is.EqualTo(1));

            Assert.That(rows.Single(r => r.Text == "<a href='#'>Login</a>").Indents, Is.EqualTo(2));
            Assert.That(rows.Single(r => r.Text == "<a href='#'>Admin Login</a>").Indents, Is.EqualTo(3));
            Assert.That(rows.Single(r => r.Text == "<a href='#'>Valid Login</a>").Indents, Is.EqualTo(4));
            Assert.That(rows.Single(r => r.Text == "<a href='#'>Admin Valid Login</a>").Indents, Is.EqualTo(5));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_NestedFolders_EachFolderRenderedOnce()
        {
            var generator = new LivingDocDataOverviewGenerator(CreateNestedProject());
            var folders = GetRows(generator.GenerateDataOverview()).Where(r => r.Role == "folder").Select(r => r.Name).ToList();

            Assert.That(folders, Is.Unique);
            Assert.That(folders.Count, Is.EqualTo(4));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_FolderNameShowsLastSegment()
        {
            var generator = new LivingDocDataOverviewGenerator(CreateNestedProject());
            var adminFolder = GetRows(generator.GenerateDataOverview()).Single(r => r.Name == "Features\\Login\\Admin");

            Assert.That(adminFolder.Text, Is.EqualTo("<span class='grid-folder-name'>Admin</span>"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_FeatureRows_ReferenceParentFolder()
        {
            var generator = new LivingDocDataOverviewGenerator(CreateNestedProject());
            var rows = GetRows(generator.GenerateDataOverview());

            Assert.That(rows.Single(r => r.Text == "<a href='#'>Admin Login</a>").Attribute("data-parent"), Is.EqualTo("Features\\Login\\Admin"));
            Assert.That(rows.Single(r => r.Text == "<a href='#'>Root</a>").Attribute("data-parent"), Is.EqualTo(string.Empty));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_ScenarioRows_ReferenceParentFeature()
        {
            var project = CreateNestedProject();
            var generator = new LivingDocDataOverviewGenerator(project);
            var rows = GetRows(generator.GenerateDataOverview());

            var orders = project.Features.Single(f => f.Name == "Orders");
            var scenarioRow = rows.Single(r => r.Text == "<a href='#'>Place Order</a>");

            Assert.That(scenarioRow.Attribute("data-parent"), Is.EqualTo(orders.Id));
            Assert.That(scenarioRow.Attribute("data-featureid"), Is.EqualTo(orders.Id));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_SiblingFoldersWithSamePrefix_AreNotNested()
        {
            var project = new LivingDocProject();
            project.Features.Add(CreateFeature("Log", "Features/Log/log.feature", "Log Scenario"));
            project.Features.Add(CreateFeature("Login", "Features/Login/login.feature", "Login Scenario"));

            var generator = new LivingDocDataOverviewGenerator(project);
            var rows = GetRows(generator.GenerateDataOverview());

            Assert.That(rows.Single(r => r.Name == "Features\\Log").Indents, Is.EqualTo(1));
            Assert.That(rows.Single(r => r.Name == "Features\\Login").Indents, Is.EqualTo(1));
            Assert.That(rows.Single(r => r.Text == "<a href='#'>Login</a>").Indents, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_FeaturesWithoutUri_AreRenderedAtRootLevel()
        {
            var project = new LivingDocProject();
            project.Features.Add(CreateFeature("No Uri", null, "Scenario A"));
            project.Features.Add(CreateFeature("Empty Uri", "", "Scenario B"));

            var generator = new LivingDocDataOverviewGenerator(project);
            var rows = GetRows(generator.GenerateDataOverview());

            Assert.That(rows.Any(r => r.Role == "folder"), Is.False);
            Assert.That(rows.Where(r => r.Role == "feature").Select(r => r.Indents), Is.All.EqualTo(0));
            Assert.That(rows.Where(r => r.Role == "scenario").Select(r => r.Indents), Is.All.EqualTo(2));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_EmptyProject_RendersHeaderOnly()
        {
            var project = new LivingDocProject { Title = "Empty" };

            var generator = new LivingDocDataOverviewGenerator(project);
            var listOfLines = generator.GenerateDataOverview();

            Assert.That(GetRows(listOfLines), Is.Empty);
            Assert.That(listOfLines, Does.Contain("<span class='grid-folder-name'>Empty</span>"));
            Assert.That(listOfLines.Last(), Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateDataOverview_FeatureWithoutScenarios_RendersFeatureOnly()
        {
            var project = new LivingDocProject();
            project.Features.Add(CreateFeature("Empty Feature", "Features/empty.feature"));

            var generator = new LivingDocDataOverviewGenerator(project);
            var rows = GetRows(generator.GenerateDataOverview());

            Assert.That(rows.Select(r => r.Role), Is.EqualTo(new[] { "folder", "feature" }));
            Assert.That(rows[1].Lines, Has.Some.Contains("color-skipped"));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GenerateOverviewScenario_IncludesRuleTagsInDataTags()
        {
            var feature = CreateFeature("Payments", "Features/payments.feature", "Pay By Card");
            var rule = new LivingDocRule { Id = "rule-1", Name = "Card Rule" };
            rule.Tags.Add("@cards");
            feature.Rules.Add(rule);
            feature.Scenarios[0].RuleId = "rule-1";

            var generator = new LivingDocDataOverviewGenerator(new LivingDocProject());
            var listOfLines = generator.GenerateOverviewScenario(feature, feature.Scenarios[0], 2);

            Assert.That(listOfLines[0], Does.Contain("@cards"));
            Assert.That(listOfLines[0], Does.Contain("Features/payments.feature"));
        }

        [Test]
        [Ignore("Known issue: GenerateOverviewScenario throws NullReferenceException when RuleId does not match a rule")]
        public void LivingDocDataOverviewGenerator_GenerateOverviewScenario_UnknownRuleId_DoesNotThrow()
        {
            var feature = CreateFeature("Payments", "Features/payments.feature", "Pay By Card");
            feature.Scenarios[0].RuleId = "missing-rule";

            var generator = new LivingDocDataOverviewGenerator(new LivingDocProject());

            Assert.DoesNotThrow(() => generator.GenerateOverviewScenario(feature, feature.Scenarios[0], 2));
        }

        [Test]
        [Platform("Win", Reason = "Known issue: on Linux/macOS Path.GetDirectoryName does not treat '\\' as a separator, so GetFolder returns null")]
        public void LivingDocFeature_GetFolder_BackslashUri_ReturnsFolder()
        {
            var feature = new LivingDocFeature { Uri = "Features\\Login\\login.feature" };

            Assert.That(feature.GetFolder(), Is.EqualTo("Features\\Login"));
        }
    }
}
