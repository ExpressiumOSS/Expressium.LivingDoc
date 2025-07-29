using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expressium.LivingDoc.Generators
{
    internal partial class LivingDocDataViewsGenerator
    {
        internal List<string> GenerateDataOverviewWithFolders(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Overview -->");
            listOfLines.Add($"<div class='data-item' id='project-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='tree-view'>");

            listOfLines.Add("<tbody id='table-list'>");

            listOfLines.Add($"<tr class='gridline-header' data-role='folder'>");
            listOfLines.Add($"<td width='8px'>📂</td>");
            listOfLines.Add($"<td colspan='4' class='gridline'>");
            listOfLines.Add($"<span><b>{project.Title}</b></span>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='gridline' align='right'></td>");
            listOfLines.Add($"</tr>");

            var listOfFolders = project.GetFolders();
            foreach (var folder in listOfFolders)
            {
                if (!string.IsNullOrWhiteSpace(folder))
                {
                    if (!string.IsNullOrWhiteSpace(folder))
                    {
                        listOfLines.Add($"<tr class='gridline-header' data-role='folder'>");
                        listOfLines.Add($"<td></td>");
                        listOfLines.Add($"<td width='8px'>📂</td>");
                        listOfLines.Add($"<td colspan='3' class='gridline'>");
                        listOfLines.Add($"<span><b>{folder}</b></span>");
                        listOfLines.Add($"</td>");
                        listOfLines.Add($"<td class='gridline' align='right'></td>");
                        listOfLines.Add($"</tr>");
                    }
                }

                foreach (var feature in project.Features)
                {
                    var featureFolder = feature.GetFolder();
                    if (featureFolder != folder)
                        continue;

                    listOfLines.Add($"<tr class='gridline-header' data-name='{feature.Name}' data-role='feature' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");
                    listOfLines.Add($"<td></td>");

                    if (!string.IsNullOrWhiteSpace(featureFolder))
                    {
                        listOfLines.Add($"<td></td>");
                        listOfLines.Add($"<td data-collapse='false' width='8px' onclick=\"loadCollapse(this);\">&#11206;</td>");
                        listOfLines.Add($"<td colspan='2' class='gridline'>");
                    }
                    else
                    {
                        listOfLines.Add($"<td data-collapse='false' width='8px' onclick=\"loadCollapse(this);\">&#11206;</td>");
                        listOfLines.Add($"<td colspan='3' class='gridline'>");
                    }

                    listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
                    listOfLines.Add($"<span><b>{feature.Name}</b></span>");
                    listOfLines.Add($"</td>");
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

                        listOfLines.Add($"<tr data-parent='{feature.Name}' data-role='scenario' data-tags='{feature.Name} {scenario.GetStatus()} {feature.GetTags()} {scenario.GetTags()} {ruleTags} {feature.Uri}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");

                        listOfLines.Add($"<td></td>");
                        if (!string.IsNullOrWhiteSpace(featureFolder))
                            listOfLines.Add($"<td></td>");

                        listOfLines.Add($"<td></td>");
                        listOfLines.Add($"<td width='16px;'></td>");

                        if (!string.IsNullOrWhiteSpace(featureFolder))
                            listOfLines.Add($"<td class='gridline'>");
                        else
                            listOfLines.Add($"<td class='gridline' colspan='2'>");

                        listOfLines.Add($"<span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span>");
                        listOfLines.Add($"<a href='#'>{scenario.Name}</a>");
                        listOfLines.Add($"</td>");
                        listOfLines.Add($"<td class='gridline' align='right'></td>");
                        listOfLines.Add($"</tr>");
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
