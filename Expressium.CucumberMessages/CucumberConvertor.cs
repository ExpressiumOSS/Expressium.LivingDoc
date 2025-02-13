using Expressium.TestExecution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Expressium.CucumberMessages
{
    public static class CucumberConvertor
    {
        public static void SaveAsTestExecution(string inputFileName, string outputFileName)
        {
            Console.WriteLine("Parsing Cucumber JSON File...");
            var testExecutionProject = new TestExecutionProject();
            testExecutionProject.Title = "Cucumber";

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var features = CucumberUtilities.DeserializeAsJson<List<Feature>>(inputFileName);

            foreach (var feature in features)
            {
                var testExecutionFeature = new TestExecutionFeature();

                if (feature.tags != null)
                {
                    foreach (var tag in feature.tags)
                        testExecutionFeature.Tags += tag.name.Replace("@", "") + " ";
                    testExecutionFeature.Tags.Trim();
                }

                //testExecutionFeature.Id = feature.Id;
                testExecutionFeature.Description = feature.description;
                testExecutionFeature.Name = feature.name;
                testExecutionFeature.Keyword = feature.keyword;
                //testExecutionFeature.Line = feature.line;
                //testExecutionFeature.Uri = feature.uri;
                testExecutionProject.Features.Add(testExecutionFeature);

                foreach (var child in feature.children)
                {
                    var testExecutionScenario = new TestExecutionScenario();

                    if ( child.scenario == null )
                        continue;

                    var scenario = child.scenario;

                    if (scenario.tags != null)
                    {
                        foreach (var tag in scenario.tags)
                            testExecutionScenario.Tags += tag.name.Replace("@", "") + " ";
                        testExecutionScenario.Tags.Trim();
                    }

                    //testExecutionScenario.Id = scenario.Id;
                    testExecutionScenario.Description = scenario.description;
                    testExecutionScenario.Name = scenario.name;
                    testExecutionScenario.Keyword = scenario.keyword;
                    //testExecutionScenario.Line = scenario.line;
                    //testExecutionScenario.Type = scenario.type;
                    testExecutionFeature.Scenarios.Add(testExecutionScenario);

                    var testExecutionExample = new TestExecutionExample();
                    testExecutionScenario.Examples.Add(testExecutionExample);

                    foreach (var step in scenario.steps)
                    {
                        var testExecutionStep = new TestExecutionStep();
                        testExecutionStep.Name = step.text;
                        //testExecutionStep.Line = step.text;
                        testExecutionStep.Keyword = step.keyword.Trim();
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

                        testExecutionExample.Steps.Add(testExecutionStep);
                    }
                }
            }

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

            CucumberUtilities.SerializeAsJson(outputFileName, testExecutionProject);
        }
    }
}
