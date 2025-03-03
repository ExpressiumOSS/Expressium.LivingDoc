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

            listOfLines.AddRange(GenerateFeatureDataSections(project));
            listOfLines.AddRange(GenerateScenarioDataSections(project));

            return listOfLines;
        }

        internal List<string> GenerateFeatureDataSections(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                listOfLines.Add("<!-- Feature Data Section -->");
                listOfLines.Add($"<div class='data-item' id='{feature.Id}'>");

                listOfLines.Add("<div class='section'>");
                listOfLines.AddRange(GenerateFeatureDataTagSection(feature));
                listOfLines.AddRange(GenerateFeatureDataNameSection(feature));
                listOfLines.AddRange(GenerateFeatureDataDescriptionSection(feature));
                listOfLines.AddRange(GenerateFeatureDataBackgroundSection(feature));
                listOfLines.Add("</div>");

                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateFeatureDataTagSection(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Data Tag Section -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='tag-names'>" + feature.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFeatureDataNameSection(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Data Name Section -->");
            listOfLines.Add("<div>");
            listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<span class='feature-name'>Feature: {feature.Name}</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFeatureDataDescriptionSection(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Feature Data Description Section -->");
            listOfLines.Add("<div class='feature-description'>");
            var listOfDescription = feature.Description.Trim().Split("\n");
            foreach (var line in listOfDescription)
                listOfLines.Add("<span>" + line.Trim() + "</span><br />");
            listOfLines.Add("</div>");
            listOfLines.Add("<p></p>");

            return listOfLines;
        }

        internal List<string> GenerateFeatureDataBackgroundSection(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            if (feature.Background != null && feature.Background.Steps.Count > 0)
            {
                listOfLines.Add("<!-- Feature Data Backgrounds Steps Section -->");
                listOfLines.Add("<div class='feature-background'>");
                listOfLines.Add("<span class='feature-name'>Background:</span><br />");

                listOfLines.AddRange(GenerateScenarioDataStepsSection(feature.Background.Steps, false));

                listOfLines.Add("</div>");
                listOfLines.Add("<p></p>");
            }

            return listOfLines;
        }

        internal List<string> GenerateScenarioDataSections(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add("<!-- Scenario Data Section -->");
                    listOfLines.Add($"<div class='data-item' id='{scenario.Id}'>");

                    bool exampleSplitter = false;
                    foreach (var example in scenario.Examples)
                    {
                        if (exampleSplitter)
                            listOfLines.Add("<hr>");
                        exampleSplitter = true;

                        listOfLines.Add("<!-- Scenario Outline Section -->");
                        listOfLines.Add("<div class='section'>");

                        listOfLines.AddRange(GenerateScenarioDataTagSection(scenario));

                        listOfLines.Add("<table class='scenario-outline'>");
                        listOfLines.Add("<tbody>");
                        listOfLines.AddRange(GenerateScenarioDataTitleSection(scenario, example));
                        listOfLines.AddRange(GenerateScenarioDataStepsSection(example.Steps, true));
                        listOfLines.AddRange(GenerateScenarioDataExamplesSection(example));
                        listOfLines.AddRange(GenerateScenarioDataMessageSection(example));
                        listOfLines.Add("</tbody>");
                        listOfLines.Add("</table>");
                        listOfLines.Add("</div>");

                        listOfLines.AddRange(GenerateScenarioDataAttachments(example));
                    }

                    listOfLines.Add("</div>");
                }
            }

            return listOfLines;
        }

        internal List<string> GenerateScenarioDataTagSection(LivingDocScenario scenario)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Data Tag Section -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='tag-names'>" + scenario.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateScenarioDataTitleSection(LivingDocScenario scenario, LivingDocExample example)
        {
            var scenarioKeyword = "Scenario:";
            if (example.TableHeader.Cells.Count > 0)
                scenarioKeyword = "Scenario Outline:";

            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Data Title Section -->");
            listOfLines.Add("<tr>");
            listOfLines.Add("<td colspan='2'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{example.GetStatus().ToLower()}'></span>");
            listOfLines.Add("<span class='scenario-name'>" + scenarioKeyword + " " + scenario.Name + "</span>");
            listOfLines.Add("<span class='duration'>&nbsp;" + example.GetDuration() + "</span>");
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            return listOfLines;
        }

        internal List<string> GenerateScenarioDataStepsSection(List<LivingDocStep> steps, bool isExecuted)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Data Steps Section -->");

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

                if (isExecuted)
                {
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td colspan='2'>");
                    listOfLines.Add($"<span style='margin-right: 1px;' class='step-indent color-{status}'><b>{stepMarker}</b></span>");
                    listOfLines.Add($"<span class='step-keyword'> " + step.Keyword + "</span> ");
                    listOfLines.Add($"<span>" + step.Name + "</span>");
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }
                else
                {
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td colspan='2'>");
                    listOfLines.Add($"<span class='step-indent color-skipped'></span>");
                    listOfLines.Add($"<span class='step-keyword color-skipped'><i> " + step.Keyword + "</i></span> ");
                    listOfLines.Add($"<span class='color-skipped'><i>" + step.Name + "</i></span>");
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }

                if (step.DataTable.Rows.Count > 0)
                {
                    listOfLines.Add("<!-- Scenario Steps Table Section -->");
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td colspan='2' class='examples' style='padding-left: 64px;'>");

                    listOfLines.Add("<table>");
                    listOfLines.Add("<tbody>");

                    int rowNumber = 1;
                    foreach (var row in step.DataTable.Rows)
                    {
                        var numberOfCells = row.Cells.Count;
                        int i = 1;
                        listOfLines.Add($"<tr>");

                        foreach (var cell in row.Cells)
                        {
                            listOfLines.Add($"<td>|</td>");

                            if (rowNumber == 1)
                                listOfLines.Add($"<td><i>" + cell.Value + "</i></td>");
                            else
                                listOfLines.Add($"<td>" + cell.Value + "</td>");

                            if (i == numberOfCells)
                                listOfLines.Add($"<td>|</td>");

                            i++;
                        }
                        listOfLines.Add($"</tr>");

                        rowNumber++;
                    }

                    listOfLines.Add("</tbody>");
                    listOfLines.Add("</table>");

                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }
            }

            return listOfLines;
        }

        internal List<string> GenerateScenarioDataExamplesSection(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.TableHeader.Cells.Count > 0)
            {
                listOfLines.Add("<!-- Scenario Examples Section -->");
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='2' class='examples'><b>Examples:</b></td>");
                listOfLines.Add($"</tr>");

                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='2' class='examples'>");

                listOfLines.Add("<table>");
                listOfLines.Add("<tbody>");

                var numberOfCells = example.TableHeader.Cells.Count;
                int i = 1;
                listOfLines.Add($"<tr>");
                foreach (var cell in example.TableHeader.Cells)
                {
                    listOfLines.Add($"<td>|</td>");
                    listOfLines.Add($"<td><i>" + cell.Value + "</i></td>");

                    if (i == numberOfCells)
                        listOfLines.Add($"<td>|</td>");

                    i++;
                }
                listOfLines.Add($"</tr>");

                foreach (var body in example.TableBody)
                {
                    numberOfCells = body.Cells.Count;
                    i = 1;
                    listOfLines.Add($"<tr>");

                    foreach (var cell in body.Cells)
                    {
                        listOfLines.Add($"<td>|</td>");
                        listOfLines.Add($"<td>" + cell.Value + "</td>");

                        if (i == numberOfCells)
                            listOfLines.Add($"<td>|</td>");

                        i++;
                    }

                    listOfLines.Add($"</tr>");
                }

                listOfLines.Add("</tbody>");
                listOfLines.Add("</table>");

                listOfLines.Add($"</td>");
                listOfLines.Add($"</tr>");
            }

            return listOfLines;
        }

        internal List<string> GenerateScenarioDataMessageSection(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            var status = example.GetStatus().ToLower();

            string message = example.GetMessage();
            if (message != null)
            {
                listOfLines.Add("<!-- Scenario Data Message Section -->");
                listOfLines.Add($"<tr><td></td></tr>");
                listOfLines.Add($"<tr>");
                listOfLines.Add($"<td colspan='2' style='width: 0px; min-width: fit-content;'>");
                listOfLines.Add($"<div class='step-{status}'>{message}</div>");
                listOfLines.Add($"</td>");
                listOfLines.Add($"</tr>");
            }

            return listOfLines;
        }

        internal List<string> GenerateScenarioDataAttachments(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            if (example.Attachments.Count > 0)
            {
                listOfLines.Add("<!-- Scenario Data Attachments Section -->");
                listOfLines.Add("<div class='section' style='padding-top: 2px; padding-bottom: 0px;'>");

                listOfLines.Add("<span class='scenario-name'>Attachments:</span>");
                listOfLines.Add("<ul>");

                foreach (var attachment in example.Attachments)
                {
                    var filePath = Path.GetFileName(attachment);
                    listOfLines.Add($"<li><a target='_blank' href='./Attachments/{filePath}'>{filePath}</a></li>");
                }

                listOfLines.Add("</ul>");

                listOfLines.Add("</div>");
            }

            return listOfLines;
        }
    }
}
