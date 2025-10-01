using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataEditorGenerator
    {
        private LivingDocProject project;

        internal LivingDocDataEditorGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> Generate()
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateDataEditor());

            return listOfLines;
        }

        internal List<string> GenerateDataEditor()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Editor -->");
            listOfLines.Add($"<div class='data-item' id='editor-view'>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='page-name'>Gherkin Script Editor</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<hr>");

            listOfLines.Add("<!-- Editor Actions Section -->");
            listOfLines.Add("<div class='section layout-row'>");

            listOfLines.Add("<div class='layout-column align-left'>");
            listOfLines.Add("<span class='chart-name'>Scenario</span>");
            listOfLines.Add("</div>");

            /*
            listOfLines.Add("<div class='layout-column align-right'>");
            listOfLines.Add("<button title='Preview Scenario Script' class='editor-option' onclick='editorPreview()'>Preview</button>");
            listOfLines.Add("<button title='Copy Scenario Script' class='editor-option' onclick='editorCopy()'>Copy</button>");
            listOfLines.Add("<button title='Format Scenario Script' class='editor-option' onclick='editorFormat()'>Format</button>");
            listOfLines.Add("<button title='Clear Scenario Script' class='editor-option' onclick=\"loadEditor(); filterStepDefinitions();\">Clear</button>");
            listOfLines.Add("<button title='Download Scenario Script' class='editor-option' onclick='editorDownload()'>Download</button>");
            listOfLines.Add("</div>");
            */

            listOfLines.Add("</div>");

            listOfLines.Add("<div id='script-view'>");
            listOfLines.Add("<textarea class='filter' id='scenario-script'></textarea>");
            listOfLines.Add("</div>");

            listOfLines.Add("<hr>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='chart-name'>Step Definitions</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<input type='text' class='filter' onKeydown=\"Javascript: if (event.keyCode == 13) loadStepDefinitionByEnter();\" onkeyup='filterStepDefinitions()' id='stepdefinition-filter' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table id='steps-grid' class='grid-view'>");
            listOfLines.Add("<tbody id='steps-table-list'>");

            var mapOfSteps = new Dictionary<string, string>();

            foreach (var feature in project.Features)
            {
                foreach (var scenario in feature.Scenarios)
                {
                    foreach (var example in scenario.Examples)
                    {
                        var previousKeyword = "Given";
                        foreach (var step in example.Steps)
                        {
                            var keywordType = step.Keyword;
                            if (step.Keyword == "And")
                                keywordType = previousKeyword;

                            var fullName = keywordType + " " + step.Name;
                            if (!mapOfSteps.ContainsKey(fullName))
                            {
                                listOfLines.Add($"<tr class='gridline' onclick=\"loadStepDefinition(this);\">");
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
