using Expressium.LivingDoc.Models;
using Io.Cucumber.Messages.Types;
using System;
using System.Linq;
using System.Net;

namespace Expressium.LivingDoc.Parsers
{
    internal class MessagesResultParser
    {
        internal void ParseTestResults(CucumberMessages messages, LivingDocProject livingDocProject)
        {
            // Assign Test Execution Environment Details...
            var meta = messages.Metas.FirstOrDefault();
            if (meta == null)
                throw new InvalidOperationException("No Meta message found in the Cucumber messages file. The file may be empty or malformed.");

            livingDocProject.ProtocolVersion = meta.ProtocolVersion;
            livingDocProject.ImplementationName = meta.Implementation.Name;
            livingDocProject.ImplementationVersion = meta.Implementation.Version;
            livingDocProject.RuntimeName = meta.Runtime.Name;
            livingDocProject.RuntimeVersion = meta.Runtime.Version;
            livingDocProject.OsName = meta.Os.Name;
            livingDocProject.OsVersion = meta.Os.Version;
            livingDocProject.CpuName = meta.Cpu.Name;

            // Assign Test Execution Date...
            var testRunStarted = messages.TestRunStarted.FirstOrDefault();
            if (testRunStarted == null)
                throw new InvalidOperationException("No TestRunStarted message found. The test run may not have started or the file is incomplete.");
            livingDocProject.Date = testRunStarted.Timestamp.ToDateTime();

            // Assign Test Execution Duration...
            var testRunFinished = messages.TestRunFinished.LastOrDefault();
            if (testRunFinished == null)
                throw new InvalidOperationException("No TestRunFinished message found. The test run may have been interrupted before completing.");
            livingDocProject.Duration = testRunStarted.Timestamp.ToTimeSpan(testRunFinished.Timestamp);

            // Assign Scenario Test Results....
            ParseTestResultsScenarios(messages, livingDocProject);

            // Assign Scenario Execution Order...
            int orderId = 1;
            foreach (var pickle in messages.Pickles)
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

        internal void ParseTestResultsScenarios(CucumberMessages messages, LivingDocProject livingDocProject)
        {
            // Build lookup dictionaries for steps...
            var testStepByPickleStepId = messages.TestCases
                .SelectMany(p => p.TestSteps)
                .Where(s => s.PickleStepId != null)
                .GroupBy(s => s.PickleStepId)
                .ToDictionary(g => g.Key, g => g.First());

            var testStepFinishedByTestStepId = messages.TestStepFinished
                .GroupBy(f => f.TestStepId)
                .ToDictionary(g => g.Key, g => g.First());

            var testCaseStartedById = messages.TestCaseStarted
                .Where(t => t.Id != null)
                .GroupBy(t => t.Id)
                .ToDictionary(g => g.Key, g => g.First());

            var testCaseFinishedByStartedId = messages.TestCaseFinished
                .Where(t => t.TestCaseStartedId != null)
                .GroupBy(t => t.TestCaseStartedId)
                .ToDictionary(g => g.Key, g => g.First());

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
                            var pickleStep = GetPickleStep(messages, scenario, step);
                            if (pickleStep == null)
                                continue;

                            // Assign actual Step Text from PickleStep... 
                            if (!string.IsNullOrWhiteSpace(pickleStep.Text))
                                step.Name = WebUtility.HtmlEncode(pickleStep.Text);

                            if (!testStepByPickleStepId.TryGetValue(pickleStep.Id, out var testStep))
                                continue;

                            if (!testStepFinishedByTestStepId.TryGetValue(testStep.Id, out var testStepFinished))
                                continue;

                            // Assign Scenario Step Test Results...
                            ParseTestResultsSteps(step, testStepFinished);

                            testCaseStartedId = testStepFinished.TestCaseStartedId;
                        }

                        // Find Scenario TestCaseStarted...
                        if (testCaseStartedId == null ||
                            !testCaseStartedById.TryGetValue(testCaseStartedId, out var testCaseStarted))
                            continue;

                        // Find Scenario TestCaseFinished...
                        if (!testCaseFinishedByStartedId.TryGetValue(testCaseStarted.Id, out var testCaseFinished))
                            continue;

                        // Asign Before and After Hook Failures...
                        ParseHookTestResults(messages, example, testCaseStarted);

                        // Assign Scenario Duration...
                        example.Duration = testCaseStarted.Timestamp.ToTimeSpan(testCaseFinished.Timestamp);

                        // Assign Scenario Attachments...
                        if (testCaseStarted.Id != null)
                        {
                            var attachments = messages.Attachments.Where(a => a.TestCaseStartedId != null && a.TestCaseStartedId.Contains(testCaseStarted.Id)).ToList();
                            if (attachments.Count > 0)
                            {
                                foreach (var attachment in attachments)
                                    ParseTestResultsAttachments(example, attachment);
                            }
                        }
                    }
                }
            }
        }

        internal void ParseHookTestResults(CucumberMessages messages, LivingDocExample example, TestCaseStarted testCaseStarted)
        {
            if (example.Steps.Count == 0)
                return;

            if (example.IsFailed())
                return;

            var testCase = messages.TestCases.FirstOrDefault(x => x.Id == testCaseStarted.TestCaseId);
            if (testCase == null)
                return;

            var hookTestSteps = testCase.TestSteps.Where(g => g.HookId != null);
            foreach (var hookTestStep in hookTestSteps)
            {
                var testStepFinished = messages.TestStepFinished.FirstOrDefault(k => k.TestStepId == hookTestStep.Id);
                if (testStepFinished != null)
                {
                    if (testStepFinished.TestStepResult.Exception != null)
                    {
                        var hook = messages.Hooks.FirstOrDefault(d => d.Id == hookTestStep.HookId);
                        if (hook != null)
                        {
                            if (hook.Type == HookType.BEFORE_TEST_CASE)
                                example.Steps.Insert(0, CreateHookStep("Before Scenario", testStepFinished));

                            if (hook.Type == HookType.AFTER_TEST_CASE)
                                example.Steps.Add(CreateHookStep("After Scenario", testStepFinished));
                        }
                    }
                }
            }
        }

        private static LivingDocStep CreateHookStep(string name, TestStepFinished testStepFinished)
        {
            return new LivingDocStep
            {
                Name = name,
                Keyword = "Hook",
                Type = LivingDocStepTypes.Hook.ToString(),
                Status = LivingDocStatuses.Failed.ToString(),
                ExceptionType = testStepFinished.TestStepResult.Exception.Type,
                ExceptionMessage = testStepFinished.TestStepResult.Exception.Message,
                ExceptionStackTrace = testStepFinished.TestStepResult.Exception.StackTrace
            };
        }

        internal PickleStep GetPickleStep(CucumberMessages messages, LivingDocScenario scenario, LivingDocStep step)
        {
            if (step.TableBodyId != null)
                return GetPickleStepForExampleTableStep(messages, scenario, step);

            if (step.TableIndexId != -1)
                return GetPickleStepForExampleBackgroundStep(messages, scenario, step);

            return GetPickleStepForScenarioStep(messages, scenario, step);
        }

        private PickleStep GetPickleStepForScenarioStep(CucumberMessages messages, LivingDocScenario scenario, LivingDocStep step)
        {
            // Normal Step in Scenario (no Examples table)...
            var pickle = messages.Pickles.FirstOrDefault(x => x.AstNodeIds.Contains(scenario.Id));
            if (pickle == null)
                return null;

            return pickle.Steps.FirstOrDefault(s => s.AstNodeIds.Contains(step.Id));
        }

        private PickleStep GetPickleStepForExampleTableStep(CucumberMessages messages, LivingDocScenario scenario, LivingDocStep step)
        {
            // Normal Step in Scenario with Examples table...
            var pickle = messages.Pickles.FirstOrDefault(x => x.AstNodeIds.Contains(scenario.Id) && x.AstNodeIds.Contains(step.TableBodyId));
            if (pickle == null)
                return null;

            return pickle.Steps.FirstOrDefault(s => s.AstNodeIds.Contains(step.Id) && s.AstNodeIds.Contains(step.TableBodyId));
        }

        private PickleStep GetPickleStepForExampleBackgroundStep(CucumberMessages messages, LivingDocScenario scenario, LivingDocStep step)
        {
            // Background Step in Scenario with Examples table...
            var scenarioPickles = messages.Pickles.Where(x => x.AstNodeIds.Contains(scenario.Id));
            if (!scenarioPickles.Any())
                return null;

            var stepPickles = scenarioPickles.SelectMany(p => p.Steps).Where(s => s.AstNodeIds.Contains(step.Id)).ToList();
            if (!stepPickles.Any())
                return null;

            if (stepPickles.Count >= step.TableIndexId)
                return stepPickles[step.TableIndexId - 1];

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

            // Work-around for Failing, Pending, Undefined and Ambiguous Step Messages...
            if (livingDocStep.Status == LivingDocStatuses.Pending.ToString())
            {
                livingDocStep.Message = null;
                livingDocStep.ExceptionType = "Warning";
                livingDocStep.ExceptionMessage = "Pending Step Definition...";
                livingDocStep.ExceptionStackTrace = null;
            }
            else if (livingDocStep.Status == LivingDocStatuses.Undefined.ToString())
            {
                livingDocStep.Message = null;
                livingDocStep.ExceptionType = "Warning";
                livingDocStep.ExceptionMessage = "Undefined Step Definition...";
                livingDocStep.ExceptionStackTrace = null;
            }
            else if (livingDocStep.Status == LivingDocStatuses.Ambiguous.ToString())
            {
                livingDocStep.Message = null;
                livingDocStep.ExceptionType = "Warning";
                livingDocStep.ExceptionMessage = "Ambiguous Step Definition...";
                livingDocStep.ExceptionStackTrace = null;
            }

            // Work-around for duplicated step messages...
            if (!string.IsNullOrEmpty(livingDocStep.ExceptionType) && !string.IsNullOrEmpty(livingDocStep.ExceptionMessage))
                livingDocStep.Message = null;
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
    }
}
