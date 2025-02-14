using Expressium.TestExecution;
using Expressium.LivingDoc.Extensions;
using Expressium.TestExecutionReport.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc
{
    internal class LivingDocBodyGenerator
    {
        private bool includeNavigation = false;

        internal List<string> GenerateBody(LivingDocProject project)
        {
            var dataGenerator = new LivingDocDataGenerator();

            List<string> listOfLines = new List<string>();

            listOfLines.AddRange(GenerateBodyHeader());
            listOfLines.AddRange(GenerateHeader(project));
            listOfLines.AddRange(GenerateNavigation(project));
            listOfLines.AddRange(GenerateContent(project));
            listOfLines.AddRange(GenerateFooter(project));
            listOfLines.AddRange(dataGenerator.GenerateData(project));
            listOfLines.AddRange(GenerateBodyFooter());

            return listOfLines;
        }

        internal List<string> GenerateBodyHeader()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<body onload=\"loadAnalytics('analytics'); loadViewmode('treeview');\">");

            return listOfLines;
        }

        internal List<string> GenerateHeader(LivingDocProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Header Section -->");
            listOfLines.Add("<header>");
            listOfLines.Add("<span class='project-name'>" + project.Title + "</span><br />");
            listOfLines.Add("<span class='project-date'>generated " + project.GetExecutionTime() + "</span>");
            listOfLines.Add("</header>");

            return listOfLines;
        }

        internal List<string> GenerateNavigation(LivingDocProject project)
        {
            List<string> listOfLines = new List<string>();

            if (includeNavigation)
            {
                listOfLines.Add("<!-- Project Navigation Section -->");
                listOfLines.Add("<nav class='navigation'>");
                listOfLines.Add("<a href='#' style='color: white;' onclick=\"loadAnalytics('analytics');\">Analytics</a>");
                listOfLines.Add("</nav>");
            }

            return listOfLines;
        }

        internal List<string> GenerateContent(LivingDocProject project)
        {
            List<string> listOfLines = new List<string>();

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
            listOfLines.AddRange(Resources.SplitterScript.Split(Environment.NewLine).ToList());

            return listOfLines;
        }

        internal List<string> GenerateScenarioPreFilters(LivingDocProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features PreFilters Section -->");
            listOfLines.Add("<div class='section layout-row'>");

            listOfLines.Add("<div class='layout-column align-left'>");
            listOfLines.Add("<button title='Project Tree View' onclick=\"loadViewmode('treeview');\">&#9776;</button>");
            listOfLines.Add("<button title='Feature List View' onclick=\"loadViewmode('featurelistview');\">&#9782;</button>");
            listOfLines.Add("<button title='Scenario List View' onclick=\"loadViewmode('scenariolistview');\">&#9783;</button>");
            listOfLines.Add("<button title='Analytics' onclick=\"loadAnalytics('analytics');\">&#425;</button>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div class='layout-column align-right'>");
            listOfLines.Add("<button title='Preset Filter with Passed' class='color-passed' onclick='presetFilter(\"passed\")'>Passed</button>");
            listOfLines.Add("<button title='Preset Filter with Incomplete' class='color-incomplete' onclick='presetFilter(\"incomplete\")'>Incomplete</button>");
            listOfLines.Add("<button title='Preset Filter with Failed' class='color-failed' onclick='presetFilter(\"failed\")'>Failed</button>");
            listOfLines.Add("<button title='Preset Filter with Skipped' class='color-skipped' onclick='presetFilter(\"skipped\")'>Skipped</button>");
            listOfLines.Add("<button title='Clear Filter' onclick='presetFilter(\"\")'>Clear</button>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateScenarioFilter(LivingDocProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Features Filter Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<input class='filter' onkeyup='filterScenarios()' id='scenario-filter' type='text' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFooter(LivingDocProject project)
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("<!-- Footer Section -->");
            listOfLines.Add("<footer>");
            listOfLines.Add("©2025 Expressium All Rights Reserved");
            listOfLines.Add("</footer>");

            return listOfLines;
        }

        internal List<string> GenerateBodyFooter()
        {
            List<string> listOfLines = new List<string>();

            listOfLines.Add("</body>");

            return listOfLines;
        }
    }
}
