using Expressium.TestExecution;
using Io.Cucumber.Messages.Types;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Expressium.CucumberMessages
{
    public static class CucumberConvertor
    {
        public static void SaveAsTestExecution(string inputFileName, string outputFileName)
        {
            Console.WriteLine("Parsing Cucumber JSON File...");
            var livingDocProject = new LivingDocProject();
            livingDocProject.Title = "Cucumber";

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var listOfEnvelopes = CucumberUtilities.DeserializeAsJson<List<Envelope>>(inputFileName);

            foreach (var envelope in listOfEnvelopes)
            {
                var feature = envelope.GherkinDocument.Feature;

                var livingDocFeature = new LivingDocFeature();

                if (feature.Tags != null)
                {
                    foreach (var tag in feature.Tags)
                        livingDocFeature.Tags.Add(new LivingDocTag() { Name = tag.Name });
                }

                //testExecutionFeature.Id = feature.Id;
                livingDocFeature.Description = feature.Description;
                livingDocFeature.Name = feature.Name;
                livingDocFeature.Keyword = feature.Keyword;
                //testExecutionFeature.Line = feature.line;
                //testExecutionFeature.Uri = feature.uri;
                livingDocProject.Features.Add(livingDocFeature);

                foreach (var child in feature.Children)
                {
                    if (child.Background == null)
                        continue;

                    var background = child.Background;

                    var livingDocBackground = new LivingDocBackground();

                    livingDocBackground.Description = background.Description;
                    livingDocBackground.Name = background.Name;
                    livingDocBackground.Keyword = background.Keyword;

                    foreach (var step in background.Steps)
                    {
                        var livingDocStep = new LivingDocStep();
                        livingDocStep.Name = step.Text;
                        livingDocStep.Keyword = step.Keyword.Trim();
                        livingDocBackground.Steps.Add(livingDocStep);
                    }

                    livingDocFeature.Backgrounds.Add(livingDocBackground);
                }

                foreach (var child in feature.Children)
                {
                    if (child.Scenario == null)
                        continue;

                    var scenario = child.Scenario;

                    var livingDocScenario = new LivingDocScenario();

                    if (scenario.Tags != null)
                    {
                        foreach (var tag in scenario.Tags)
                            livingDocScenario.Tags.Add(new LivingDocTag() { Name = tag.Name });
                    }

                    livingDocScenario.Description = scenario.Description;
                    livingDocScenario.Name = scenario.Name;
                    livingDocScenario.Keyword = scenario.Keyword;
                    livingDocFeature.Scenarios.Add(livingDocScenario);

                    var livingDocExample = new LivingDocExample();
                    livingDocScenario.Examples.Add(livingDocExample);

                    foreach (var example in scenario.Examples)
                    {
                        foreach (var headerCell in example.TableHeader.Cells)
                            livingDocExample.TableHeader.Cells.Add(new LivingDocTableCell() { Value = headerCell.Value });

                        foreach (var tablebodyRow in example.TableBody)
                        {
                            var tableRow = new LivingDocTableRow();
                            foreach (var tableBodyRowCell in tablebodyRow.Cells)
                                tableRow.Cells.Add(new LivingDocTableCell() { Value = tableBodyRowCell.Value });
                            livingDocExample.TableBody.Add(tableRow);
                        }
                    }

                    foreach (var step in scenario.Steps)
                    {
                        var livingDocStep = new LivingDocStep();
                        livingDocStep.Name = step.Text;
                        livingDocStep.Keyword = step.Keyword.Trim();
                        livingDocExample.Steps.Add(livingDocStep);
                    }
                }
            }

            CucumberUtilities.SerializeAsJson(outputFileName, livingDocProject);
        }
    }
}
