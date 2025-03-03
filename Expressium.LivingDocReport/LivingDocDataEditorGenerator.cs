using Expressium.LivingDoc;
using System.Collections.Generic;

namespace Expressium.LivingDocReport
{
    internal partial class LivingDocDataEditorGenerator
    {
        internal List<string> Generate(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateProjectDataEditorSection(project));

            return listOfLines;
        }

        internal List<string> GenerateProjectDataEditorSection(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Data Editor Section -->");
            listOfLines.Add($"<div class='data-item' id='editor'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='project-name'>Gherkin Script Editor</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='section'>");
            //listOfLines.Add("<button onclick='copyToClipboard();'>Copy</button>");
            // Clear
            // Download
            // View
            //listOfLines.Add("<br />");

            listOfLines.Add("<span class='scenario-name'>Scenario:</span>");
            listOfLines.Add("<br />");
            listOfLines.Add("<textarea class='filter' id='script' rows='7'></textarea>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='scenario-name'>Step Definitions:</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<input type='text' class='filter' onKeydown=\"Javascript: if (event.keyCode == 13) loadStepDefinitionByEnter();\" onkeyup='filterStepDefinitions()' id='stepdefinition-filter' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='steps-grid' class='grid'>");
            listOfLines.Add("<tbody id='steps-table-list'>");

            var mapOfSteps = new Dictionary<string, string>();

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        foreach (var step in example.Steps)
                        {
                            var fullName = step.Keyword + " " + step.Name;
                            if (!mapOfSteps.ContainsKey(fullName))
                            {
                                listOfLines.Add($"<tr class='gridlines' onclick=\"loadStepDefinition(this);\">");
                                listOfLines.Add($"<td><a href='#'>{fullName}</a></td>");
                                listOfLines.Add($"</tr>");

                                mapOfSteps.Add(fullName, step.GetStatus());
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
