using Expressium.LivingDoc;
using Expressium.LivingDocReport.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDocReport
{
    internal class LivingDocBodyGenerator
    {
        private bool includeEditor = false;

        internal List<string> GenerateBody(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateBodyHeader());
            listOfLines.AddRange(GenerateHeader(project));
            listOfLines.AddRange(GenerateNavigation(project));
            listOfLines.AddRange(GenerateContent(project));
            listOfLines.AddRange(GenerateFooter(project));
            listOfLines.AddRange(GenerateDataViews(project));
            listOfLines.AddRange(GenerateDataObjects(project));
            listOfLines.AddRange(GenerateDataAnalytics(project));
            listOfLines.AddRange(GenerateDataEditor(project));
            listOfLines.AddRange(GenerateBodyFooter());

            return listOfLines;
        }

        internal List<string> GenerateBodyHeader()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<body onload=\"loadAnalytics('analytics'); loadViewmode('project-view');\">");

            return listOfLines;
        }

        internal List<string> GenerateHeader(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Header Section -->");
            listOfLines.Add("<header>");
            listOfLines.Add("<span class='project-name'>" + project.Title + "</span><br />");
            listOfLines.Add("<span class='project-date'>Generated " + project.GetExecutionTime() + "</span>");
            listOfLines.Add("</header>");

            return listOfLines;
        }

        internal List<string> GenerateNavigation(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Navigation Section -->");
            listOfLines.Add("<nav class='navigation'>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Overview' href='#' onclick=\"loadViewmode('project-view');\">Overview</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Features List View' href='#' onclick=\"loadViewmode('features-view');\">Features</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Scenarios List View' href='#' onclick=\"loadViewmode('scenarios-view');\">Scenarios</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Steps List View' href='#' onclick=\"loadViewmode('steps-view');\">Steps</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Analytics' href='#' onclick=\"loadAnalytics('analytics');\">Analytics</a>");
            listOfLines.Add("<span>|</span>");

            if (includeEditor)
            {
                listOfLines.Add("<a class='navigation-link' title='Gherkin Script Editor' href='#' onclick=\"loadEditor('editor'); filterStepDefinitions();\">Editor</a>");
                listOfLines.Add("<span>|</span>");
            }

            listOfLines.Add("</nav>");

            return listOfLines;
        }

        internal List<string> GenerateContent(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Content Wrapper Section -->");
            listOfLines.Add("<div id='content-wrapper'>");

            listOfLines.Add("<!-- Left Content Section -->");
            listOfLines.Add("<div id='left-section' class='bg-light p-3'>");
            listOfLines.AddRange(GenerateScenarioPreFilters(project));
            listOfLines.AddRange(GenerateScenarioFilter(project));
            listOfLines.Add("<div id='scenario-view'></div>");
            listOfLines.Add("</div>");

            listOfLines.Add("<!-- Splitter Content Section -->");
            listOfLines.Add("<div id='splitter'></div>");

            listOfLines.Add("<!-- Right Content Section -->");
            listOfLines.Add("<div id='right-section' class='bg-light p-3'>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            listOfLines.Add("<!-- Content Splitter Script -->");
            listOfLines.AddRange(Resources.Splitter.Split(Environment.NewLine).ToList());

            return listOfLines;
        }

        internal List<string> GenerateScenarioPreFilters(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Features PreFilters Section -->");
            listOfLines.Add("<div class='section layout-row'>");

            listOfLines.Add("<div class='layout-column align-right'>");
            listOfLines.Add("<button title='Preset Filter with Passed' class='color-undefined' onclick='presetFilter(\"passed\")'>Passed</button>");
            listOfLines.Add("<button title='Preset Filter with Incomplete' class='color-undefined' onclick='presetFilter(\"incomplete\")'>Incomplete</button>");
            listOfLines.Add("<button title='Preset Filter with Failed' class='color-undefined' onclick='presetFilter(\"failed\")'>Failed</button>");
            listOfLines.Add("<button title='Preset Filter with Skipped' class='color-undefined' onclick='presetFilter(\"skipped\")'>Skipped</button>");
            listOfLines.Add("<button title='Clear Filter' class='color-undefined' onclick='presetFilter(\"\")'>Clear</button>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateScenarioFilter(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Features Filter Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<input class='filter' onkeyup='filterScenarios()' id='scenario-filter' type='text' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFooter(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Footer Section -->");
            listOfLines.Add("<footer>");
            listOfLines.Add("©2025 Expressium All Rights Reserved");
            listOfLines.Add("</footer>");

            return listOfLines;
        }

        internal List<string> GenerateDataViews(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            var dataGenerator = new LivingDocDataViewsGenerator();
            listOfLines.AddRange(dataGenerator.Generate(project));

            return listOfLines;
        }

        internal List<string> GenerateDataObjects(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            var dataGenerator = new LivingDocDataObjectsGenerator();
            listOfLines.AddRange(dataGenerator.Generate(project));

            return listOfLines;
        }

        internal List<string> GenerateDataAnalytics(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            var dataGenerator = new LivingDocDataAnalyticsGenerator();
            listOfLines.AddRange(dataGenerator.Generate(project));

            return listOfLines;
        }

        internal List<string> GenerateDataEditor(LivingDocProject project)
        {
            var listOfLines = new List<string>();

            var dataGenerator = new LivingDocDataEditorGenerator();
            listOfLines.AddRange(dataGenerator.Generate(project));

            return listOfLines;
        }

        internal List<string> GenerateBodyFooter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("</body>");

            return listOfLines;
        }
    }
}
