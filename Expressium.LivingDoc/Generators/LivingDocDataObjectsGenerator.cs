using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataObjectsGenerator
    {
        private LivingDocProject project;

        internal LivingDocDataObjectsGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> GenerateDataFeatures()
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                listOfLines.Add("<!-- Data Feature -->");
                listOfLines.Add($"<div id='{feature.Id}'>");

                listOfLines.Add("<!-- Feature Section -->");
                listOfLines.Add("<div class='section'>");
                listOfLines.AddRange(GenerateDataFeatureTags(feature));
                listOfLines.AddRange(GenerateDataFeatureName(feature));
                listOfLines.AddRange(GenerateDataFeatureDescription(feature));
                listOfLines.AddRange(GenerateDataFeatureBackground(feature));
                listOfLines.Add("</div>");

                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureTags(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Feature Tags -->");
            listOfLines.Add("<div class='feature-tag-group'>");

            foreach (var tag in feature.Tags)
                listOfLines.Add("<span class='feature-tag'>" + tag + "</span>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureName(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Feature Name -->");
            listOfLines.Add("<div>");

            var status = feature.GetStatus().ToLower();
            if (project.ExperimentFlagSymbols)
            {
                var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(status);
                listOfLines.Add($"<span class='{symbol} color-{status} status-symbol'></span>");
            }
            else
                listOfLines.Add($"<span class='status-dot bgcolor-{status}'></span>");

            listOfLines.Add($"<span class='feature-keyword'>Feature: </span><span class='feature-name'>{feature.Name}</span>");
            //listOfLines.Add($"<span class='feature-duration'>{feature.GetDuration()}</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureDescription(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            if (feature.Description != null)
            {
                listOfLines.Add("<!-- Data Feature Description -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<ul class='feature-description'>");

                var listOfDescription = feature.Description.Trim().Split("\n");
                foreach (var line in listOfDescription)
                    listOfLines.Add("<li>" + line.Trim() + "</li>");

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            listOfLines.Add("<hr>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureBackground(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            if (feature.Background != null && feature.Background.Steps.Count > 0)
            {
                listOfLines.Add("<!-- Data Feature Background -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<span class='background-keyword'>Background:</span>");
                listOfLines.AddRange(GenerateDataFeatureBackgroundSteps(feature.Background.Steps));
                listOfLines.Add("</div>");

                listOfLines.Add("<hr>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureBackgroundSteps(List<LivingDocStep> steps)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Background Steps -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<ul class='scenario-steps'>");

            foreach (var step in steps)
            {
                listOfLines.Add("<li>");
                listOfLines.Add($"<span class='color-skipped'></span>");
                listOfLines.Add($"<span class='step-keyword'>" + step.Keyword + "</span>");
                listOfLines.Add($"<span class='step-name'>" + step.Name + "</span>");
                listOfLines.Add("</li>");
            }

            listOfLines.Add("</ul>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarios()
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                var previousRule = string.Empty;
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<!-- Data Scenario -->");
                    listOfLines.Add($"<div id='{scenario.Id}'>");

                    if (!string.IsNullOrEmpty(scenario.RuleId))
                    {
                        var rule = feature.Rules.Find(r => r.Id == scenario.RuleId);
                        listOfLines.AddRange(GenerateDataScenarioRule(rule, previousRule));
                        previousRule = scenario.RuleId;
                    }

                    int index = 1;
                    foreach (var example in scenario.Examples)
                    {
                        string indexId = string.Empty;
                        if (example.HasDataTable())
                            indexId = index.ToString();
                        index++;

                        listOfLines.Add("<!-- Scenario Section -->");
                        listOfLines.Add("<div class='section' style='width: fit-content; max-width: 98%'>");
                        listOfLines.AddRange(GenerateDataScenarioTags(scenario));
                        listOfLines.AddRange(GenerateDataScenarioName(scenario, example, indexId));
                        listOfLines.AddRange(GenerateDataScenarioDescription(scenario));
                        listOfLines.AddRange(GenerateDataScenarioSteps(example));
                        listOfLines.AddRange(GenerateDataScenarioExamples(example));
                        listOfLines.AddRange(GenerateDataScenarioAttachments(example));
                        listOfLines.AddRange(GenerateDataScenarioHistory(example));
                        listOfLines.Add("</div>");
                        listOfLines.Add("<hr>");
                    }

                    listOfLines.Add("</div>");
                }
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioRule(LivingDocRule rule, string previousRule)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Rule Section -->");
            if (previousRule != rule.Id)
                listOfLines.Add("<div class='section'>");
            else
                listOfLines.Add("<div class='section' data-rule-replica>");

            listOfLines.AddRange(GenerateDataRuleTags(rule));
            listOfLines.AddRange(GenerateDataRuleName(rule));
            listOfLines.AddRange(GenerateDataRuleDescription(rule));

            listOfLines.Add("<hr>");
            listOfLines.Add("</div>");


            return listOfLines;
        }

        internal List<string> GenerateDataRuleTags(LivingDocRule rule)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Rule Tags -->");
            listOfLines.Add("<div class='rule-tag-group'>");

            foreach (var tag in rule.Tags)
                listOfLines.Add("<span class='rule-tag'>" + tag + "</span>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataRuleName(LivingDocRule rule)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Rule Name -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='rule-keyword'>Rule: </span>");
            listOfLines.Add("<span class='rule-name'>" + rule.Name + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataRuleDescription(LivingDocRule rule)
        {
            var listOfLines = new List<string>();

            if (!string.IsNullOrWhiteSpace(rule.Description))
            {
                listOfLines.Add("<!-- Data Rule Description -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<ul class='rule-description'>");

                var listOfDescription = rule.Description.Trim().Split("\n");
                foreach (var line in listOfDescription)
                    listOfLines.Add("<li>" + line.Trim() + "</li>");

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioTags(LivingDocScenario scenario)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Tags -->");
            listOfLines.Add("<div class='scenario-tag-group'>");

            foreach (var tag in scenario.Tags)
                listOfLines.Add("<span class='scenario-tag'>" + tag + "</span>");

            if (scenario.HasHealth())
                listOfLines.Add($"<span class='scenario-tag-health color-{scenario.Health.ToLower()}'>@{scenario.Health}</span>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioName(LivingDocScenario scenario, LivingDocExample example, string indexId)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Name -->");
            listOfLines.Add("<div>");

            var status = example.GetStatus().ToLower();
            if (project.ExperimentFlagSymbols)
            {
                var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(status);
                listOfLines.Add($"<span class='{symbol} color-{status} status-symbol'></span>");
            }
            else
                listOfLines.Add($"<span class='status-dot bgcolor-{status}'></span>");

            listOfLines.Add("<span class='scenario-keyword'>Scenario: </span>");
            listOfLines.Add("<span class='scenario-name'>" + scenario.Name + "</span>");

            if (!string.IsNullOrEmpty(indexId))
                listOfLines.Add($"<span class='scenario-index'>{indexId}</span>");

            listOfLines.Add($"<span class='scenario-duration'>{example.GetDuration()}</span>");

            ///////////////////////////////////////////////////////
            // Toggle option for visibility of Background steps...
            ///////////////////////////////////////////////////////
            //var hasBackgrounds = example.Steps?.Any(x => x.Type == LivingDocStepTypes.Background.ToString()) ?? false;
            //if (hasBackgrounds)
            //    listOfLines.Add("<button class='scenario-backgrounds bi bi-chevron-double-down' title='Toggle Backgrounds' onclick=\"toggleBackgrounds(this)\"></button>");
            ///////////////////////////////////////////////////////

            var hasStacktraces = example.Steps?.Any(x => x.ExceptionStackTrace != null) ?? false;
            if (hasStacktraces)
                listOfLines.Add("<button class='scenario-stacktraces bi bi-code-slash' title='Toggle Stacktrace' onclick=\"toggleStacktraces(this)\"></button>");

            if (example.Attachments.Count > 0)
                listOfLines.Add("<button class='scenario-attachments bi bi-list' title='Toggle Attachments' onclick=\"toggleAttachments(this)\"></button>");

            if (example.History.Count > 0)
                listOfLines.Add("<button class='scenario-history bi bi-calendar4' title='Toggle History' onclick=\"toggleHistory(this)\"></button>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioDescription(LivingDocScenario scenario)
        {
            var listOfLines = new List<string>();

            if (!string.IsNullOrWhiteSpace(scenario.Description))
            {
                listOfLines.Add("<!-- Data Scenario Description -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<ul class='scenario-description'>");

                var listOfDescription = scenario.Description.Trim().Split("\n");
                foreach (var line in listOfDescription)
                    listOfLines.Add("<li>" + line.Trim() + "</li>");

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioSteps(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Steps -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<ul class='scenario-steps'>");

            foreach (var step in example.Steps)
            {
                listOfLines.AddRange(GenerateDataScenarioStep(step));
                listOfLines.AddRange(GenerateDataScenarioStepMessage(step));
                listOfLines.AddRange(GenerateDataScenarioStepExceptionMessage(step));
                listOfLines.AddRange(GenerateDataScenarioStepExceptionStackTrace(step));
            }

            listOfLines.Add("</ul>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioStep(LivingDocStep step)
        {
            var listOfLines = new List<string>();

            var status = step.GetStatus().ToLower();

            ///////////////////////////////////////////////////////
            // Toggle option for visibility of Background steps...
            ///////////////////////////////////////////////////////
            //if (step.Type == LivingDocStepTypes.Background.ToString())
            //{
            //    if (isPassed)
            //        listOfLines.Add($"<li class='backgrounds' style='display: none;'>");
            //    else
            //        listOfLines.Add($"<li class='backgrounds' style='display: block;'>");
            //}
            //else
            ///////////////////////////////////////////////////////

            listOfLines.Add("<li>");

            if (project.ExperimentFlagSymbols)
            {
                var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(status);
                listOfLines.Add($"<span class='{symbol} color-{status} status-symbol'></span>");
            }
            else
            {
                var symbol = step.IsPassed() ? "&check;" : "&cross;";
                listOfLines.Add($"<span class='step-symbol color-{status}'>{symbol}</span>");
            }

            listOfLines.Add($"<span class='step-keyword'>{step.Keyword}</span>");
            listOfLines.Add($"<span class='step-name'>{step.Name}</span>");

            if (step.DataTable.Rows.Count > 0)
            {
                listOfLines.Add("<!-- Scenario Steps Data Table Section -->");
                listOfLines.Add($"<div class='steps-datatable'>");
                listOfLines.AddRange(GenerateDataScenarioDataTable(step.DataTable));
                listOfLines.Add("</div>");
            }

            listOfLines.Add("</li>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioStepMessage(LivingDocStep step)
        {
            var listOfLines = new List<string>();

            var message = step.Message;
            if (!string.IsNullOrWhiteSpace(message))
            {
                listOfLines.Add("<!-- Data Step Message -->");
                listOfLines.Add("<li>");
                listOfLines.Add($"<div class='message-box'>");
                listOfLines.Add($"<div class='message-skipped'>{message.Trim().Replace("\n", "<br>")}</div>");
                listOfLines.Add("</div>");
                listOfLines.Add("</li>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioStepExceptionMessage(LivingDocStep step)
        {
            var listOfLines = new List<string>();

            var exceptionType = step.ExceptionType;
            var exceptionMessage = step.ExceptionMessage;
            if (!string.IsNullOrWhiteSpace(exceptionType) && !string.IsNullOrWhiteSpace(exceptionMessage))
            {
                listOfLines.Add("<!-- Data Step Message -->");
                listOfLines.Add("<li>");
                listOfLines.Add($"<div class='message-box'>");
                listOfLines.Add($"<div class='message-{step.GetStatus().ToLower()}'>");
                listOfLines.Add($"<span class='message-header'>{exceptionType}</span><br>");
                listOfLines.Add($"{exceptionMessage.Replace("\n", "<br>")}<br>");
                listOfLines.Add("</div>");
                listOfLines.Add("</div>");
                listOfLines.Add("</li>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioStepExceptionStackTrace(LivingDocStep step)
        {
            var listOfLines = new List<string>();

            var exceptionStackTrace = step.ExceptionStackTrace;
            if (!string.IsNullOrWhiteSpace(exceptionStackTrace))
            {
                listOfLines.Add("<!-- Data Step Message -->");
                listOfLines.Add("<li class='stacktraces' style='display : none;'>");
                listOfLines.Add($"<div class='message-box'>");
                listOfLines.Add($"<div class='message-{step.GetStatus().ToLower()}'>");
                listOfLines.Add($"<span class='message-header'>Stacktrace</span><br>");
                listOfLines.Add($"<div class='message-stacktrace'>{exceptionStackTrace.Replace("\n", "<br>")}<br></div>");
                listOfLines.Add("</div>");
                listOfLines.Add("</div>");
                listOfLines.Add("</li>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioExamples(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.HasDataTable())
            {
                listOfLines.Add("<!-- Data Scenario Examples -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<span class='examples-keyword'>Examples: </span>");
                //listOfLines.Add($"<span class='examples-name'>{example.Name}</span>");
                if (!string.IsNullOrEmpty(example.Description))
                    listOfLines.Add($"<br><span class='examples-description'>{example.Description}</span>");
                listOfLines.Add("<div class='examples-datatable'>");
                listOfLines.AddRange(GenerateDataScenarioDataTable(example.DataTable));
                listOfLines.Add("</div>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioDataTable(LivingDocDataTable dataTable)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<table class='scenario-datatable'>");
            listOfLines.Add("<tbody>");

            foreach (var row in dataTable.Rows)
            {
                var numberOfCells = row.Cells.Count;
                int i = 1;
                listOfLines.Add($"<tr>");

                foreach (var cell in row.Cells)
                {
                    listOfLines.Add($"<td>|</td>");
                    listOfLines.Add($"<td>" + cell + "</td>");

                    if (i == numberOfCells)
                        listOfLines.Add($"<td>|</td>");

                    i++;
                }
                listOfLines.Add($"</tr>");
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioAttachments(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.Attachments.Count > 0)
            {
                listOfLines.Add("<!-- Data Scenario Attachments -->");
                listOfLines.Add($"<div class='attachments' style='display: none;'>");
                listOfLines.Add($"<span class='attachments-keyword'>Attachments</span>");
                listOfLines.Add("<ul class='attachments-files'>");

                foreach (var attachment in example.Attachments)
                {
                    var filePath = attachment;
                    if (!Uri.IsWellFormedUriString(attachment, UriKind.Absolute))
                        filePath = Path.GetFileName(attachment);

                    listOfLines.Add($"<li><span class='bi bi-file-text attachment-symbol'></span><a target='_blank' href='{attachment}'>{filePath}</a></li>");
                }

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioHistory(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.History.Count > 0)
            {
                listOfLines.Add("<!-- Data Scenario History -->");
                listOfLines.Add($"<div class='history' style='display: none;'>");
                listOfLines.Add($"<span class='history-keyword'>History</span>");
                listOfLines.Add("<ul class='history-files'>");

                foreach (var history in example.History)
                    listOfLines.Add($"<li><span class='status-dot bgcolor-{history.Status.ToLower()}'></span> {history.GetDate()}</li>");

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }
    }
}
