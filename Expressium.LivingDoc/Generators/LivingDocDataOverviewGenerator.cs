using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataOverviewGenerator
    {
        private int numberOfColumns = 10;
        private bool showFolderStructure = true;

        private LivingDocProject project;

        internal LivingDocDataOverviewGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> Generate()
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateDataOverview());

            return listOfLines;
        }

        internal List<string> GenerateDataOverview()
        {
            // Overview without folder structure...
            if (!showFolderStructure)
            {
                foreach (var feature in project.Features)
                    feature.Uri = string.Empty;
            }

            var listOfFolders = project.GetFolders();

            var listOfExcludeFolders = new List<string>();

            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Overview -->");
            listOfLines.Add($"<div class='data-item' id='project-view'>");

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

            listOfLines.Add($"<tr class='gridline-header'>");
            listOfLines.Add($"<td class='grid-folder' width='16px;'>📂</td>");
            listOfLines.Add($"<td class='gridline' colspan='{numberOfColumns - 1}'>");
            listOfLines.Add($"<span><b>{folder?.Split("\\").LastOrDefault() ?? string.Empty}</b></span>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='gridline' style='padding-right: 8px;' colspan='2' align='right'>");
            listOfLines.Add("<a class='grid-option' title='Expand All Features' href='#' onclick='loadExpandAll()'><b>&plus;</b></a>");
            listOfLines.Add("<a class='grid-option' title='Collapse All Features' href='#' onclick='loadCollapseAll()'><b>&minus;</b></a>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"</tr>");

            return listOfLines;
        }

        internal List<string> GenerateOverviewFolder(string folder, int indent)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<tr class='gridline-header' data-name='{folder}' data-role='folder'>");

            for (var i = 0; i < indent; i++)
                listOfLines.Add($"<td></td>");

            listOfLines.Add($"<td class='grid-folder' width='16px;'>📂</td>");
            listOfLines.Add($"<td class='gridline' colspan='{numberOfColumns - indent}'>");
            listOfLines.Add($"<span><b>{folder?.Split("\\").LastOrDefault() ?? string.Empty}</b></span>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='gridline' align='right'></td>");

            listOfLines.Add($"</tr>");

            return listOfLines;
        }

        internal List<string> GenerateOverviewFeature(LivingDocFeature feature, int indent)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"<tr class='gridline-header' data-parent='{feature.GetFolder()}' data-name='{feature.Id}' data-role='feature' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");

            for (var i = 0; i < indent; i++)
                listOfLines.Add($"<td></td>");

            listOfLines.Add($"<td data-collapse='false' width='16px;' style='text-align: center;' onclick=\"loadCollapse(this);\">&#11206;</td>");
            listOfLines.Add($"<td class='gridline' colspan='{numberOfColumns - indent}'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<span><b>{feature.Name}</b></span>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='gridline' align='right'></td>");
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

            listOfLines.Add($"<td width='16px;'></td>");
            listOfLines.Add($"<td class='gridline' colspan='{numberOfColumns - indent}'>");
            listOfLines.Add($"<span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<a href='#'>{scenario.Name}</a>");
            listOfLines.Add($"</td>");
            listOfLines.Add($"<td class='gridline' align='right'></td>");
            listOfLines.Add($"</tr>");

            return listOfLines;
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
