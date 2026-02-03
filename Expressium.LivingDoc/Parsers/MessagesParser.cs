using Expressium.LivingDoc.Models;
using Io.Cucumber.Messages.Types;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Expressium.LivingDoc.Parsers
{
    internal class MessagesParser
    {
        private List<GherkinDocument> listOfGherkinDocuments;
        private List<Pickle> listOfPickles;
        private List<TestCase> listOfTestCases;
        private List<TestStepFinished> listOfTestStepFinished;
        private List<TestCaseStarted> listOfTestCaseStarted;
        private List<TestCaseFinished> listOfTestCaseFinished;
        private List<TestRunStarted> listOfTestRunStarted;
        private List<TestRunFinished> listOfTestRunFinished;
        private List<Attachment> listOftAttachment;

        internal MessagesParser()
        {
            listOfGherkinDocuments = new List<GherkinDocument>();
            listOfPickles = new List<Pickle>();
            listOfTestCases = new List<TestCase>();
            listOfTestStepFinished = new List<TestStepFinished>();
            listOfTestCaseStarted = new List<TestCaseStarted>();
            listOfTestCaseFinished = new List<TestCaseFinished>();
            listOfTestRunStarted = new List<TestRunStarted>();
            listOfTestRunFinished = new List<TestRunFinished>();
            listOftAttachment = new List<Attachment>();
        }

        internal LivingDocProject ConvertToLivingDoc(string filePath)
        {
            var livingDocProject = new LivingDocProject();
            livingDocProject.Title = "Expressium LivingDoc";

            ParseCucumberMessagesFile(filePath);
            ParseGherkinDocuments(livingDocProject);
            ParseTestResults(livingDocProject);
            PostProcessingProject(livingDocProject);

            return livingDocProject;
        }

        internal void ParseCucumberMessagesFile(string filePath)
        {
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
        }

        internal void ParseGherkinDocuments(LivingDocProject livingDocProject)
        {
            foreach (var gherkinDocument in listOfGherkinDocuments)
            {
                var uri = gherkinDocument.Uri;
                var feature = gherkinDocument.Feature;

                var livingDocFeature = new LivingDocFeature();

                ParseFeature(livingDocFeature, feature, uri);

                foreach (var child in feature.Children)
                {
                    if (child.Background != null)
                        ParseBackground(livingDocFeature, child.Background);

                    if (child.Rule != null)
                        ParseRule(livingDocFeature, child.Rule);

                    if (child.Scenario != null)
                        ParseScenario(livingDocFeature, child.Scenario);
                }

                livingDocProject.Features.Add(livingDocFeature);
            }
        }

        internal static void ParseFeature(LivingDocFeature livingDocFeature, Feature feature, string uri)
        {
            if (feature.Tags != null)
            {
                foreach (var tag in feature.Tags)
                    livingDocFeature.Tags.Add(tag.Name);
            }

            livingDocFeature.Description = feature.Description;
            livingDocFeature.Uri = uri;
            livingDocFeature.Name = feature.Name;
            livingDocFeature.Keyword = feature.Keyword;
        }

        internal static void ParseBackground(LivingDocFeature livingDocFeature, Background background)
        {
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
                livingDocStep.Type = LivingDocStepTypes.Background.ToString();
                livingDocBackground.Steps.Add(livingDocStep);
            }

            livingDocFeature.Background = livingDocBackground;
        }

        internal static void ParseRule(LivingDocFeature livingDocFeature, Rule rule)
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

            foreach (var ruleChild in rule.Children)
            {
                if (ruleChild.Scenario == null)
                    continue;

                ParseScenario(livingDocFeature, ruleChild.Scenario, rule.Id);
            }
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
                foreach (var example in scenario.Examples)
                {
                    int tableIndexId = 1;
                    foreach (var tableBodyRow in example.TableBody)
                    {
                        var livingDocExample = new LivingDocExample();
                        livingDocExample.Name = example.Name;
                        livingDocExample.Description = example.Description;
                        livingDocScenario.Examples.Add(livingDocExample);

                        ParseScenarioBackgroundSteps(livingDocExample, livingDocFeature, tableIndexId++);
                        ParseScenarioExampleTableSteps(livingDocExample, scenario, tableBodyRow.Id);
                        ParseScenarioExampleTableHeaders(livingDocExample, example);
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

        internal static void ParseScenarioBackgroundSteps(LivingDocExample livingDocExample, LivingDocFeature livingDocFeature, int tableIndexId = -1)
        {
            if (livingDocFeature.Background != null)
            {
                foreach (var backgroundStep in livingDocFeature.Background.Steps)
                {
                    var copy = backgroundStep.Copy(backgroundStep);
                    copy.TableIndexId = tableIndexId;
                    livingDocExample.Steps.Add(copy);
                }
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

        internal static void ParseScenarioExampleTableSteps(LivingDocExample livingDocExample, Scenario scenario, string tableBodyRowId)
        {
            foreach (var step in scenario.Steps)
            {
                var livingDocStep = new LivingDocStep();
                livingDocStep.Id = step.Id;
                livingDocStep.TableBodyId = tableBodyRowId;
                livingDocStep.Name = WebUtility.HtmlEncode(step.Text);
                livingDocStep.Type = LivingDocStepTypes.Scenario.ToString();
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

        internal void ParseTestResults(LivingDocProject livingDocProject)
        {
            // Assign Test Execution Date...
            var testRunStarted = listOfTestRunStarted.FirstOrDefault();
            livingDocProject.Date = testRunStarted.Timestamp.ToDateTime();

            // Assign Test Execution Duration...
            var testRunFinished = listOfTestRunFinished.Last();
            livingDocProject.Duration = testRunStarted.Timestamp.ToTimeSpan(testRunFinished.Timestamp);

            // Assign Scenario Test Results....
            ParseTestResultsScenarios(livingDocProject);
        }

        internal void ParseTestResultsScenarios(LivingDocProject livingDocProject)
        {
            foreach (var feature in livingDocProject.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        string testCaseStartedId = null;

                        // Assign Scenario Example Test Step Results...
                        foreach (var step in example.Steps)
                        {
                            var pickleStep = GetPickleStep(scenario, step);
                            if (pickleStep == null)
                                continue;

                            var testStep = listOfTestCases.SelectMany(p => p.TestSteps).FirstOrDefault(s => s.PickleStepId == pickleStep.Id);
                            if (testStep == null)
                                continue;

                            var testStepFinished = listOfTestStepFinished.Find(f => f.TestStepId == testStep.Id);
                            if (testStepFinished == null)
                                continue;

                            // Assign Scenario Step Test Results...
                            ParseTestResultsSteps(step, testStepFinished);

                            testCaseStartedId = testStepFinished.TestCaseStartedId;
                        }

                        // Find Scenario TestCaseStarted...
                        var testCaseStarted = listOfTestCaseStarted.Find(g => g.Id == testCaseStartedId);
                        if (testCaseStarted == null)
                            continue;

                        // Find Scenario TestCaseFinished...
                        var testCaseFinished = listOfTestCaseFinished.Find(j => j.TestCaseStartedId == testCaseStarted.Id);
                        if (testCaseFinished == null)
                            continue;

                        // Assign Scenario Duration...
                        example.Duration = testCaseStarted.Timestamp.ToTimeSpan(testCaseFinished.Timestamp);

                        // Assign Scenario Attachments...
                        var attachments = listOftAttachment.FindAll(a => a.TestCaseStartedId.Contains(testCaseStarted.Id));
                        if (attachments.Count > 0)
                        {
                            foreach (var attachment in attachments)
                                ParseTestResultsAttachments(example, attachment);
                        }
                    }
                }
            }

            // Assign Scenario Execution Order...
            int orderId = 1;
            foreach (var pickle in listOfPickles)
            {
                var astNodeId = pickle.AstNodeIds.FirstOrDefault();

                var scenario = livingDocProject.Features
                    .SelectMany(feature => feature.Scenarios)
                    .FirstOrDefault(s => s.Id == astNodeId);

                if (scenario != null)
                    if (scenario.Order == 0)
                        scenario.Order = orderId++;
            }
        }

        internal PickleStep GetPickleStep(LivingDocScenario scenario, LivingDocStep step)
        {
            if (step.TableBodyId != null)
            {
                // Normal Step in Scenario with Examples...
                var pickle = listOfPickles.Find(x => x.AstNodeIds.Contains(scenario.Id) && x.AstNodeIds.Contains(step.TableBodyId));
                if (pickle == null)
                    return null;

                return pickle.Steps.FirstOrDefault(s => s.AstNodeIds.Contains(step.Id) && s.AstNodeIds.Contains(step.TableBodyId));
            }
            else if (step.TableIndexId != -1)
            {
                // Background Step in Scenario with Examples...
                var scenarioPickles = listOfPickles.Where(x => x.AstNodeIds.Contains(scenario.Id));
                if (!scenarioPickles.Any())
                    return null;

                var stepPickles = scenarioPickles.SelectMany(p => p.Steps).Where(s => s.AstNodeIds.Contains(step.Id));
                if (!stepPickles.Any())
                    return null;

                if (stepPickles.Count() >= step.TableIndexId)
                    return stepPickles.ElementAt(step.TableIndexId - 1);
            }
            else
            {
                // Normal Step in Scenario...
                var pickle = listOfPickles.Find(x => x.AstNodeIds.Contains(scenario.Id));
                if (pickle == null)
                    return null;

                return pickle.Steps.FirstOrDefault(s => s.AstNodeIds.Contains(step.Id));
            }

            return null;
        }

        internal static void ParseTestResultsSteps(LivingDocStep livingDocStep, TestStepFinished testStepFinished)
        {
            livingDocStep.Status = testStepFinished.TestStepResult.Status.ToString().ToLower().CapitalizeWords();
            livingDocStep.Message = WebUtility.HtmlEncode(testStepFinished.TestStepResult.Message);

            if (testStepFinished.TestStepResult.Exception != null)
            {
                livingDocStep.ExceptionType = testStepFinished.TestStepResult.Exception.Type;
                livingDocStep.ExceptionMessage = WebUtility.HtmlEncode(testStepFinished.TestStepResult.Exception.Message);
                livingDocStep.ExceptionStackTrace = testStepFinished.TestStepResult.Exception.StackTrace;
            }
        }

        internal static void ParseTestResultsAttachments(LivingDocExample livingDocExample, Attachment attachment)
        {
            if (attachment.MediaType == "text/uri-list")
                livingDocExample.Attachments.Add(attachment.Body);
            else if (attachment.MediaType == "text/x.cucumber.log+plain" && attachment.ContentEncoding.ToString() == "IDENTITY")
            {
                if (attachment.Body.StartsWith("[Attachment: ") && attachment.Body.EndsWith("]"))
                {
                    var attachmentFile = attachment.Body.Substring(13, attachment.Body.Length - 14);
                    livingDocExample.Attachments.Add(attachmentFile);
                }
            }
        }

        internal static void PostProcessingProject(LivingDocProject livingDocProject)
        {
            var examples = livingDocProject.Features.SelectMany(feature => feature.Scenarios).SelectMany(scenario => scenario.Examples);

            // Work-around for Failing, Pending, Undefined and Ambiguous Step Messages...
            foreach (var example in examples)
            {
                foreach (var step in example.Steps)
                {
                    if (step.Status == LivingDocStatuses.Failed.ToString())
                    {
                        if (!string.IsNullOrEmpty(step.ExceptionType) && !string.IsNullOrEmpty(step.ExceptionMessage))
                        {
                            step.Message = null;
                        }
                    }
                    else if (step.Status == LivingDocStatuses.Pending.ToString())
                    {
                        step.Message = null;
                        step.ExceptionType = "Warning";
                        step.ExceptionMessage = "Pending Step Definition...";
                        step.ExceptionStackTrace = null;
                    }
                    else if (step.Status == LivingDocStatuses.Undefined.ToString())
                    {
                        step.Message = null;
                        step.ExceptionType = "Warning";
                        step.ExceptionMessage = "Undefined Step Definition...";
                        step.ExceptionStackTrace = null;
                    }
                    else if (step.Status == LivingDocStatuses.Ambiguous.ToString())
                    {
                        step.Message = null;
                        step.ExceptionType = "Warning";
                        step.ExceptionMessage = "Ambiguous Step Definition...";
                        step.ExceptionStackTrace = null;
                    }
                }
            }
        }
    }
}
