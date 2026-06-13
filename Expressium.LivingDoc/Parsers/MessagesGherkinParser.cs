using Expressium.LivingDoc.Models;
using Io.Cucumber.Messages.Types;
using System.Net;

namespace Expressium.LivingDoc.Parsers
{
    internal class MessagesGherkinParser
    {
        internal void ParseGherkinDocuments(CucumberMessages messages, LivingDocProject livingDocProject)
        {
            foreach (var gherkinDocument in messages.GherkinDocuments)
            {
                var uri = gherkinDocument.Uri;
                var feature = gherkinDocument.Feature;

                var livingDocFeature = new LivingDocFeature();

                ParseFeature(livingDocFeature, feature, uri);

                foreach (var child in feature.Children)
                {
                    if (child.Background != null)
                        ParseBackground(livingDocFeature, child.Background);

                    if (child.Rule != null)
                        ParseRule(livingDocFeature, child.Rule);

                    if (child.Scenario != null)
                        ParseScenario(livingDocFeature, child.Scenario);
                }

                livingDocProject.Features.Add(livingDocFeature);
            }
        }

        internal static void ParseFeature(LivingDocFeature livingDocFeature, Feature feature, string uri)
        {
            if (feature.Tags != null)
            {
                foreach (var tag in feature.Tags)
                    livingDocFeature.Tags.Add(tag.Name);
            }

            livingDocFeature.Description = feature.Description;
            livingDocFeature.Uri = uri;
            livingDocFeature.Name = feature.Name;
            livingDocFeature.Keyword = feature.Keyword;
        }

        internal static void ParseBackground(LivingDocFeature livingDocFeature, Background background)
        {
            var livingDocBackground = new LivingDocBackground();

            livingDocBackground.Id = background.Id;
            livingDocBackground.Description = background.Description;
            livingDocBackground.Name = background.Name;
            livingDocBackground.Keyword = background.Keyword;

            foreach (var step in background.Steps)
            {
                var livingDocStep = new LivingDocStep();
                livingDocStep.Name = WebUtility.HtmlEncode(step.Text);
                livingDocStep.Keyword = step.Keyword.Trim();
                livingDocStep.Id = step.Id;
                livingDocStep.Type = LivingDocStepTypes.Background.ToString();
                livingDocBackground.Steps.Add(livingDocStep);
            }

            livingDocFeature.Background = livingDocBackground;
        }

        internal static void ParseRule(LivingDocFeature livingDocFeature, Rule rule)
        {
            var livingDocRule = new LivingDocRule();

            if (rule.Tags != null)
            {
                foreach (var tag in rule.Tags)
                    livingDocRule.Tags.Add(tag.Name);
            }

            livingDocRule.Id = rule.Id;
            livingDocRule.Description = rule.Description;
            livingDocRule.Name = rule.Name;
            livingDocRule.Keyword = rule.Keyword;

            livingDocFeature.Rules.Add(livingDocRule);

            foreach (var ruleChild in rule.Children)
            {
                if (ruleChild.Scenario == null)
                    continue;

                ParseScenario(livingDocFeature, ruleChild.Scenario, rule.Id);
            }
        }

        internal static void ParseScenario(LivingDocFeature livingDocFeature, Scenario scenario, string ruleId = null)
        {
            var livingDocScenario = new LivingDocScenario();

            if (scenario.Tags != null)
            {
                foreach (var tag in scenario.Tags)
                    livingDocScenario.Tags.Add(tag.Name);
            }

            livingDocScenario.RuleId = ruleId;
            livingDocScenario.Id = scenario.Id;
            livingDocScenario.Description = scenario.Description;
            livingDocScenario.Name = scenario.Name;
            livingDocScenario.Keyword = scenario.Keyword;

            livingDocFeature.Scenarios.Add(livingDocScenario);

            if (scenario.Examples.Count > 0)
            {
                // Consolidating Examples as Self-Contained Scenarios...
                foreach (var example in scenario.Examples)
                {
                    int tableIndexId = 1;
                    foreach (var tableBodyRow in example.TableBody)
                    {
                        var livingDocExample = new LivingDocExample();
                        livingDocExample.Name = example.Name;
                        livingDocExample.Description = example.Description;
                        livingDocScenario.Examples.Add(livingDocExample);

                        ParseScenarioBackgroundSteps(livingDocExample, livingDocFeature, tableIndexId++);
                        ParseScenarioExampleTableSteps(livingDocExample, scenario, tableBodyRow.Id);
                        ParseScenarioExampleTableHeaders(livingDocExample, example);
                        ParseScenarioExampleTableData(livingDocExample, tableBodyRow);
                    }
                }
            }
            else
            {
                var livingDocExample = new LivingDocExample();
                livingDocScenario.Examples.Add(livingDocExample);

                ParseScenarioBackgroundSteps(livingDocExample, livingDocFeature);
                ParseScenarioExampleSteps(livingDocExample, scenario);
            }
        }

        internal static void ParseScenarioBackgroundSteps(LivingDocExample livingDocExample, LivingDocFeature livingDocFeature, int tableIndexId = -1)
        {
            if (livingDocFeature.Background != null)
            {
                foreach (var backgroundStep in livingDocFeature.Background.Steps)
                {
                    var copy = backgroundStep.Copy(backgroundStep);
                    copy.TableIndexId = tableIndexId;
                    livingDocExample.Steps.Add(copy);
                }
            }
        }

        internal static void ParseScenarioExampleSteps(LivingDocExample livingDocExample, Scenario scenario)
        {
            foreach (var step in scenario.Steps)
            {
                var livingDocStep = new LivingDocStep();
                livingDocStep.Id = step.Id;
                livingDocStep.Name = WebUtility.HtmlEncode(step.Text);
                livingDocStep.Keyword = step.Keyword.Trim();

                ParseStepDataTable(livingDocStep, step);

                livingDocExample.Steps.Add(livingDocStep);
            }
        }

        internal static void ParseScenarioExampleTableSteps(LivingDocExample livingDocExample, Scenario scenario, string tableBodyRowId)
        {
            foreach (var step in scenario.Steps)
            {
                var livingDocStep = new LivingDocStep();
                livingDocStep.Id = step.Id;
                livingDocStep.TableBodyId = tableBodyRowId;
                livingDocStep.Name = WebUtility.HtmlEncode(step.Text);
                livingDocStep.Type = LivingDocStepTypes.Scenario.ToString();
                livingDocStep.Keyword = step.Keyword.Trim();

                ParseStepDataTable(livingDocStep, step);

                livingDocExample.Steps.Add(livingDocStep);
            }
        }

        private static void ParseStepDataTable(LivingDocStep livingDocStep, Step step)
        {
            if (step.DataTable == null)
                return;

            foreach (var row in step.DataTable.Rows)
            {
                var dataTableRow = new LivingDocDataTableRow();
                foreach (var cell in row.Cells)
                    dataTableRow.Cells.Add(cell.Value);
                livingDocStep.DataTable.Rows.Add(dataTableRow);
            }
        }

        internal static void ParseScenarioExampleTableHeaders(LivingDocExample livingDocExample, Examples examples)
        {
            var dataTableRow = new LivingDocDataTableRow();
            foreach (var tableHeaderRowCell in examples.TableHeader.Cells)
                dataTableRow.Cells.Add(tableHeaderRowCell.Value);
            livingDocExample.DataTable.Rows.Add(dataTableRow);
        }

        internal static void ParseScenarioExampleTableData(LivingDocExample livingDocExample, TableRow tableBodyRow)
        {
            var dataTableRow = new LivingDocDataTableRow();
            foreach (var tableBodyRowCell in tableBodyRow.Cells)
                dataTableRow.Cells.Add(tableBodyRowCell.Value);
            livingDocExample.DataTable.Rows.Add(dataTableRow);
        }
    }
}
