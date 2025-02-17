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
                    var livingDocScenario = new LivingDocScenario();

                    if (child.Scenario == null)
                        continue;

                    var scenario = child.Scenario;

                    if (scenario.Tags != null)
                    {
                        foreach (var tag in scenario.Tags)
                            livingDocScenario.Tags.Add(new LivingDocTag() { Name = tag.Name });
                    }

                    //testExecutionScenario.Id = scenario.Id;
                    livingDocScenario.Description = scenario.Description;
                    livingDocScenario.Name = scenario.Name;
                    livingDocScenario.Keyword = scenario.Keyword;
                    //testExecutionScenario.Line = scenario.line;
                    //testExecutionScenario.Type = scenario.type;
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
                        //testExecutionStep.Line = step.text;
                        livingDocStep.Keyword = step.Keyword.Trim();

                        //if (step.result != null)
                        //{
                        //    testExecutionStep.Status = step.Result.Status.CapitalizeWords();
                        //    testExecutionStep.Duration = step.Result.Duration;
                        //    testExecutionStep.Error = step.Result.Error_message;
                        //}

                        //foreach (var row in step.Rows)
                        //{
                        //    foreach (var cell in row.Cells)
                        //    {
                        //        //testExecutionStep.Arguments.Add();
                        //    }
                        //}

                        livingDocExample.Steps.Add(livingDocStep);
                    }
                }
            }

            /*
            foreach (var feature in testExecutionProject.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        foreach (var step in example.Steps)
                        {
                            if (step.Status == "Failed")
                            {
                                step.Status = TestExecutionStatuses.TestError.ToString();
                                example.Status = TestExecutionStatuses.TestError.ToString();
                            }
                            else if (step.Status == "Passed")
                            {
                                step.Status = TestExecutionStatuses.OK.ToString();
                                example.Status = TestExecutionStatuses.OK.ToString();
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
            */

            CucumberUtilities.SerializeAsJson(outputFileName, livingDocProject);
        }
    }
}
