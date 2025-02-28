using Expressium.LivingDoc;
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

            var listOfEnvelopes = LivingDocUtilities.DeserializeAsJson<List<Envelope>>(inputFileName);

            var listOfPickles = new List<Pickle>();
            var listOfTestCases = new List<TestCase>();
            var listOfTestStepFinished = new List<TestStepFinished>();
            var listOfTestCaseStarted = new List<TestCaseStarted>();
            var listOfTestCaseFinished = new List<TestCaseFinished>();
            var listOfTestRunFinished = new List<TestRunFinished>();

            foreach (var envelope in listOfEnvelopes)
            {
                if (envelope.GherkinDocument != null)
                    ParsingGherkinDocument(livingDocProject, envelope);

                if (envelope.Pickle != null)
                    listOfPickles.Add(envelope.Pickle);

                if (envelope.TestCase != null)
                    listOfTestCases.Add(envelope.TestCase);

                if (envelope.TestStepFinished != null)
                    listOfTestStepFinished.Add(envelope.TestStepFinished);

                if (envelope.TestCaseStarted != null)
                    listOfTestCaseStarted.Add(envelope.TestCaseStarted);

                if (envelope.TestCaseFinished != null)
                    listOfTestCaseFinished.Add(envelope.TestCaseFinished);

                if (envelope.TestRunFinished != null)
                    listOfTestRunFinished.Add(envelope.TestRunFinished);
            }

            var duration = new TimeSpan(0, 0, 0, 0, 0);
            foreach (var testRunStarted in listOfTestRunFinished)
                duration += new TimeSpan(0, 0, 0, (int)testRunStarted.Timestamp.Seconds, 0, (int)testRunStarted.Timestamp.Nanos);
            livingDocProject.Duration = duration;

            foreach (var feature in livingDocProject.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    var pickle = listOfPickles.Find(x => x.AstNodeIds.Contains(scenario.Id));
                    if (pickle != null)
                    {
                        var testCase = listOfTestCases.Find(y => y.PickleId == pickle.Id);
                        if (testCase == null)
                            continue;

                        foreach (var example in scenario.Examples)
                        {
                            foreach (var step in example.Steps)
                            {
                                var pickleStep = pickle.Steps.Find(x => x.AstNodeIds.Contains(step.Id));
                                if (pickleStep == null)
                                    continue;

                                var testCaseStep = testCase.TestSteps.Find(z => z.PickleStepId == pickleStep.Id);
                                if (testCaseStep == null)
                                    continue;

                                var testStepFinished = listOfTestStepFinished.Find(f => f.TestStepId == testCaseStep.Id);
                                if (testStepFinished == null)
                                    continue;

                                step.Status = testStepFinished.TestStepResult.Status.ToLower().CapitalizeWords();
                                step.Message = testStepFinished.TestStepResult.Message;
                            }

                            var testCaseStarted = listOfTestCaseStarted.Find(g => g.TestCaseId == testCase.Id);
                            if (testCaseStarted == null)
                                continue;

                            var testCaseFinished = listOfTestCaseFinished.Find(j => j.TestCaseStartedId == testCaseStarted.Id);
                            if (testCaseFinished == null)
                                continue;

                            example.Duration = new TimeSpan(0, 0, 0, (int)testCaseFinished.Timestamp.Seconds, 0, (int)testCaseFinished.Timestamp.Nanos);
                        }
                    }
                }
            }

            LivingDocUtilities.SerializeAsJson(outputFileName, livingDocProject);
        }

        public static void ParsingGherkinDocument(LivingDocProject livingDocProject, Envelope envelope)
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

            // Background
            foreach (var child in feature.Children)
            {
                if (child.Background == null)
                    continue;

                var background = child.Background;

                var livingDocBackground = new LivingDocBackground();

                livingDocBackground.Id = background.Id;
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

                livingDocFeature.Background = livingDocBackground;
            }

            // Scenario
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

                livingDocScenario.Id = scenario.Id;
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

                if (livingDocFeature.Background.Steps.Count > 0)
                {
                    foreach (var backgroundStep in livingDocFeature.Background.Steps)
                        livingDocExample.Steps.Add(backgroundStep.Copy(backgroundStep));
                }

                foreach (var step in scenario.Steps)
                {
                    var livingDocStep = new LivingDocStep();
                    livingDocStep.Id = step.Id;
                    livingDocStep.Name = step.Text;
                    livingDocStep.Keyword = step.Keyword.Trim();

                    if (step.DataTable != null)
                    {
                        foreach (var row in step.DataTable.Rows)
                        {
                            var tableRow = new LivingDocTableRow();
                            foreach (var cell in row.Cells)
                                tableRow.Cells.Add(new LivingDocTableCell() { Value = cell.Value });
                            livingDocStep.DataTable.Rows.Add(tableRow);
                        }
                    }

                    livingDocExample.Steps.Add(livingDocStep);
                }
            }

            // Rule
            foreach (var child in feature.Children)
            {
                if (child.Rule == null)
                    continue;

                var rule = child.Rule;

                foreach (var ruleChild in rule.Children)
                {
                    if (ruleChild.Scenario == null)
                        continue;

                    var ruleScenario = ruleChild.Scenario;

                    // var livingDocScenario = new LivingDocScenario();
                    // TODO - Missing implementation of Rule Scenarios...
                }
            }
        }
    }
}
