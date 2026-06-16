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
            var livingDocBackground = new LivingDocBackground
            {
                Id = background.Id,
                Description = background.Description,
                Name = background.Name,
                Keyword = background.Keyword
            };

            foreach (var step in background.Steps)
            {
                var livingDocStep = new LivingDocStep
                {
                    Name = WebUtility.HtmlEncode(step.Text),
                    Keyword = step.Keyword.Trim(),
                    Id = step.Id,
                    Type = LivingDocStepTypes.Background.ToString()
                };
                livingDocBackground.Steps.Add(livingDocStep);
            }

            livingDocFeature.Background = livingDocBackground;
        }

        internal static void ParseRule(LivingDocFeature livingDocFeature, Rule rule)
        {
            var livingDocRule = new LivingDocRule
            {
                Id = rule.Id,
                Description = rule.Description,
                Name = rule.Name,
                Keyword = rule.Keyword
            };

            if (rule.Tags != null)
            {
                foreach (var tag in rule.Tags)
                    livingDocRule.Tags.Add(tag.Name);
            }

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
            var livingDocScenario = new LivingDocScenario
            {
                RuleId = ruleId,
                Id = scenario.Id,
                Description = scenario.Description,
                Name = scenario.Name,
                Keyword = scenario.Keyword
            };

            if (scenario.Tags != null)
            {
                foreach (var tag in scenario.Tags)
                    livingDocScenario.Tags.Add(tag.Name);
            }

            livingDocFeature.Scenarios.Add(livingDocScenario);

            if (scenario.Examples.Count > 0)
            {
                // Consolidating Examples as Self-Contained Scenarios...
                foreach (var example in scenario.Examples)
                {
                    int tableIndexId = 1;
                    foreach (var tableBodyRow in example.TableBody)
                    {
                        var livingDocExample = new LivingDocExample
                        {
                            Name = example.Name,
                            Description = example.Description
                        };
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
                var livingDocStep = new LivingDocStep
                {
                    Id = step.Id,
                    Name = WebUtility.HtmlEncode(step.Text),
                    Keyword = step.Keyword.Trim()
                };

                ParseStepDataTable(livingDocStep, step);

                livingDocExample.Steps.Add(livingDocStep);
            }
        }

        internal static void ParseScenarioExampleTableSteps(LivingDocExample livingDocExample, Scenario scenario, string tableBodyRowId)
        {
            foreach (var step in scenario.Steps)
            {
                var livingDocStep = new LivingDocStep
                {
                    Id = step.Id,
                    TableBodyId = tableBodyRowId,
                    Name = WebUtility.HtmlEncode(step.Text),
                    Type = LivingDocStepTypes.Scenario.ToString(),
                    Keyword = step.Keyword.Trim()
                };

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
