using Expressium.TestExecution;
using System;
using System.IO;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    public partial class BaseHooks
    {
        private static TestExecutionProject testExecutionProject;
        private static string outputFileName = "TestExecution.json";

        private TestExecutionScenario testExecutionScenario;

        private static void InitializeTestExecution()
        {
            testExecutionProject = new TestExecutionProject();
            testExecutionProject.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            testExecutionProject.ExecutionTime = DateTime.UtcNow.ToLocalTime();
            testExecutionProject.StartTime = DateTime.Now;
            testExecutionProject.EndTime = DateTime.Now;
            testExecutionProject.Environment = "Development";

            System.Threading.Thread.Sleep(1000);
        }

        private static void FinalizeTestExecution()
        {
            testExecutionProject.EndTime = DateTime.Now;

            TestExecutionUtilities.SerializeAsJson(Path.Combine(Directory.GetCurrentDirectory(), outputFileName), testExecutionProject);
        }

        private void AddTestExecutionBeforeScenario()
        {
            if (!testExecutionProject.IsFeatureAdded(featureContext.FeatureInfo.Title))
            {
                testExecutionProject.Features.Add(new TestExecutionFeature()
                {
                    Tags = string.Join(" ", featureContext.FeatureInfo.Tags),
                    Title = featureContext.FeatureInfo.Title,
                    Description = featureContext.FeatureInfo.Description,
                    FolderPath = featureContext.FeatureInfo.FolderPath
                });
            }

            if (testExecutionProject.IsFeatureAdded(featureContext.FeatureInfo.Title))
            {
                testExecutionScenario = (new TestExecutionScenario()
                {
                    Tags = string.Join(" ", scenarioContext.ScenarioInfo.Tags),
                    Title = scenarioContext.ScenarioInfo.Title,
                    Description = scenarioContext.ScenarioInfo.Description
                });

                var testExecutionExample = new TestExecutionExample();
                testExecutionExample.StartTime = DateTime.Now;
                testExecutionExample.EndTime = DateTime.Now;
                testExecutionScenario.Examples.Add(testExecutionExample);

                if (scenarioContext.ScenarioInfo.Arguments.Count > 0)
                {
                    var keyCollection = scenarioContext.ScenarioInfo.Arguments.Keys;
                    var valueCollection = scenarioContext.ScenarioInfo.Arguments.Values;

                    var numberOfArguments = scenarioContext.ScenarioInfo.Arguments.Count;

                    string[] arrayOfKeys = new string[numberOfArguments];
                    string[] arrayOfValues = new string[numberOfArguments];
                    keyCollection.CopyTo(arrayOfKeys, 0);
                    valueCollection.CopyTo(arrayOfValues, 0);

                    for (int i = 0; i < numberOfArguments; i++)
                    {
                        testExecutionScenario.Examples[0].Arguments.Add(new TestExecutionArgument()
                        {
                            Name = arrayOfKeys[i],
                            Value = arrayOfValues[i]
                        });
                    }
                }
            }
        }

        private void AddTestExecutionAfterScenario()
        {
            if (testExecutionScenario != null)
            {
                testExecutionScenario.Examples[0].Status = scenarioContext.ScenarioExecutionStatus.ToString();
                testExecutionScenario.Examples[0].Error = scenarioContext.TestError?.Message;
                testExecutionScenario.Examples[0].Stacktrace = scenarioContext.TestError?.StackTrace;
                testExecutionScenario.Examples[0].EndTime = DateTime.Now;

                if (testExecutionProject.IsFeatureAdded(featureContext.FeatureInfo.Title))
                {
                    var testExecutionFeature = testExecutionProject.GetFeature(featureContext.FeatureInfo.Title);

                    if (testExecutionFeature.IsScenarioAdded(scenarioContext.ScenarioInfo.Title))
                    {
                        var testExecutionScenarioParent = testExecutionFeature.GetScenario(scenarioContext.ScenarioInfo.Title);
                        testExecutionScenarioParent.Examples.Add(testExecutionScenario.Examples[0]);
                    }
                    else
                    {
                        testExecutionFeature.Scenarios.Add(testExecutionScenario);
                    }
                }
            }
        }

        private void AddTestExecutionScenarioAttachment(string filePath)
        {
            if (testExecutionScenario != null)
            {
                testExecutionScenario.Examples[0].Attachments.Add(filePath);
            }
        }

        private void AddTestExecutionAfterStep()
        {
            if (testExecutionScenario != null)
            {
                var testExecutionStep = new TestExecutionStep();
                testExecutionStep.Type = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                testExecutionStep.Text = scenarioContext.StepContext.StepInfo.Text;
                testExecutionStep.Status = scenarioContext.StepContext.Status.ToString();

                if (scenarioContext.StepContext.StepInfo.Table != null)
                {
                    foreach (var row in scenarioContext.StepContext.StepInfo.Table.Rows)
                    {
                        var keyCollection = row.Keys;
                        var valueCollection = row.Values;

                        var numberOfArguments = row.Keys.Count;

                        string[] arrayOfKeys = new string[numberOfArguments];
                        string[] arrayOfValues = new string[numberOfArguments];
                        keyCollection.CopyTo(arrayOfKeys, 0);
                        valueCollection.CopyTo(arrayOfValues, 0);

                        for (int i = 0; i < numberOfArguments; i++)
                        {
                            testExecutionStep.Arguments.Add(new TestExecutionArgument()
                            {
                                Name = arrayOfKeys[i],
                                Value = arrayOfValues[i]
                            });
                        }
                    }
                }

                testExecutionScenario.Examples[0].Steps.Add(testExecutionStep);
            }
        }
    }
}
