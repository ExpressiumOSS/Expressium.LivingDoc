using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc
{
    internal class LivingDocDataObjectsGenerator
    {
        internal List<string> Generate(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateDataFeatures(project));
            listOfLines.AddRange(GenerateDataScenarios(project));

            return listOfLines;
        }

        internal List<string> GenerateDataFeatures(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                listOfLines.Add("<!-- Data Feature -->");
                listOfLines.Add($"<div class='data-item' id='{feature.Id}'>");

                listOfLines.Add("<!-- Feature Section -->");
                listOfLines.Add("<div class='section'>");
                listOfLines.AddRange(GenerateDataFeaturesTags(feature));
                listOfLines.AddRange(GenerateDataFeatureName(feature));
                listOfLines.AddRange(GenerateDataFeatureDescription(feature));
                listOfLines.AddRange(GenerateDataFeatureBackground(feature));
                listOfLines.Add("</div>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataFeaturesTags(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Feature Tags -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='tag-names'>" + feature.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureName(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Feature Name -->");
            listOfLines.Add("<div>");
            listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<span class='feature-keyword'>Feature: </span><span class='feature-name'>{feature.Name}</span>");
            //listOfLines.Add($"<span class='duration'>&nbsp;{feature.GetDuration()}</span>");
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
                listOfLines.Add("<hr>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureBackgroundSteps(List<LivingDocStep> steps)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Background Steps -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<ul class='scenario-steps'>");

            string previousKeyword = null;
            foreach (var step in steps)
            {
                var keyword = step.Keyword;
                if (keyword == previousKeyword)
                    keyword = "And";

                listOfLines.Add("<li>");
                listOfLines.Add($"<span class='color-skipped'></span>");
                listOfLines.Add($"<span class='step-keyword'>" + keyword + "</span>");
                listOfLines.Add($"<span>" + step.Name + "</span>");
                listOfLines.Add("</li>");

                previousKeyword = step.Keyword;
            }

            listOfLines.Add("</ul>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarios(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                var previousRule = string.Empty;
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<!-- Data Scenario -->");
                    listOfLines.Add($"<div class='data-item' id='{scenario.Id}'>");

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
                        listOfLines.Add("<div class='section' style='width: fit-content;'>");
                        listOfLines.AddRange(GenerateDataScenarioTags(scenario));
                        listOfLines.AddRange(GenerateDataScenarioName(scenario, example, indexId));
                        listOfLines.AddRange(GenerateDataScenarioSteps(example.Steps));
                        listOfLines.AddRange(GenerateDataScenarioExamples(example));
                        listOfLines.AddRange(GenerateDataScenarioAttachments(example));
                        listOfLines.Add("<hr>");
                        listOfLines.Add("</div>");
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

            listOfLines.Add("<!-- Data Rule Name -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='rule-keyword'>Rule: </span>");
            listOfLines.Add("<span class='rule-name'>" + rule.Name + "</span>");
            listOfLines.Add("</div>");
            listOfLines.Add("<hr>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataRuleTags(LivingDocRule rule)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Rule Tags -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='tag-names'>" + rule.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioTags(LivingDocScenario scenario)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Tags -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='tag-names'>" + scenario.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioName(LivingDocScenario scenario, LivingDocExample example, string indexId)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Name -->");
            listOfLines.Add("<div>");
            listOfLines.Add($"<span class='status-dot bgcolor-{example.GetStatus().ToLower()}'></span>");
            listOfLines.Add("<span class='scenario-keyword'>Scenario: </span>");
            listOfLines.Add("<span class='scenario-name'>" + scenario.Name + "</span>");

            if (!string.IsNullOrEmpty(indexId))
                listOfLines.Add($"<span class='circle-number'>{indexId}</span>");

            listOfLines.Add($"<span class='duration'>&nbsp;{example.GetDuration()}</span>");

            if (example.Attachments.Count > 0)
                listOfLines.Add("<span style='color: gray;' title='Toggle Attachments' onclick=\"toggleAttachments(this)\">&nbsp;&#9776;</span>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioSteps(List<LivingDocStep> steps)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Steps -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<ul class='scenario-steps'>");

            string previousKeyword = null;
            foreach (var step in steps)
            {
                var status = step.GetStatus().ToLower();

                var stepMarker = "";
                if (step.IsPassed())
                    stepMarker = "&check;";
                else if (step.IsFailed() || step.IsIncomplete())
                    stepMarker = "&cross;";
                else
                {
                    stepMarker = "&cross;";
                }

                var keyword = step.Keyword;
                if (keyword == previousKeyword)
                    keyword = "And";

                listOfLines.Add("<li>");

                if (step.IsSkipped())
                {
                    listOfLines.Add($"<span class='step-marker color-skipped'>{stepMarker}</span>");
                    listOfLines.Add($"<span class='step-keyword'>" + keyword + "</span>");
                    listOfLines.Add($"<span>" + step.Name + "</span>");
                }
                else
                {
                    listOfLines.Add($"<span class='step-marker color-{status}'>{stepMarker}</span>");
                    listOfLines.Add($"<span class='step-keyword'> " + keyword + "</span> ");
                    listOfLines.Add($"<span>" + step.Name + "</span>");
                }

                if (step.DataTable.Rows.Count > 0)
                {
                    listOfLines.Add("<!-- Scenario Steps Data Table Section -->");
                    listOfLines.Add($"<div class='steps-datatable'>");
                    listOfLines.AddRange(GenerateDataTable(step.DataTable));
                    listOfLines.Add("</div>");
                }

                listOfLines.Add("</li>");

                string message = step.Message;
                if (!string.IsNullOrWhiteSpace(message))
                {
                    listOfLines.Add("<!-- Data Step Message -->");
                    listOfLines.Add("<li>");
                    listOfLines.Add($"<div class='message-box'>");
                    listOfLines.Add($"<div class='message-{status}'>{message.Trim().Replace("\n", "<br>")}</div>");
                    listOfLines.Add("</div>");
                    listOfLines.Add("</li>");
                }

                previousKeyword = step.Keyword;
            }

            listOfLines.Add("</ul>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioExamples(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.HasDataTable())
            {
                listOfLines.Add("<!-- Data Scenario Examples -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<span class='examples-keyword'>Examples:</span>");
                listOfLines.Add("<div class='examples-datatable'>");
                listOfLines.AddRange(GenerateDataTable(example.DataTable));
                listOfLines.Add("</div>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataTable(LivingDocDataTable dataTable)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<table class='datatable'>");
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
                listOfLines.Add($"<span class='attachments-keyword'>Attachments:</span>");
                listOfLines.Add("<ul class='attachments-files'>");

                foreach (var attachment in example.Attachments)
                {
                    var filePath = attachment;
                    if (!Uri.IsWellFormedUriString(attachment, UriKind.Absolute))
                        filePath = Path.GetFileName(attachment);

                    listOfLines.Add($"<li><a target='_blank' href='{attachment}'>{filePath}</a></li>");
                }

                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }
    }
}
