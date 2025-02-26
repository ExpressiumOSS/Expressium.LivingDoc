using Expressium.TestExecution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc
{
    internal partial class LivingDocDataGenerator
    {
        internal List<string> GenerateData(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateProjectDataProjectViewSection(project));
            listOfLines.AddRange(GenerateProjectDataFeaturesViewSection(project));
            listOfLines.AddRange(GenerateProjectDataScenariosViewSection(project));
            listOfLines.AddRange(GenerateProjectDataStepsViewSection(project));
            listOfLines.AddRange(GenerateFeatureDataSections(project));
            listOfLines.AddRange(GenerateScenarioDataSections(project));
            listOfLines.AddRange(GenerateProjectDataAnalyticsSection(project));

            return listOfLines;
        }

        internal List<string> GenerateProjectDataProjectViewSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Project View Section -->");
            listOfLines.Add($"<div class='data-item' id='project-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<tbody id='table-list'>");

            //listOfLines.Add($"<tr data-role='folder' style='color: dimgray;'>");
            //listOfLines.Add($"<td width='8px'>📂</td>");
            //listOfLines.Add($"<td colspan='2' class='gridlines' style='font-weight: bold;'>");
            //listOfLines.Add($"<span>{project.Title}</span>");
            //listOfLines.Add($"</td>");
            //listOfLines.Add($"<td class='gridlines' align='right'></td>");
            //listOfLines.Add($"</tr>");

            foreach (var feature in project.Features)
            {
                listOfLines.Add($"<tr data-name='{feature.Name}' data-role='feature' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\" style='color: dimgray;'>");
                listOfLines.Add($"<td width='8px'>&#10011;</td>");
                listOfLines.Add($"<td colspan='2' class='gridlines' style='font-weight: bold;'>");
                listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
                listOfLines.Add($"<span>{feature.Name}</span>");
                listOfLines.Add($"</td>");
                listOfLines.Add($"<td class='gridlines' align='right'></td>");
                listOfLines.Add($"</tr>");

                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add($"<tr data-parent='{feature.Name}' data-role='scenario' data-tags='{feature.Name} {scenario.GetStatus()} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                    listOfLines.Add($"<td width='8px'></td>");
                    listOfLines.Add($"<td width='16px'></td>");
                    listOfLines.Add($"<td class='gridlines'>");
                    listOfLines.Add($"<span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span>");
                    listOfLines.Add($"<a href='#'>{scenario.Name}</a>");
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"<td class='gridlines'></td>");
                    listOfLines.Add($"</tr>");
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateProjectDataFeaturesViewSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Features View Section -->");
            listOfLines.Add($"<div class='data-item' id='features-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20px;' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Feature<span class='sort-column'>&udarr;</span></th>");
            //listOfLines.Add("<th onClick='sortTableByColumnByAttibute(2, \"data-scenarios\")'>Scenarios<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumnByAttibute(2, \"data-coverage\")'>Coverage<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumnByAttibute(3, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                listOfLines.Add($"<tr class='gridlines' data-tags='{feature.Name} {feature.GetTags()}' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");
                listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span></td>");
                listOfLines.Add($"<td><a href='#'>{feature.Name}</a></td>");
                //listOfLines.Add($"<td align='center' data-scenarios='{feature.GetNumberOfScenariosSortId()}'>{feature.GetNumberOfScenarios()}</td>");
                listOfLines.Add($"<td align='center' data-coverage='{feature.GetPercentageOfPassedSortId()}'>{feature.GetPercentageOfPassed()}%</td>");
                listOfLines.Add($"<td align='center' data-duration='{feature.GetDurationSortId()}'>{feature.GetDuration()}</td>");
                listOfLines.Add($"<td>{feature.GetStatus()}</td>");
                listOfLines.Add($"</tr>");
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateProjectDataScenariosViewSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Scenarios View Section -->");
            listOfLines.Add($"<div class='data-item' id='scenarios-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20px;' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Scenario<span class='sort-column'>&udarr;</span></th>");
            //listOfLines.Add("<th onClick='sortTableByColumnByAttibute(2, \"data-sequence\")'>Sequence<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumnByAttibute(2, \"data-coverage\")'>Coverage<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumnByAttibute(3, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add($"<tr class='gridlines' data-tags='{scenario.GetStatus()} {feature.Name} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                    listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span></td>");
                    listOfLines.Add($"<td><a href='#'>{scenario.Name}</a></td>");
                    //listOfLines.Add($"<td align='center' data-sequence='{scenario.GetIndexSortId()}'>{scenario.Index}</td>");
                    listOfLines.Add($"<td align='center' data-coverage='{scenario.GetPercentageOfPassedSortId()}'>{scenario.GetPercentageOfPassed()}%</td>");
                    listOfLines.Add($"<td align='center' data-duration='{scenario.GetDurationSortId()}'>{scenario.GetDuration()}</td>");
                    listOfLines.Add($"<td>{scenario.GetStatus()}</td>");
                    listOfLines.Add($"</tr>");
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateProjectDataStepsViewSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Steps View Section -->");
            listOfLines.Add($"<div class='data-item' id='steps-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20px;' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Step<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumnByAttibute(2, \"data-count\")'>Count<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(3)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            var mapOfSteps = new Dictionary<string, string>();

            var listOfStatuses = new List<string> { "Failed", "Incomplete", "Skipped", "Passed", "All" };
            foreach (var status in listOfStatuses)
            {
                foreach (var feature in project.Features)
                {
                    foreach (var scenario in feature.Scenarios)
                    {
                        foreach (var example in scenario.Examples)
                        {
                            foreach (var step in example.Steps)
                            {
                                if (status != "All" && status != step.GetStatus())
                                    continue;

                                var fullName = step.Keyword + " " + step.Name;
                                if (!mapOfSteps.ContainsKey(fullName))
                                {
                                    var count = project.Features
                                        .SelectMany(feature => feature.Scenarios)
                                        .SelectMany(scenario => scenario.Examples)
                                        .SelectMany(example => example.Steps)
                                        .Where(x => x.Keyword + " " + x.Name == fullName)
                                        .Count();

                                    listOfLines.Add($"<tr class='gridlines' data-tags='{step.GetStatus()} {feature.Name} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                                    listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{step.GetStatus().ToLower()}'></span></td>");
                                    listOfLines.Add($"<td><a href='#'>{fullName}</a></td>");
                                    listOfLines.Add($"<td align='center' data-count='{count.ToString("D4")}'>{count}</td>");
                                    listOfLines.Add($"<td>{step.GetStatus()}</td>");
                                    listOfLines.Add($"</tr>");

                                    mapOfSteps.Add(fullName, step.GetStatus());
                                }
                            }
                        }
                    }
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

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

            if (feature.Backgrounds != null && feature.Backgrounds.Count > 0)
            {
                listOfLines.Add("<!-- Feature Data Backgrounds Steps Section -->");
                listOfLines.Add("<div class='feature-background'>");
                listOfLines.Add("<span class='feature-name'>Background:</span><br />");
                foreach (var background in feature.Backgrounds)
                {
                    foreach (var step in background.Steps)
                    {
                        var status = step.GetStatus().ToLower();

                        var stepMarker = "";
                        if (step.IsPassed())
                            stepMarker = "&check;";
                        //else if (step.IsIncomplete())
                        //stepMarker = "&minus;";
                        else
                            stepMarker = "&cross;";

                        //if (step.IsPassed() || step.IsFailed())
                        {
                            listOfLines.Add($"<tr>");
                            listOfLines.Add($"<td colspan='2'>");
                            listOfLines.Add($"<span style='margin-right: 1px;' class='step-indent color-{status}'><b>{stepMarker}</b></span>");
                            listOfLines.Add($"<span class='step-keyword'> " + step.Keyword + "</span> ");
                            listOfLines.Add($"<span>" + step.Name + "</span>");
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

                            foreach (var row in step.DataTable.Rows)
                            {
                                listOfLines.Add($"<tr>");
                                foreach (var cell in row.Cells)
                                    listOfLines.Add($"<td><i>| " + cell.Value + "</i></td>");
                                listOfLines.Add($"<td>|</td>");
                                listOfLines.Add($"</tr>");
                            }

                            listOfLines.Add("</tbody>");
                            listOfLines.Add("</table>");

                            listOfLines.Add($"</td>");
                            listOfLines.Add($"</tr>");
                        }
                    }
                }

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
                        listOfLines.AddRange(GenerateScenarioDataStepsSection(example));
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

        internal List<string> GenerateScenarioDataStepsSection(LivingDocExample example)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Scenario Data Steps Section -->");

            foreach (var step in example.Steps)
            {
                var status = step.GetStatus().ToLower();

                var stepMarker = "";
                if (step.IsPassed())
                    stepMarker = "&check;";
                //else if (step.IsIncomplete())
                //stepMarker = "&minus;";
                else
                    stepMarker = "&cross;";

                //if (step.IsPassed() || step.IsFailed())
                {
                    listOfLines.Add($"<tr>");
                    listOfLines.Add($"<td colspan='2'>");
                    listOfLines.Add($"<span style='margin-right: 1px;' class='step-indent color-{status}'><b>{stepMarker}</b></span>");
                    listOfLines.Add($"<span class='step-keyword'> " + step.Keyword + "</span> ");
                    listOfLines.Add($"<span>" + step.Name + "</span>");
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"</tr>");
                }
                //else
                //{
                //    listOfLines.Add($"<tr>");
                //    listOfLines.Add($"<td colspan='2'>");
                //    listOfLines.Add($"<span class='step-indent color-skipped'><b>{stepMarker}</b></span>");
                //    listOfLines.Add($"<span class='step-keyword color-skipped'><i> " + step.Type + "</i></span> ");
                //    listOfLines.Add($"<span class='color-skipped'><i>" + step.Text + "</i></span>");
                //    listOfLines.Add($"</td>");
                //    listOfLines.Add($"</tr>");
                //}

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

        internal List<string> GenerateProjectDataEditSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Editor Section -->");
            listOfLines.Add($"<div class='data-item' id='editor'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='project-name'>Gherkin Editor</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<table>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<td>");
            listOfLines.Add("<label for='ftags'>Tags:</label>");
            listOfLines.Add("</td>");
            listOfLines.Add("<td>");
            listOfLines.Add("<input type='text' id='ftags' name='ftags'>");
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            listOfLines.Add("<tr>");
            listOfLines.Add("<td>");
            listOfLines.Add("<label for='fscenario'>Scenario:</label>");
            listOfLines.Add("</td>");
            listOfLines.Add("<td>");
            listOfLines.Add("<input type='text' id='fscenario' name='fscenario'>");
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");

            listOfLines.Add("<tr>");
            listOfLines.Add("<td>");
            listOfLines.Add("<label for='fsteps'>Steps:</label>");
            listOfLines.Add("</td>");
            listOfLines.Add("<td>");
            listOfLines.Add("<textarea id='fsteps' name='fsteps' rows='10' cols='50'></textarea>");
            listOfLines.Add("</td>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</table>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateProjectDataAnalyticsSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Analytics Section -->");
            listOfLines.Add($"<div class='data-item' id='analytics'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='project-name'>Analytics</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("</div>");

            listOfLines.AddRange(GenerateProjectDataAnalyticsFeaturesStatusChartSection(project));
            listOfLines.AddRange(GenerateProjectDataAnalyticsScenariosStatusChartSection(project));
            listOfLines.AddRange(GenerateProjectDataAnalyticsDurationSection(project));

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateProjectDataAnalyticsFeaturesStatusChartSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            var numberOfPassed = project.GetNumberOfPassedFeatures();
            var numberOfIncomplete = project.GetNumberOfIncompleteFeatures();
            var numberOfFailed = project.GetNumberOfFailedFeatures();
            var numberOfSkipped = project.GetNumberOfSkippedFeatures();
            var numberOfTests = project.Features.Count;

            listOfLines.AddRange(GenerateProjectDataAnalyticsStatusChartSection("Features", numberOfPassed, numberOfIncomplete, numberOfFailed, numberOfSkipped, numberOfTests));

            return listOfLines;
        }

        internal List<string> GenerateProjectDataAnalyticsScenariosStatusChartSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            var numberOfPassed = project.GetNumberOfPassed();
            var numberOfIncomplete = project.GetNumberOfIncomplete();
            var numberOfFailed = project.GetNumberOfFailed();
            var numberOfSkipped = project.GetNumberOfSkipped();
            var numberOfTests = project.GetNumberOfTests();

            listOfLines.AddRange(GenerateProjectDataAnalyticsStatusChartSection("Scenarios", numberOfPassed, numberOfIncomplete, numberOfFailed, numberOfSkipped, numberOfTests));

            return listOfLines;
        }

        internal List<string> GenerateProjectDataAnalyticsStatusChartSection(string title, int numberOfPassed, int numberOfIncomplete, int numberOfFailed, int numberOfSkipped, int numberOfTests)
        {
            var percentageOfPassed = (int)Math.Round(100.0f / numberOfTests * numberOfPassed);
            var percentageOfIncomplete = (int)Math.Round(100.0f / numberOfTests * numberOfIncomplete);
            var percentageOfFailed = (int)Math.Round(100.0f / numberOfTests * numberOfFailed);
            var percentageOfSkipped = (int)Math.Round(100.0f / numberOfTests * numberOfSkipped);

            var totalPercentage = percentageOfPassed + percentageOfIncomplete + percentageOfFailed + percentageOfSkipped;

            // Adjust the largest category if there's a discrepancy
            int difference = 100 - totalPercentage;
            if (difference != 0)
            {
                var percentages = new List<(int value, Action<int> setter)>
                {
                    (percentageOfPassed, val => percentageOfPassed = val),
                    (percentageOfIncomplete, val => percentageOfIncomplete = val),
                    (percentageOfFailed, val => percentageOfFailed = val),
                    (percentageOfSkipped, val => percentageOfSkipped = val)
                };

                // Find the category with the largest percentage and adjust it
                var maxCategory = percentages.OrderByDescending(p => p.value).First();
                maxCategory.setter(maxCategory.value + difference);
            }

            var listOfLines = new List<string>();

            listOfLines.Add("<div class='section' style='width: fit-content; margin: auto;'>");
            listOfLines.Add($"<span class='project-name' style='padding-left: 8px; color: dimgray;'>{title}</span>");
            listOfLines.Add("<div class='section' style='width: fit-content; margin: auto; padding: 16px; border-radius: 16px; background-color: whitesmoke;'>");

            {
                listOfLines.Add("<!-- Project Data Analytics Status Chart Section -->");
                listOfLines.Add($"<div class='section' style='text-align: center; max-width: 500px; margin: auto;'>");
                listOfLines.Add($"    <svg width='180px' height='180px' viewBox='0 0 42 42'>");
                listOfLines.Add($"        <g transform='rotate(-90, 21, 21)'>");
                listOfLines.Add($"            <circle class='donut-segment-skipped' cx='21' cy='21' r='15.9155'></circle>");
                listOfLines.Add($"            <circle class='donut-segment-passed' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfPassed} {100 - percentageOfPassed}' stroke-dashoffset='0'></circle>");
                listOfLines.Add($"            <circle class='donut-segment-incomplete' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfIncomplete} {100 - percentageOfIncomplete}' stroke-dashoffset='-{percentageOfPassed}'></circle>");
                listOfLines.Add($"            <circle class='donut-segment-failed' cx='21' cy='21' r='15.9155' stroke-dasharray='{percentageOfFailed} {100 - percentageOfFailed}' stroke-dashoffset='-{percentageOfPassed + percentageOfIncomplete}'></circle>");
                listOfLines.Add($"        </g>");
                listOfLines.Add($"        <g class='chart-text'>");
                listOfLines.Add($"            <text x='50%' y='50%' class='chart-number'>{percentageOfPassed}%</text>");
                listOfLines.Add($"            <text x='50%' y='50%' class='chart-label'>Passed</text>");
                listOfLines.Add($"        </g>");
                listOfLines.Add($"    </svg>");
                listOfLines.Add($"</div>");
            }

            {
                listOfLines.Add("<!-- Status Slave Chart Section -->");
                listOfLines.Add("<div class='section' style='text-align: center; max-width: 700px; margin: auto;'>");
                listOfLines.Add("<table align='center'>");
                listOfLines.Add("<tr>");

                listOfLines.Add("<td class='color-passed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfPassed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfPassed}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Passed</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-passed'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-incomplete chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfIncomplete}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfIncomplete}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Incomplete</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-incomplete'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-failed chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfFailed}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfFailed}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Failed</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-failed'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-skipped chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfSkipped}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>{percentageOfSkipped}%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Skipped</span><br />");
                listOfLines.Add($"<div class='chart-count-bar bgcolor-skipped'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("<td class='color-total chart-count'>");
                listOfLines.Add($"<span class='chart-count-number'>{numberOfTests}</span><br />");
                listOfLines.Add($"<span class='chart-count-percentage'>100%</span><br />");
                listOfLines.Add($"<span class='chart-count-status'>Total</span><br />");
                listOfLines.Add($"<div class='chart-count-bar' style='background-color: black;'></div>");
                listOfLines.Add("</td>");

                listOfLines.Add("</tr>");
                listOfLines.Add("</table>");
                listOfLines.Add("</div>");
            }

            listOfLines.Add("</div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateProjectDataAnalyticsDurationSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Analytics Duration Section -->");
            listOfLines.Add("<div style='padding-top: 6px; text-align: center; justify-content: center; align-items: center; display: flex;'>");
            listOfLines.Add($"<span style='font-size: 40px;'>&#9201;</span>");
            listOfLines.Add($"<span style='font-size: 24px; padding-top: 6px;'>");
            listOfLines.Add($"<span style='color: gray;'>Duration </span>");
            listOfLines.Add($"<span>{project.GetDuration()}</span>");
            listOfLines.Add("</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        // Old Status Chart Visualization...
        //    {
        //    listOfLines.Add("<!-- Project Data Analytics Status Chart Section -->");
        //    listOfLines.Add("<div class='section' style='text-align: center; max-width: 500px; margin: auto;'>");
        //    listOfLines.Add($"<span class='chart-percentage'>{percentageOfPassed.ToString("0")}%</span><br />");
        //    listOfLines.Add("<span class='chart-status'>Passed</span><br />");

        //    listOfLines.Add("<div style='padding: 6px;'></div>");

        //    listOfLines.Add($"<div class='chart-bar' style='width: 100%;'>");
        //    listOfLines.Add($"<div class='chart-bar bgcolor-passed' style='width: {percentageOfPassed}%;'></div>");
        //    listOfLines.Add($"<div class='chart-bar bgcolor-incomplete' style='width: {percentageOfIncomplete}%;'></div>");
        //    listOfLines.Add($"<div class='chart-bar bgcolor-failed' style='width: {percentageOfFailed}%;'></div>");
        //    listOfLines.Add($"<div class='chart-bar bgcolor-skipped' style='width: {percentageOfSkipped}%;'></div>");
        //    listOfLines.Add("</div>");

        //    var message = GetStatusMessage((int)percentageOfPassed);
        //    listOfLines.Add($"<span class='chart-message'>{message}</span>");
        //    listOfLines.Add("</div>");
        //}
    }
}
