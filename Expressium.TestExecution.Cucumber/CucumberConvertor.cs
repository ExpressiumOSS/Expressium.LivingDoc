using System;

namespace Expressium.TestExecution.Cucumber
{
    public static class CucumberConvertor
    {
        public static void SaveAsTestExecution(string inputFileName, string outputFileName)
        {
            Console.WriteLine("Parsing Cucumber JSON File...");
            var testExecutionProject = new TestExecutionProject();
            testExecutionProject.Title = "Cucumber";

            var cucumberContext = CucumberUtilities.DeserializeAsJson<CucumberProject>(inputFileName);

            foreach (var feature in cucumberContext.objects)
            {
                var testExecutionFeature = new TestExecutionFeature();
                testExecutionFeature.FolderPath = feature.uri.ToString();
                testExecutionFeature.Tags = feature.id.ToString();
                testExecutionFeature.Title = feature.name.ToString();
                testExecutionFeature.Description = feature.description.ToString();
                testExecutionProject.Features.Add(testExecutionFeature);

                foreach (var scenario in feature.elements)
                {
                    var testExecutionScenario = new TestExecutionScenario();
                    testExecutionScenario.Tags = scenario.id.ToString().Replace(";", " ");
                    testExecutionScenario.Title = scenario.name.ToString();
                    testExecutionScenario.Description = scenario.line.ToString();
                    testExecutionFeature.Scenarios.Add(testExecutionScenario);

                    var testExecutionExample = new TestExecutionExample();
                    testExecutionScenario.Examples.Add(testExecutionExample);

                    foreach (var step in scenario.steps)
                    {
                        var testExecutionStep = new TestExecutionStep();
                        testExecutionStep.Text = step.name.ToString();
                        testExecutionStep.Type = step.keyword.Trim().ToString();
                        testExecutionStep.Status = step.result.status.ToString().CapitalizeWords();
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
