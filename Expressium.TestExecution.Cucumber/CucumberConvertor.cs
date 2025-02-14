using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Expressium.TestExecution.Cucumber
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

            var json = File.ReadAllText(inputFileName);
            var features = JsonSerializer.Deserialize<List<Feature>>(json, options);

            //var features = CucumberUtilities.DeserializeAsJson<List<CucumberFeature>>(inputFileName);

            foreach (var feature in features)
            {
                var livingDocFeature = new LivingDocFeature();

                if (feature.Tags != null)
                {
                    foreach (var tag in feature.Tags)
                        livingDocFeature.Tags.Add(new LivingDocTag() { Name = tag.Name });
                }

                livingDocFeature.Id = feature.Id;
                livingDocFeature.Description = feature.Description;
                livingDocFeature.Name = feature.Name;
                livingDocFeature.Keyword = feature.Keyword;
                livingDocFeature.Line = feature.Line;
                livingDocFeature.Uri = feature.Uri;
                livingDocProject.Features.Add(livingDocFeature);

                foreach (var scenario in feature.Elements)
                {
                    var livingDocScenario = new LivingDocScenario();

                    if (scenario.Tags != null)
                    {
                        foreach (var tag in scenario.Tags)
                            livingDocScenario.Tags.Add(new LivingDocTag() { Name = tag.Name });
                    }

                    livingDocScenario.Id = scenario.Id;
                    livingDocScenario.Description = scenario.Description;
                    livingDocScenario.Name = scenario.Name;
                    livingDocScenario.Keyword = scenario.Keyword;
                    livingDocScenario.Line = scenario.Line;
                    livingDocScenario.Type = scenario.Type;
                    livingDocFeature.Scenarios.Add(livingDocScenario);

                    var livingDocExample = new LivingDocExample();
                    livingDocScenario.Examples.Add(livingDocExample);

                    foreach (var step in scenario.Steps)
                    {
                        var livingDocStep = new LivingDocStep();
                        livingDocStep.Name = step.Name;
                        livingDocStep.Line = step.Line;
                        livingDocStep.Keyword = step.Keyword.Trim();
                        if (step.Result != null)
                        {
                            livingDocStep.Status = step.Result.Status.CapitalizeWords();
                            livingDocStep.Duration = step.Result.Duration;
                            livingDocStep.Error = step.Result.Error_message;
                        }

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

            foreach (var feature in livingDocProject.Features)
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

            CucumberUtilities.SerializeAsJson(outputFileName, livingDocProject);
        }
    }
}
