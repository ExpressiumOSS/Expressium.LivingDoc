using Expressium.LivingDoc.Models;
using Io.Cucumber.Messages.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Expressium.LivingDoc.Messages
{
    internal static class MessagesConvertorEx
    {
        internal static LivingDocProject ConvertToLivingDoc(string filePath)
        {
            var listOfGherkinDocuments = new List<GherkinDocument>();
            var listOfPickles = new List<Pickle>();
            var listOfTestCases = new List<TestCase>();
            var listOfTestStepFinished = new List<TestStepFinished>();
            var listOfTestCaseStarted = new List<TestCaseStarted>();
            var listOfTestCaseFinished = new List<TestCaseFinished>();
            var listOfTestRunStarted = new List<TestRunStarted>();
            var listOfTestRunFinished = new List<TestRunFinished>();
            var listOftAttachment = new List<Attachment>();

            // Parse Cucumber Messages file...
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                var enumerator = new MessagesReader(fileStream).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var envelope = enumerator.Current;

                    if (envelope.GherkinDocument != null)
                        listOfGherkinDocuments.Add(envelope.GherkinDocument);

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

                    if (envelope.TestRunStarted != null)
                        listOfTestRunStarted.Add(envelope.TestRunStarted);

                    if (envelope.TestRunFinished != null)
                        listOfTestRunFinished.Add(envelope.TestRunFinished);

                    if (envelope.Attachment != null)
                        listOftAttachment.Add(envelope.Attachment);
                }
            }

            var livingDocProject = new LivingDocProject();
            livingDocProject.Title = "LivingDoc";

            // Get Test Execution Date...
            var testRunStarted = listOfTestRunStarted.FirstOrDefault();
            livingDocProject.Date = testRunStarted.Timestamp.ToDateTime();

            // Get Test Execution Duration...
            var testRunFinished = listOfTestRunFinished.Last();
            livingDocProject.Duration = testRunStarted.Timestamp.ToTimeSpan(testRunFinished.Timestamp);

            // Parse Gherkin Documents...
            foreach (var gherkinDocument in listOfGherkinDocuments)
                ParseGherkinDocument(livingDocProject, gherkinDocument);

            // Parse Test Results...
            foreach (var feature in livingDocProject.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        if (example.HasDataTable())
                        {
                            TestStepFinished testStepFinished = null;

                            foreach (var step in example.Steps)
                            {
                                if (step.TableRowId != null)
                                {
                                    var pickleItemStep = listOfPickles
                                    .SelectMany(p => p.Steps)  // flatten all steps across all pickles
                                    .Where(s => s.AstNodeIds.Contains(step.Id) && s.AstNodeIds.Contains(step.TableRowId))
                                    .FirstOrDefault();

                                      //Pickle pickleItemStep = null;
                                    //var pickleList = listOfPickles.FindAll(y => y.AstNodeIds.Contains(step.TableRowId));
                                    //foreach (var pickle in pickleList)
                                    //{
                                    //    if (pickle.AstNodeIds.Contains(step.Id) && pickle.AstNodeIds.Contains(step.TableRowId))
                                    //    {
                                    //        pickleItemStep = pickle;
                                    //        break;
                                    //    }
                                    //}

                                    if (pickleItemStep == null)
                                        continue;

                                    //var pickleItemStep = pickle.Steps.Find(x => x.AstNodeIds.Contains(step.TableRowId));
                                    //if (pickleItemStep == null)
                                    //    continue;

                                    foreach (var testCase in listOfTestCases)
                                    {
                                        var testCaseTestStep = testCase.TestSteps.Find(y => y.PickleStepId == pickleItemStep.Id);
                                        if (testCaseTestStep == null)
                                            continue;

                                        testStepFinished = listOfTestStepFinished.Find(f => f.TestStepId == testCaseTestStep.Id);
                                        if (testStepFinished == null)
                                            continue;

                                        MessagesConvertor.ParseTestStepResults(step, testStepFinished);
                                        break;
                                    }
                                }
                            }

                            if (testStepFinished == null)
                                continue;

                            var testCaseStarted = listOfTestCaseStarted.Find(g => g.Id == testStepFinished.TestCaseStartedId);
                            if (testCaseStarted == null)
                                continue;

                            var attachments = listOftAttachment.FindAll(a => a.TestCaseStartedId.Contains(testCaseStarted.Id));
                            if (attachments.Count > 0)
                            {
                                foreach (var attachment in attachments)
                                    MessagesConvertor.ParseExampleAttachments(example, attachment);
                            }

                            var testCaseFinished = listOfTestCaseFinished.Find(j => j.TestCaseStartedId == testStepFinished.TestCaseStartedId);
                            if (testCaseFinished == null)
                                continue;

                            example.Duration = testCaseStarted.Timestamp.ToTimeSpan(testCaseFinished.Timestamp);
                        }
                    }
                }
            }

            MessagesConvertor.PostProcessingProject(livingDocProject);

            return livingDocProject;
        }

        internal static void ParseGherkinDocument(LivingDocProject livingDocProject, GherkinDocument gherkinDocument)
        {
            var uri = gherkinDocument.Uri;
            var feature = gherkinDocument.Feature;

            var livingDocFeature = new LivingDocFeature();

            if (feature.Tags != null)
            {
                foreach (var tag in feature.Tags)
                    livingDocFeature.Tags.Add(tag.Name);
            }

            livingDocFeature.Description = feature.Description;
            livingDocFeature.Uri = uri;
            livingDocFeature.Name = feature.Name;
            livingDocFeature.Keyword = feature.Keyword;
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
                    livingDocStep.Name = WebUtility.HtmlEncode(step.Text);
                    livingDocStep.Keyword = step.Keyword.Trim();
                    livingDocStep.Id = step.Id;
                    livingDocBackground.Steps.Add(livingDocStep);
                }

                livingDocFeature.Background = livingDocBackground;
            }

            foreach (var child in feature.Children)
            {
                if (child.Scenario != null)
                {
                    ParseScenario(livingDocFeature, child.Scenario);
                }
                else if (child.Rule != null)
                {
                    var rule = child.Rule;

                    var ruleId = ParseRule(livingDocFeature, rule);

                    foreach (var ruleChild in rule.Children)
                    {
                        if (ruleChild.Scenario == null)
                            continue;

                        ParseScenario(livingDocFeature, ruleChild.Scenario, ruleId);
                    }
                }
            }
        }

        internal static string ParseRule(LivingDocFeature livingDocFeature, Rule rule)
        {
            var livingDocRule = new LivingDocRule();

            if (rule.Tags != null)
            {
                foreach (var tag in rule.Tags)
                    livingDocRule.Tags.Add(tag.Name);
            }

            livingDocRule.Id = rule.Id;
            livingDocRule.Description = rule.Description;
            livingDocRule.Name = rule.Name;
            livingDocRule.Keyword = rule.Keyword;

            livingDocFeature.Rules.Add(livingDocRule);

            return livingDocRule.Id;
        }

        internal static void ParseScenario(LivingDocFeature livingDocFeature, Scenario scenario, string ruleId = null)
        {
            var livingDocScenario = new LivingDocScenario();

            if (scenario.Tags != null)
            {
                foreach (var tag in scenario.Tags)
                    livingDocScenario.Tags.Add(tag.Name);
            }

            livingDocScenario.RuleId = ruleId;
            livingDocScenario.Id = scenario.Id;
            livingDocScenario.Description = scenario.Description;
            livingDocScenario.Name = scenario.Name;
            livingDocScenario.Keyword = scenario.Keyword;
            livingDocFeature.Scenarios.Add(livingDocScenario);

            if (scenario.Examples.Count > 0)
            {
                // Consolidating Scenario Examples as Self-Contained...
                foreach (var examples in scenario.Examples)
                {
                    foreach (var tableBodyRow in examples.TableBody)
                    {
                        var livingDocExample = new LivingDocExample();
                        livingDocScenario.Examples.Add(livingDocExample);

                        ParseScenarioBackgroundSteps(livingDocExample, livingDocFeature);

                        //ParseScenarioExampleSteps(livingDocExample, scenario);
                        foreach (var step in scenario.Steps)
                        {
                            var livingDocStep = new LivingDocStep();
                            livingDocStep.Id = step.Id;
                            livingDocStep.TableRowId = tableBodyRow.Id;
                            livingDocStep.Name = WebUtility.HtmlEncode(step.Text);
                            livingDocStep.Keyword = step.Keyword.Trim();

                            if (step.DataTable != null)
                            {
                                foreach (var row in step.DataTable.Rows)
                                {
                                    var dataTableRow = new LivingDocDataTableRow();
                                    foreach (var cell in row.Cells)
                                        dataTableRow.Cells.Add(cell.Value);
                                    livingDocStep.DataTable.Rows.Add(dataTableRow);
                                }
                            }

                            livingDocExample.Steps.Add(livingDocStep);
                        }

                        ParseScenarioExampleTableHeaders(livingDocExample, examples);
                        ParseScenarioExampleTableData(livingDocExample, tableBodyRow);
                    }
                }
            }
            else
            {
                var livingDocExample = new LivingDocExample();
                livingDocScenario.Examples.Add(livingDocExample);

                ParseScenarioBackgroundSteps(livingDocExample, livingDocFeature);
                ParseScenarioExampleSteps(livingDocExample, scenario);
            }
        }

        internal static void ParseScenarioBackgroundSteps(LivingDocExample livingDocExample, LivingDocFeature livingDocFeature)
        {
            if (livingDocFeature.Background != null)
            {
                foreach (var backgroundStep in livingDocFeature.Background.Steps)
                    livingDocExample.Steps.Add(backgroundStep.Copy(backgroundStep));
            }
        }

        internal static void ParseScenarioExampleSteps(LivingDocExample livingDocExample, Scenario scenario)
        {
            foreach (var step in scenario.Steps)
            {
                var livingDocStep = new LivingDocStep();
                livingDocStep.Id = step.Id;
                livingDocStep.Name = WebUtility.HtmlEncode(step.Text);
                livingDocStep.Keyword = step.Keyword.Trim();

                if (step.DataTable != null)
                {
                    foreach (var row in step.DataTable.Rows)
                    {
                        var dataTableRow = new LivingDocDataTableRow();
                        foreach (var cell in row.Cells)
                            dataTableRow.Cells.Add(cell.Value);
                        livingDocStep.DataTable.Rows.Add(dataTableRow);
                    }
                }

                livingDocExample.Steps.Add(livingDocStep);
            }
        }

        internal static void ParseScenarioExampleTableHeaders(LivingDocExample livingDocExample, Examples examples)
        {
            var dataTableRow = new LivingDocDataTableRow();
            foreach (var tableHeaderRowCell in examples.TableHeader.Cells)
                dataTableRow.Cells.Add(tableHeaderRowCell.Value);
            livingDocExample.DataTable.Rows.Add(dataTableRow);
        }

        internal static void ParseScenarioExampleTableData(LivingDocExample livingDocExample, TableRow tableBodyRow)
        {
            var dataTableRow = new LivingDocDataTableRow();
            foreach (var tableBodyRowCell in tableBodyRow.Cells)
                dataTableRow.Cells.Add(tableBodyRowCell.Value);
            livingDocExample.DataTable.Rows.Add(dataTableRow);
        }
    }
}
