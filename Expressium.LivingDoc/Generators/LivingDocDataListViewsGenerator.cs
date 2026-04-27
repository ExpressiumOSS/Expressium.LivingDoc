using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataListViewsGenerator
    {
        private LivingDocProject project;

        internal LivingDocDataListViewsGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> GenerateDataFeaturesListView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Features View -->");
            listOfLines.Add("<div id='features-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='list-view'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Feature<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(2, \"data-scenarios\")'>Scenarios<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='120' onClick='sortTableByColumnByAttibute(3, \"data-completion\")'>Completion<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(4, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(5)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                listOfLines.Add($"<tr class='grid-border' data-role='feature' data-tags='{feature.Name} {feature.GetTags()}' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");

                var status = feature.GetStatus().ToLower();
                if (project.ExperimentFlagSymbols)
                {
                    var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(status);
                    listOfLines.Add($"<td align='center'><span class='{symbol} color-{status} status-symbol'></span></td>");
                }
                else
                    listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{status}'></span></td>");

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

        internal List<string> GenerateDataScenariosListView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Scenarios View -->");
            listOfLines.Add("<div id='scenarios-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='list-view'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Scenario<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumnByAttibute(2, \"data-order\")'>Order<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(3, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    listOfLines.Add($"<tr class='grid-border' data-role='scenario' data-tags='{scenario.GetStatus()} {feature.Name} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");

                    var status = scenario.GetStatus().ToLower();
                    if (project.ExperimentFlagSymbols)
                    {
                        var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(status);
                        listOfLines.Add($"<td align='center'><span class='{symbol} color-{status} status-symbol'></span></td>");
                    }
                    else
                        listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{status}'></span></td>");

                    listOfLines.Add($"<td><a href='#'>{scenario.Name}</a></td>");
                    listOfLines.Add($"<td align='center' data-order='{scenario.GetOrderSortId()}'>{scenario.GetOrder()}</td>");
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

        internal List<string> GenerateDataStepsListView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Steps View -->");
            listOfLines.Add("<div id='steps-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='list-view'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Step<span class='sort-column'>&udarr;</span></th>");
            //listOfLines.Add("<th width='100' onClick='sortTableByColumnByAttibute(2, \"failure-count\")'>Failures<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumnByAttibute(2, \"data-count\")'>Usage<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(3)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            var mapOfSteps = new Dictionary<string, string>();

            var listOfStatuses = new List<string>
            {
                LivingDocStatuses.Failed.ToString(),
                LivingDocStatuses.Incomplete.ToString(),
                LivingDocStatuses.Skipped.ToString(),
                LivingDocStatuses.Passed.ToString(),
            };

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
                                if (status != step.GetStatus())
                                    continue;

                                var fullName = step.Keyword + " " + step.Name;
                                if (!mapOfSteps.ContainsKey(fullName))
                                {
                                    var count = project.Features
                                        .SelectMany(feature => feature.Scenarios)
                                        .SelectMany(scenario => scenario.Examples)
                                        .SelectMany(example => example.Steps)
                                        .Count(x => x.Keyword + " " + x.Name == fullName);

                                    //var failures = project.Features
                                    //    .SelectMany(feature => feature.Scenarios)
                                    //    .SelectMany(scenario => scenario.Examples)
                                    //    .SelectMany(example => example.Steps)
                                    //    .Count(x => x.Keyword + " " + x.Name == fullName && x.IsFailed());

                                    listOfLines.Add($"<tr class='grid-border' data-role='step' data-tags='{step.GetStatus()} {feature.Name} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");

                                    var stepStatus = step.GetStatus().ToLower();
                                    if (project.ExperimentFlagSymbols)
                                    {
                                        var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(stepStatus);
                                        listOfLines.Add($"<td align='center'><span class='{symbol} color-{stepStatus} status-symbol'></span></td>");
                                    }
                                    else
                                        listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{stepStatus}'></span></td>");

                                    listOfLines.Add($"<td><a href='#'>{fullName}</a></td>");
                                    //listOfLines.Add($"<td align='center' failure-count='{failures.ToString("D4")}'>{failures}</td>");
                                    listOfLines.Add($"<td align='center' data-count='{count.ToString("D4")}'>{count}</td>");
                                    listOfLines.Add($"<td>{step.GetStatus()}</td>");
                                    listOfLines.Add($"</tr>");

                                    mapOfSteps.Add(fullName, fullName);
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


        /*
        internal List<string> GenerateDataDefectsListView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Defects View -->");
            listOfLines.Add("<div id='defects-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='list-view'>");

            listOfLines.Add("<thead>");
            listOfLines.Add("<tr data-role='header'>");
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumn(0)'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Message<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='200' onClick='sortTableByColumn(2)'>Type<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumnByAttibute(3, \"data-count\")'>Count<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            var mapOfDefects = new Dictionary<string, string>();

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        foreach (var step in example.Steps)
                        {
                            if (!step.IsFailed())
                                continue;

                            if (string.IsNullOrWhiteSpace(step.ExceptionType))
                                continue;

                            var defectKey = step.ExceptionMessage + "|" + step.ExceptionType;

                            //if (!mapOfDefects.ContainsKey(defectKey))
                            {
                                var count = project.Features
                                    .SelectMany(f => f.Scenarios)
                                    .SelectMany(s => s.Examples)
                                    .SelectMany(e => e.Steps)
                                    .Count(x => x.ExceptionMessage == step.ExceptionMessage && x.ExceptionType == step.ExceptionType);

                                listOfLines.Add($"<tr class='grid-border' data-role='step' data-tags='{step.GetStatus()} {feature.Name} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                                listOfLines.Add($"<td align='center'><span class='status-dot bgcolor-{step.GetStatus().ToLower()}'></span></td>");
                                listOfLines.Add($"<td><a href='#'>{step.ExceptionMessage}</a></td>");
                                listOfLines.Add($"<td>{step.ExceptionType}</td>");
                                listOfLines.Add($"<td align='center' data-count='{count.ToString("D4")}'>{count}</td>");
                                listOfLines.Add($"<td>{step.GetStatus()}</td>");
                                listOfLines.Add($"</tr>");

                                //mapOfDefects.Add(defectKey, defectKey);
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
        */
    }
}