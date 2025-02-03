using Expressium.TestExecution;
using System;
using System.IO;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    public partial class BaseHooks
    {
        private static TestExecutionContext testExecutionContext;
        private static string outputFileName = "TestExecution.json";

        private TestExecutionScenario testExecutionScenario;

        private static void InitializeTestExecution()
        {
            testExecutionContext = new TestExecutionContext();
            testExecutionContext.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            testExecutionContext.ExecutionTime = DateTime.UtcNow.ToLocalTime();
            testExecutionContext.StartTime = DateTime.Now;
            testExecutionContext.EndTime = DateTime.Now;

            System.Threading.Thread.Sleep(1000);
        }

        private static void FinalizeTestExecution()
        {
            testExecutionContext.EndTime = DateTime.Now;

            TestExecutionUtilities.SerializeAsJson(Path.Combine(Directory.GetCurrentDirectory(), outputFileName), testExecutionContext);
        }

        private void AddTestExecutionBeforeScenario()
        {
            if (!testExecutionContext.IsFeatureAdded(featureContext.FeatureInfo.Title))
            {
                testExecutionContext.Features.Add(new TestExecutionFeature()
                {
                    Tags = string.Join(" ", featureContext.FeatureInfo.Tags),
                    Title = featureContext.FeatureInfo.Title,
                    Description = featureContext.FeatureInfo.Description,
                    FolderPath = featureContext.FeatureInfo.FolderPath
                });
            }

            if (testExecutionContext.IsFeatureAdded(featureContext.FeatureInfo.Title))
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

                if (testExecutionContext.IsFeatureAdded(featureContext.FeatureInfo.Title))
                {
                    var testExecutionFeature = testExecutionContext.GetFeature(featureContext.FeatureInfo.Title);

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
                testExecutionScenario.Examples[0].Steps.Add(new TestExecutionStep()
                {
                    Type = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString(),
                    Text = scenarioContext.StepContext.StepInfo.Text,
                    Status = scenarioContext.StepContext.Status.ToString(),
                });
            }
        }
    }
}
