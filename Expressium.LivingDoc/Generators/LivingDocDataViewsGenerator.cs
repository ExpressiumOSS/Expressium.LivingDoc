using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal partial class LivingDocDataViewsGenerator
    {
        internal List<string> Generate(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateDataOverview(project));
            listOfLines.AddRange(GenerateDataFeaturesView(project));
            listOfLines.AddRange(GenerateDataScenariosView(project));
            listOfLines.AddRange(GenerateDataStepsView(project));

            return listOfLines;
        }

        internal List<string> GenerateDataOverview(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Overview -->");
            listOfLines.Add($"<div class='data-item' id='project-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<tbody id='table-list'>");

            //listOfLines.Add($"<tr class='gridline-header' data-role='folder'>");
            //listOfLines.Add($"<td width='8px'>📂</td>");
            //listOfLines.Add($"<td colspan='2' class='gridline'>");
            //listOfLines.Add($"<span><b>{project.Title}</b></span>");
            //listOfLines.Add($"</td>");
            //listOfLines.Add($"<td class='gridline' align='right'></td>");
            //listOfLines.Add($"</tr>");

            foreach (var feature in project.Features)
            {
                //listOfLines.Add($"<tr class='gridline-header' data-role='folder'>");
                //listOfLines.Add($"<td width='8px'>📂</td>");
                //listOfLines.Add($"<td colspan='2' class='gridline'>");
                //listOfLines.Add($"<span><b>{feature.Uri}</b></span>");
                //listOfLines.Add($"</td>");
                //listOfLines.Add($"<td class='gridline' align='right'></td>");
                //listOfLines.Add($"</tr>");

                listOfLines.Add($"<tr class='gridline-header' data-name='{feature.Name}' data-role='feature' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");
                listOfLines.Add($"<td data-collapse='false' width='8px' onclick=\"loadCollapse(this);\">&#11206;</td>");
                listOfLines.Add($"<td colspan='2' class='gridline'>");
                listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
                listOfLines.Add($"<span><b>{feature.Name}</b></span>");
                listOfLines.Add($"</td>");
                //listOfLines.Add($"<td class='gridline' align='right'><span class='duration'>{feature.GetPercentageOfPassed()}%</span></td>");
                listOfLines.Add($"<td class='gridline' align='right'></td>");
                listOfLines.Add($"</tr>");

                foreach (var scenario in feature.Scenarios)
                {
                    var ruleTags = string.Empty;
                    if (!string.IsNullOrEmpty(scenario.RuleId))
                    {
                        var rule = feature.Rules.Find(r => r.Id == scenario.RuleId);
                        ruleTags = rule.GetTags();
                    }

                    listOfLines.Add($"<tr data-parent='{feature.Name}' data-role='scenario' data-tags='{feature.Name} {scenario.GetStatus()} {feature.GetTags()} {scenario.GetTags()} {ruleTags}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                    listOfLines.Add($"<td width='8px'></td>");
                    listOfLines.Add($"<td width='16px'></td>");
                    listOfLines.Add($"<td class='gridline'>");
                    listOfLines.Add($"<span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span>");
                    listOfLines.Add($"<a href='#'>{scenario.Name}</a>");
                    listOfLines.Add($"</td>");
                    listOfLines.Add($"<td class='gridline'></td>");
                    listOfLines.Add($"</tr>");
                }
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeaturesView(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Features View -->");
            listOfLines.Add($"<div class='data-item' id='features-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Feature<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(2, \"data-scenarios\")'>Scenarios<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='120' onClick='sortTableByColumnByAttibute(2, \"data-completion\")'>Completion<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(3, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                listOfLines.Add($"<tr class='gridline' data-tags='{feature.Name} {feature.GetTags()}' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");
                listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span></td>");
                listOfLines.Add($"<td><a href='#'>{feature.Name}</a></td>");
                listOfLines.Add($"<td align='center' data-scenarios='{feature.GetNumberOfScenariosSortId()}'>{feature.GetNumberOfScenarios()}</td>");
                listOfLines.Add($"<td align='center' data-completion='{feature.GetPercentageOfPassedSortId()}'>{feature.GetPercentageOfPassed()}%</td>");
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

        internal List<string> GenerateDataScenariosView(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenarios View -->");
            listOfLines.Add($"<div class='data-item' id='scenarios-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Scenario<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumnByAttibute(2, \"data-steps\")'>Steps<span class='sort-column'>&udarr;</span></th>");
            //listOfLines.Add("<th width='120' onClick='sortTableByColumnByAttibute(2, \"data-completion\")'>Completion<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(3, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add($"<tr class='gridline' data-tags='{scenario.GetStatus()} {feature.Name} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                    listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span></td>");
                    listOfLines.Add($"<td><a href='#'>{scenario.Name}</a></td>");
                    listOfLines.Add($"<td align='center' data-steps='{scenario.GetNumberOfStepsSortId()}'>{scenario.GetNumberOfSteps()}</td>");
                    //listOfLines.Add($"<td align='center' data-completion='{scenario.GetPercentageOfPassedSortId()}'>{scenario.GetPercentageOfPassed()}%</td>");
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

        internal List<string> GenerateDataStepsView(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Steps View -->");
            listOfLines.Add($"<div class='data-item' id='steps-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='grid'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Step<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumnByAttibute(2, \"data-count\")'>Usage<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(3)'>Status<span class='sort-column'>&udarr;</span></th>");
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

                                    listOfLines.Add($"<tr class='gridline' data-tags='{step.GetStatus()} {feature.Name} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
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
    }
}
