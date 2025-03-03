using Expressium.LivingDoc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDocReport
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

        internal List<string> GenerateDataFeaturesView(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Features View -->");
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

        internal List<string> GenerateDataScenariosView(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenarios View -->");
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

        internal List<string> GenerateDataStepsView(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Steps View -->");
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
    }
}
