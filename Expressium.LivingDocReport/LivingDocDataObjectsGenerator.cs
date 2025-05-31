using Expressium.LivingDoc;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDocReport
{
    internal partial class LivingDocDataObjectsGenerator
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
            listOfLines.Add($"<span class='feature-name'>Feature: {feature.Name}</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureDescription(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Feature Description -->");
            listOfLines.Add("<div class='feature-description'>");

            if (feature.Description != null)
            {
                var listOfDescription = feature.Description.Trim().Split("\n");
                foreach (var line in listOfDescription)
                    listOfLines.Add("<span>" + line.Trim() + "</span><br />");
            }
            listOfLines.Add("</div>");
            listOfLines.Add("<p></p>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureBackground(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            if (feature.Background != null && feature.Background.Steps.Count > 0)
            {
                listOfLines.Add("<!-- Data Feature Background -->");
                listOfLines.Add("<div class='feature-background'>");
                listOfLines.Add("<span class='feature-name'>Background:</span><br />");

                listOfLines.AddRange(GenerateDataScenarioSteps(feature.Background.Steps, false));

                listOfLines.Add("</div>");
                listOfLines.Add("<p></p>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarios(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<!-- Data Scenario -->");
                    listOfLines.Add($"<div class='data-item' id='{scenario.Id}'>");

                    bool exampleSplitter = false;
                    foreach (var example in scenario.Examples)
                    {
                        if (exampleSplitter)
                            listOfLines.Add("<hr>");
                        exampleSplitter = true;

                        listOfLines.Add("<!-- Scenario Outline Section -->");
                        listOfLines.Add("<div class='section'>");

                        listOfLines.AddRange(GenerateDataScenarioTags(scenario));

                        listOfLines.Add("<table class='scenario-outline'>");
                        listOfLines.Add("<tbody>");
                        listOfLines.AddRange(GenerateDataScenarioTitle(scenario, example));
                        listOfLines.AddRange(GenerateDataScenarioSteps(example.Steps, true));
                        listOfLines.AddRange(GenerateDataScenarioExamples(example));
                        listOfLines.AddRange(GenerateDataScenarioMessage(example));
                        listOfLines.Add("</tbody>");
                        listOfLines.Add("</table>");
                        listOfLines.Add("</div>");

                        listOfLines.AddRange(GenerateDataScenarioAttachments(example));
                    }

                    listOfLines.Add("</div>");
                }
            }

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

        internal List<string> GenerateDataScenarioTitle(LivingDocScenario scenario, LivingDocExample example)
        {
            var scenarioKeyword = "Scenario:";
            if (example.HasDataTable())
                scenarioKeyword = "Scenario Outline:";

            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Title -->");
            listOfLines.Add("<tr>");
            listOfLines.Add("<td colspan='2'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{example.GetStatus().ToLower()}'></span>");
            listOfLines.Add("<span class='scenario-name'>" + scenarioKeyword + " " + scenario.Name + "</span>");
            listOfLines.Add("<span class='duration'>&nbsp;" + example.GetDuration() + "</span>");
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioSteps(List<LivingDocStep> steps, bool isExecuted)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenario Steps -->");

            string previousKeyword = null;
            foreach (var step in steps)
            {
                var status = step.GetStatus().ToLower();

                var stepMarker = "";
                if (step.IsPassed())
                    stepMarker = "&check;";
                //else if (step.IsIncomplete())
                //stepMarker = "&minus;";
                else
                    stepMarker = "&cross;";

                var keyword = step.Keyword;
                if (keyword == previousKeyword)
                    keyword = "And";

                if (isExecuted)
                {
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td colspan='2'>");
                    listOfLines.Add($"<span class='step-indent color-{status}'><b>{stepMarker}</b></span>");
                    listOfLines.Add($"<span class='step-keyword'> " + keyword + "</span> ");
                    listOfLines.Add($"<span>" + step.Name + "</span>");
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }
                else
                {
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td colspan='2'>");
                    listOfLines.Add($"<span class='step-indent color-skipped'></span>");
                    listOfLines.Add($"<span class='step-keyword color-skipped'><i> " + keyword + "</i></span> ");
                    listOfLines.Add($"<span class='color-skipped'><i>" + step.Name + "</i></span>");
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }

                if (step.DataTable.Rows.Count > 0)
                {
                    listOfLines.Add("<!-- Scenario Steps Table Section -->");
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td colspan='2' style='padding-left: 64px;'>");
                    listOfLines.AddRange(GenerateDataTable(step.DataTable));
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }

                previousKeyword = step.Keyword;
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioExamples(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.HasDataTable())
            {
                listOfLines.Add("<!-- Data Scenario Examples -->");
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='2' class='step-table-name'>Examples:</td>");
                listOfLines.Add($"</tr>");

                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='2'>");
                listOfLines.AddRange(GenerateDataTable(example.DataTable));
                listOfLines.Add($"</td>");
                listOfLines.Add($"</tr>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataTable(LivingDocDataTable dataTable)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<table class='step-table'>");
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

        internal List<string> GenerateDataScenarioMessage(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            var status = example.GetStatus().ToLower();

            string message = example.GetMessage();
            if (message != null)
            {
                listOfLines.Add("<!-- Data Scenario Message -->");
                listOfLines.Add($"<tr><td></td></tr>");
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td class='step-message' colspan='2'>");
                listOfLines.Add($"<div class='step-{status}'>{message.Trim().Replace("\n", "<br>")}</div>");
                listOfLines.Add($"</td>");
                listOfLines.Add($"</tr>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataScenarioAttachments(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.Attachments.Count > 0)
            {
                listOfLines.Add("<!-- Data Scenario Attachments -->");
                listOfLines.Add("<div class='section attachments'>");

                listOfLines.Add("<span class='scenario-name'>Attachments:</span>");
                listOfLines.Add("<ul>");

                foreach (var attachment in example.Attachments)
                {
                    var filePath = Path.GetFileName(attachment);
                    listOfLines.Add($"<li><a target='_blank' href='{attachment}'>{filePath}</a></li>");
                }

                listOfLines.Add("</ul>");

                listOfLines.Add("</div>");
            }

            return listOfLines;
        }
    }
}
