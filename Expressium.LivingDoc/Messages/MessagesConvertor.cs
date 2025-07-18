using Expressium.LivingDoc.Models;
using Io.Cucumber.Messages.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Expressium.LivingDoc.Messages
{
    public static class MessagesConvertor
    {
        public static LivingDocProject ConvertToLivingDoc(string filePath)
        {
            var livingDocProject = new LivingDocProject();
            livingDocProject.Title = "Cucumber";

            var listOfPickles = new List<Pickle>();
            var listOfTestCases = new List<TestCase>();
            var listOfTestStepFinished = new List<TestStepFinished>();
            var listOfTestCaseStarted = new List<TestCaseStarted>();
            var listOfTestCaseFinished = new List<TestCaseFinished>();
            var listOfTestRunFinished = new List<TestRunFinished>();
            var listOftAttachment = new List<Attachment>();

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                var enumerator = new MessagesReader(fileStream).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var envelope = enumerator.Current;

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

                    if (envelope.Attachment != null)
                        listOftAttachment.Add(envelope.Attachment);
                }
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

                                step.Status = testStepFinished.TestStepResult.Status.ToString().ToLower().CapitalizeWords();
                                step.Message = testStepFinished.TestStepResult.Message;
                            }

                            var testCaseStarted = listOfTestCaseStarted.Find(g => g.TestCaseId == testCase.Id);
                            if (testCaseStarted == null)
                                continue;

                            var testCaseFinished = listOfTestCaseFinished.Find(j => j.TestCaseStartedId == testCaseStarted.Id);
                            if (testCaseFinished == null)
                                continue;

                            var attachments = listOftAttachment.FindAll(a => a.TestCaseStartedId.Contains(testCaseStarted.Id));
                            if (attachments.Count > 0)
                            {
                                foreach (var attachment in attachments)
                                {
                                    if (attachment.MediaType == "text/uri-list")
                                        example.Attachments.Add(attachment.Body);
                                }
                            }

                            example.Duration = new TimeSpan(0, 0, 0, (int)testCaseFinished.Timestamp.Seconds, 0, (int)testCaseFinished.Timestamp.Nanos);
                        }
                    }
                }
            }

            return livingDocProject;
        }

        public static void ParsingGherkinDocument(LivingDocProject livingDocProject, Envelope envelope)
        {
            var uri = envelope.GherkinDocument.Uri;
            var feature = envelope.GherkinDocument.Feature;

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
                    livingDocStep.Name = step.Text;
                    livingDocStep.Keyword = step.Keyword.Trim();
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

        public static string ParseRule(LivingDocFeature livingDocFeature, Rule rule)
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

        public static void ParseScenario(LivingDocFeature livingDocFeature, Scenario scenario, string ruleId = null)
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

                        AddFeatureBackgroundSteps(livingDocFeature, livingDocExample);
                        AddScenarioExampleTableHeaders(examples, livingDocExample);
                        AddScenarioExampleTableData(tableBodyRow, livingDocExample);
                        AddScenarioExampleSteps(scenario, livingDocExample);
                    }
                }
            }
            else
            {
                var livingDocExample = new LivingDocExample();
                livingDocScenario.Examples.Add(livingDocExample);

                AddFeatureBackgroundSteps(livingDocFeature, livingDocExample);
                AddScenarioExampleSteps(scenario, livingDocExample);
            }
        }

        public static void AddScenarioExampleTableHeaders(Examples examples, LivingDocExample livingDocExample)
        {
            var dataTableRowHeader = new LivingDocDataTableRow();
            foreach (var headerCell in examples.TableHeader.Cells)
                dataTableRowHeader.Cells.Add(headerCell.Value);
            livingDocExample.DataTable.Rows.Add(dataTableRowHeader);
        }

        public static void AddScenarioExampleTableData(TableRow tableBodyRow, LivingDocExample livingDocExample)
        {
            var dataTableRow = new LivingDocDataTableRow();
            foreach (var tableBodyRowCell in tableBodyRow.Cells)
                dataTableRow.Cells.Add(tableBodyRowCell.Value);
            livingDocExample.DataTable.Rows.Add(dataTableRow);
        }

        public static void AddFeatureBackgroundSteps(LivingDocFeature livingDocFeature, LivingDocExample livingDocExample)
        {
            if (livingDocFeature.Background != null)
            {
                foreach (var backgroundStep in livingDocFeature.Background.Steps)
                    livingDocExample.Steps.Add(backgroundStep.Copy(backgroundStep));
            }
        }

        public static void AddScenarioExampleSteps(Scenario scenario, LivingDocExample livingDocExample)
        {
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
                        var dataTableRow = new LivingDocDataTableRow();
                        foreach (var cell in row.Cells)
                            dataTableRow.Cells.Add(cell.Value);
                        livingDocStep.DataTable.Rows.Add(dataTableRow);
                    }
                }

                livingDocExample.Steps.Add(livingDocStep);
            }
        }

        public static string CapitalizeWords(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(value, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }
    }
}
