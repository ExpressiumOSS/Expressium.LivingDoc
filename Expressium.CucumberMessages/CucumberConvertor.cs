using Expressium.TestExecution;
using Io.Cucumber.Messages.Types;
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

            var listOfEnvelopes = CucumberUtilities.DeserializeAsJson<List<Envelope>>(inputFileName);

            foreach (var envelope in listOfEnvelopes)
            {
                var feature = envelope.GherkinDocument.Feature;

                var testExecutionFeature = new TestExecutionFeature();

                if (feature.Tags != null)
                {
                    foreach (var tag in feature.Tags)
                        testExecutionFeature.Tags.Add(new TestExecutionTag() { Name = tag.Name });
                }

                //testExecutionFeature.Id = feature.Id;
                testExecutionFeature.Description = feature.Description;
                testExecutionFeature.Name = feature.Name;
                testExecutionFeature.Keyword = feature.Keyword;
                //testExecutionFeature.Line = feature.line;
                //testExecutionFeature.Uri = feature.uri;
                testExecutionProject.Features.Add(testExecutionFeature);

                foreach (var child in feature.Children)
                {
                    var testExecutionScenario = new TestExecutionScenario();

                    if (child.Scenario == null)
                        continue;

                    var scenario = child.Scenario;

                    if (scenario.Tags != null)
                    {
                        foreach (var tag in scenario.Tags)
                            testExecutionScenario.Tags.Add(new TestExecutionTag() { Name = tag.Name });
                    }

                    //testExecutionScenario.Id = scenario.Id;
                    testExecutionScenario.Description = scenario.Description;
                    testExecutionScenario.Name = scenario.Name;
                    testExecutionScenario.Keyword = scenario.Keyword;
                    //testExecutionScenario.Line = scenario.line;
                    //testExecutionScenario.Type = scenario.type;
                    testExecutionFeature.Scenarios.Add(testExecutionScenario);

                    var testExecutionExample = new TestExecutionExample();
                    testExecutionScenario.Examples.Add(testExecutionExample);

                    foreach (var step in scenario.Steps)
                    {
                        var testExecutionStep = new TestExecutionStep();
                        testExecutionStep.Name = step.Text;
                        //testExecutionStep.Line = step.text;
                        testExecutionStep.Keyword = step.Keyword.Trim();

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

            CucumberUtilities.SerializeAsJson(outputFileName, testExecutionProject);
        }
    }
}
