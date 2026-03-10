using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataOverviewGenerator
    {
        private int numberOfColumns = 10;

        private LivingDocProject project;

        internal LivingDocDataOverviewGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> GenerateDataOverview()
        {
            var listOfFolders = project.GetFolders();

            var listOfExcludeFolders = new List<string>();

            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Overview -->");
            listOfLines.Add($"<div id='project-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='table-grid' class='tree-view'>");

            listOfLines.Add("<tbody id='table-list'>");

            listOfLines.AddRange(GenerateOverviewHeaderFolder(project.Title));

            foreach (var folder in listOfFolders)
            {
                if (listOfExcludeFolders.Contains(folder))
                    continue;

                listOfLines.AddRange(GenerateOverview(listOfFolders, listOfExcludeFolders, folder, 1));
            }

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateOverview(List<string> listOfFolders, List<string> listOfExcludeFolders, string folder, int indent)
        {
            var listOfLines = new List<string>();

            if (!string.IsNullOrWhiteSpace(folder))
                listOfLines.AddRange(GenerateOverviewFolder(folder, indent));

            foreach (var subFolder in listOfFolders)
            {
                var folderDepth = GetFolderDepth(folder);
                var subFolderDepth = GetFolderDepth(subFolder);
                if (subFolder != null && subFolder.StartsWith(folder + "\\") && folderDepth + 1 == subFolderDepth)
                {
                    listOfLines.AddRange(GenerateOverview(listOfFolders, listOfExcludeFolders, subFolder, indent + 1));
                    listOfExcludeFolders.Add(subFolder);
                }
            }

            foreach (var feature in project.Features)
            {
                var featureFolder = feature.GetFolder();
                if (featureFolder == folder)
                {
                    var featureDepth = GetFolderDepth(featureFolder);

                    listOfLines.AddRange(GenerateOverviewFeature(feature, featureDepth + 1));

                    foreach (var scenario in feature.Scenarios)
                        listOfLines.AddRange(GenerateOverviewScenario(feature, scenario, featureDepth + 2));
                }
            }

            listOfExcludeFolders.Add(folder);

            return listOfLines;
        }

        internal List<string> GenerateOverviewHeaderFolder(string folder)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<tr>");
            listOfLines.Add($"<td class='grid-folder'><span>&#x1F4C2;</span></td>");
            listOfLines.Add($"<td class='grid-border' colspan='{numberOfColumns - 1}'>");
            listOfLines.Add($"<span class='grid-folder-name'>{GetFolderName(folder)}</span>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='grid-border' style='padding-right: 8px;' colspan='2' align='right'>");

            if (project.ExperimentFlag)
            {
                listOfLines.Add("<button class='grid-expand bi bi-plus-lg' title='Expand All Features' onclick='loadExpandAll()'></button>");
                listOfLines.Add("<button class='grid-collapse bi bi-dash-lg' title='Collapse All Features' onclick='loadCollapseAll()'></button>");
            }
            else
            {
                listOfLines.Add("<a class='grid-expand bi bi-plus-lg' title='Expand All Features' href='#' onclick='loadExpandAll()'></a>");
                listOfLines.Add("<a class='grid-collapse bi bi-dash-lg' title='Collapse All Features' href='#' onclick='loadCollapseAll()'></a>");
            }

            listOfLines.Add($"</td>");
            listOfLines.Add($"</tr>");

            return listOfLines;
        }

        internal List<string> GenerateOverviewFolder(string folder, int indent)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<tr data-name='{folder}' data-role='folder'>");

            for (var i = 0; i < indent; i++)
                listOfLines.Add($"<td></td>");

            listOfLines.Add($"<td class='grid-folder'><span>&#x1F4C2;</span></td>");
            listOfLines.Add($"<td class='grid-border' colspan='{numberOfColumns - indent}'>");
            listOfLines.Add($"<span class='grid-folder-name'>{GetFolderName(folder)}</span>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='grid-border' style='padding-right: 8px;' align='right' ></td>");

            listOfLines.Add($"</tr>");

            return listOfLines;
        }

        internal List<string> GenerateOverviewFeature(LivingDocFeature feature, int indent)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<tr data-parent='{feature.GetFolder()}' data-name='{feature.Id}' data-role='feature' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");

            for (var i = 0; i < indent; i++)
                listOfLines.Add($"<td></td>");

            listOfLines.Add($"<td data-collapse='false' class='grid-toggle' title='Toggle Feature' onclick=\"loadCollapse(this);\"><span class='bi bi-chevron-down'></span></td>");
            listOfLines.Add($"<td class='grid-border' colspan='{numberOfColumns - indent}'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<a class='grid-heading' href='#'>{feature.Name}</a>");
            listOfLines.Add($"</td>");

            listOfLines.Add($"<td class='grid-border' align='right'></td>");

            listOfLines.Add($"</tr>");

            return listOfLines;
        }

        internal List<string> GenerateOverviewScenario(LivingDocFeature feature, LivingDocScenario scenario, int indent)
        {
            var ruleTags = string.Empty;
            if (!string.IsNullOrEmpty(scenario.RuleId))
            {
                var rule = feature.Rules.Find(r => r.Id == scenario.RuleId);
                ruleTags = rule.GetTags();
            }

            var listOfLines = new List<string>();

            listOfLines.Add($"<tr data-parent='{feature.Id}' data-role='scenario' data-tags='{feature.Name} {scenario.GetStatus()} {feature.GetTags()} {scenario.GetTags()} {ruleTags} {feature.Uri}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");

            for (var i = 0; i < indent; i++)
                listOfLines.Add($"<td></td>");

            listOfLines.Add($"<td class='grid-indent'></td>");
            listOfLines.Add($"<td class='grid-border' colspan='{numberOfColumns - indent}'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<a class='grid-heading' href='#'>{scenario.Name}</a>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='grid-border' align='right'></td>");

            listOfLines.Add($"</tr>");

            return listOfLines;
        }

        internal static string GetFolderName(string folder)
        {
            return folder?.Split("\\").LastOrDefault() ?? string.Empty;
        }

        internal static int GetFolderDepth(string folder)
        {
            var depth = 0;

            if (string.IsNullOrWhiteSpace(folder))
                return 0;

            var tokens = folder.Split('\\');
            if (tokens.Length > depth)
                depth = tokens.Length;

            return depth;
        }
    }
}
