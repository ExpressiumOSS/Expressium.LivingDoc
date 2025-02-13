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
                var testExecutionFeature = new TestExecutionFeature();

                foreach (var tag in featureContext.FeatureInfo.Tags)
                    testExecutionFeature.Tags.Add(new TestExecutionTag() { Name = "@" + tag });

                testExecutionFeature.Name = featureContext.FeatureInfo.Title;
                testExecutionFeature.Description = featureContext.FeatureInfo.Description;
                testExecutionFeature.Uri = featureContext.FeatureInfo.FolderPath;
                testExecutionProject.Features.Add(testExecutionFeature);
            }

            if (testExecutionProject.IsFeatureAdded(featureContext.FeatureInfo.Title))
            {
                testExecutionScenario = new TestExecutionScenario();

                foreach (var tag in scenarioContext.ScenarioInfo.Tags)
                    testExecutionScenario.Tags.Add(new TestExecutionTag() { Name = "@" + tag });

                testExecutionScenario.Name = scenarioContext.ScenarioInfo.Title;
                testExecutionScenario.Description = scenarioContext.ScenarioInfo.Description;

                var testExecutionExample = new TestExecutionExample();
                testExecutionExample.StartTime = DateTime.Now;
                testExecutionExample.EndTime = DateTime.Now;
                testExecutionScenario.Examples.Add(testExecutionExample);

                if (scenarioContext.ScenarioInfo.Arguments.Count > 0)
                {                    
                    foreach (var key in scenarioContext.ScenarioInfo.Arguments.Keys)
                        testExecutionScenario.Examples[0].TableHeader.Cells.Add(new TestExecutionTableCell() { Value = key.ToString() });

                    var testExecutionTableRow = new TestExecutionTableRow();
                    foreach (var value in scenarioContext.ScenarioInfo.Arguments.Values)
                        testExecutionTableRow.Cells.Add(new TestExecutionTableCell() { Value = value.ToString() });
                    testExecutionScenario.Examples[0].TableBody.Add(testExecutionTableRow);
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
                testExecutionStep.Keyword = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                testExecutionStep.Name = scenarioContext.StepContext.StepInfo.Text;
                testExecutionStep.Status = scenarioContext.StepContext.Status.ToString();

                if (scenarioContext.StepContext.StepInfo.Table != null)
                {
                    var testExecutionTableHeaderRow = new TestExecutionTableRow();
                    foreach (var header in scenarioContext.StepContext.StepInfo.Table.Header)
                        testExecutionTableHeaderRow.Cells.Add(new TestExecutionTableCell() { Value = header });
                    testExecutionStep.DataTable.Rows.Add(testExecutionTableHeaderRow);

                    var testExecutionTableRow = new TestExecutionTableRow();
                    foreach (var row in scenarioContext.StepContext.StepInfo.Table.Rows)
                    {
                        foreach (var value in row.Values)
                            testExecutionTableRow.Cells.Add(new TestExecutionTableCell() { Value = value });
                    }
                    testExecutionStep.DataTable.Rows.Add(testExecutionTableRow);
                }

                testExecutionScenario.Examples[0].Steps.Add(testExecutionStep);
            }
        }
    }
}
