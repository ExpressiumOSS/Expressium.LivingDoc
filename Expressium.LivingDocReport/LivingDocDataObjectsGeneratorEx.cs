using Expressium.LivingDoc;
using System.Collections.Generic;

namespace Expressium.LivingDocReport
{
    internal partial class LivingDocDataObjectsGenerator
    {
        internal List<string> GenerateProjectDataTreeListSections(LivingDocProject project, LivingDocFolder node)
        {
            var listOfLines = new List<string>();

            foreach (var child in node.Children)
            {
                listOfLines.Add($"<ul>");

                listOfLines.Add($"<li style='padding: 3px; color: dimgray; font-weight: bold;' class='gridline'>");
                listOfLines.Add($"<span>📂</span>");
                listOfLines.Add($"<span>{child.Key}</span>");
                listOfLines.Add($"</li>");

                listOfLines.AddRange(GenerateProjectDataTreeListSections(project, child.Value));

                foreach (var feature in project.Features)
                {
                    if (feature.Uri.Contains("/" + child.Key))
                    {
                        listOfLines.Add($"<ul>");

                        listOfLines.Add($"<li style='padding: 3px; color: dimgray;' class='gridline' data-name='{feature.Name}' data-role='feature' data-featureid='{feature.Id}' onclick=\"loadFeature(this);\">");
                        listOfLines.Add($"<span>&#10011;</span>");
                        listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
                        listOfLines.Add($"<span><b>{feature.Name}</b></span>");
                        listOfLines.Add($"</li>");

                        foreach (var scenario in feature.Scenarios)
                        {
                            listOfLines.Add($"<ul>");
                            listOfLines.Add($"<li style='padding: 3px;' class='gridline' data-parent='{feature.Name}' data-role='scenario' data-tags='{feature.Name} {scenario.GetStatus()} {feature.GetTags()} {scenario.GetTags()}' data-featureid='{feature.Id}' data-scenarioid='{scenario.Id}' onclick=\"loadScenario(this);\">");
                            listOfLines.Add($"<span class='status-dot bgcolor-{scenario.GetStatus().ToLower()}'></span>");
                            listOfLines.Add($"<a href='#'>{scenario.Name}</a>");
                            listOfLines.Add($"</li>");
                            listOfLines.Add($"</ul>");
                        }

                        listOfLines.Add($"</ul>");
                    }
                }

                listOfLines.Add($"</ul>");
            }

            return listOfLines;
        }

        internal List<string> GenerateProjectDataTreeListSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data TreeList Section -->");
            listOfLines.Add($"<div class='data-item' id='project-view'>");

            listOfLines.Add("<div class='section'>");

            listOfLines.Add($"<ul style='padding-left: 0px;'>");

            listOfLines.Add($"<li style='padding: 3px; color: dimgray; font-weight: bold;' class='gridline'>");
            listOfLines.Add($"<span>📂</span>");
            listOfLines.Add($"<span>{project.Title}</span>");
            listOfLines.Add($"</li>");

            var node = project.GetListOfFolderNodes();
            listOfLines.AddRange(GenerateProjectDataTreeListSections(project, node));

            listOfLines.Add("</ul>");

            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }
    }
}
