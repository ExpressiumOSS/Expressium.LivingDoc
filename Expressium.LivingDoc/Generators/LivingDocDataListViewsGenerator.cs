using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;

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
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumnByAttibute(0, \"data-status\")'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Feature<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(2, \"data-scenarios\")'>Scenarios<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(3, \"data-passrate\")'>Pass Rate<span class='sort-column'>&udarr;</span></th>");

            if (project.HasHealth())
            {
                listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(4, \"data-flakyrate\")'>Flaky Rate<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(5, \"data-coverage\")'>Coverage<span class='sort-column'>&udarr;</span></th>");
                //listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(6, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th width='100' onClick='sortTableByColumn(6)'>Status<span class='sort-column'>&udarr;</span></th>");
            }
            else
            {
                listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(4, \"data-coverage\")'>Coverage<span class='sort-column'>&udarr;</span></th>");
                //listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(5, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th width='100' onClick='sortTableByColumn(5)'>Status<span class='sort-column'>&udarr;</span></th>");
            }

            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                var status = feature.GetStatus().ToLower();
                var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(status);

                listOfLines.Add($"<tr class='grid-border' data-role='feature' data-tags='{feature.GetDataStatus()} {feature.Name} {feature.GetDataTags()}' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");
                listOfLines.Add($"<td class='align-center' data-status='{feature.GetStatusSortId()}'><span class='{symbol} color-{status} status-symbol'></span></td>");
                listOfLines.Add($"<td><a href='#'>{feature.Name}</a></td>");
                listOfLines.Add($"<td class='align-center' data-scenarios='{feature.GetNumberOfScenariosSortId()}'>{feature.GetNumberOfScenarios()}</td>");
                listOfLines.Add($"<td class='align-center' data-passrate='{feature.GetPassRateSortId()}'>{feature.GetPassRate()}%</td>");

                if (project.HasHealth())
                    listOfLines.Add($"<td class='align-center' data-flakyrate='{feature.GetFlakyRateSortId()}'>{feature.GetFlakyRate()}%</td>");

                listOfLines.Add($"<td class='align-center' data-coverage='{feature.GetCoverageSortId()}'>{feature.GetCoverage()}%</td>");
                //listOfLines.Add($"<td class='align-center' data-duration='{feature.GetDurationSortId()}'>{feature.GetDuration()}</td>");

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
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumnByAttibute(0, \"data-status\")'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Scenario<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='90' onClick='sortTableByColumnByAttibute(2, \"data-order\")'>Order<span class='sort-column'>&udarr;</span></th>");

            var hasHealth = project.HasHealth();
            if (hasHealth)
            {
                listOfLines.Add("<th width='100' onClick='sortTableByColumnByAttibute(3, \"data-health\")'>Health<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(4, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th width='100' onClick='sortTableByColumn(5)'>Status<span class='sort-column'>&udarr;</span></th>");
            }
            else
            {
                listOfLines.Add("<th width='110' onClick='sortTableByColumnByAttibute(3, \"data-duration\")'>Duration<span class='sort-column'>&udarr;</span></th>");
                listOfLines.Add("<th width='100' onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            }

            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    var status = scenario.GetStatus().ToLower();
                    var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(status);

                    listOfLines.Add($"<tr class='grid-border' data-role='scenario' data-tags='{scenario.GetDataStatus()} {feature.Name} {feature.GetDataTags()} {scenario.GetDataTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                    listOfLines.Add($"<td class='align-center' data-status='{scenario.GetStatusSortId()}'><span class='{symbol} color-{status} status-symbol'></span></td>");
                    listOfLines.Add($"<td><a href='#'>{scenario.Name}</a></td>");
                    listOfLines.Add($"<td class='align-center' data-order='{scenario.GetOrderSortId()}'>{scenario.GetOrder()}</td>");

                    if (hasHealth)
                        listOfLines.Add($"<td data-health='{scenario.GetHealthSortId()}'>{scenario.Health}</td>");

                    listOfLines.Add($"<td class='align-center' data-duration='{scenario.GetDurationSortId()}'>{scenario.GetDuration()}</td>");
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
            listOfLines.Add("<th width='20' class='align-center' onClick='sortTableByColumnByAttibute(0, \"data-status\")'></th>");
            listOfLines.Add("<th onClick='sortTableByColumn(1)'>Step<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='90' onClick='sortTableByColumnByAttibute(2, \"data-used\")'>Used<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='90' onClick='sortTableByColumnByAttibute(3, \"data-failure\")'>Failure<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("<th width='100' onClick='sortTableByColumn(4)'>Status<span class='sort-column'>&udarr;</span></th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");

            listOfLines.Add("<tbody id='table-list'>");

            var mapOfUsedSteps = new Dictionary<string, int>();
            var mapOfFailedSteps = new Dictionary<string, int>();
            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        foreach (var step in example.Steps)
                        {
                            var name = step.Keyword + " " + step.Name;

                            mapOfUsedSteps.TryGetValue(name, out var usedCount);
                            mapOfUsedSteps[name] = usedCount + 1;

                            if (step.IsFailed())
                            {
                                mapOfFailedSteps.TryGetValue(name, out var failedCount);
                                mapOfFailedSteps[name] = failedCount + 1;
                            }
                        }
                    }
                }
            }

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
                                    var used = mapOfUsedSteps[fullName];
                                    mapOfFailedSteps.TryGetValue(fullName, out var failed);

                                    var failure = 0;
                                    if (used != 0)
                                        failure = (int)Math.Round(failed * 100.0 / used, MidpointRounding.AwayFromZero);

                                    var stepStatus = step.GetStatus().ToLower();
                                    var symbol = LivingDocDataUtilitiesGenerator.GetStatusSymbol(stepStatus);

                                    listOfLines.Add($"<tr class='grid-border' data-role='step' data-tags='{step.GetDataStatus()} {feature.Name} {feature.GetDataTags()} {scenario.GetDataTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                                    listOfLines.Add($"<td class='align-center' data-status='{step.GetStatusSortId()}'><span class='{symbol} color-{stepStatus} status-symbol'></span></td>");

                                    listOfLines.Add($"<td><a href='#'>{fullName}</a></td>");
                                    listOfLines.Add($"<td class='align-center' data-used='{used.ToString("D4")}'>{used}</td>");
                                    listOfLines.Add($"<td class='align-center' data-failure='{failure.ToString("D4")}'>{failure}%</td>");
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
    }
}