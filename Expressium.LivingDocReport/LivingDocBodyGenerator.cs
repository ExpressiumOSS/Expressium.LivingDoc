using Expressium.LivingDoc;
using Expressium.LivingDocReport.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDocReport
{
    internal class LivingDocBodyGenerator
    {
        private bool includeNavigation = true;

        internal List<string> GenerateBody(LivingDocProject project)
        {
            var dataGenerator = new LivingDocDataGenerator();

            var listOfLines = new List<string>();

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

            if (includeNavigation)
            {
                listOfLines.Add("<!-- Project Navigation Section -->");
                listOfLines.Add("<nav class='navigation' style='color: white;'>");
                listOfLines.Add("<span>|</span>");
                listOfLines.Add("<a title='Overview' style='color: white;' href='#' onclick=\"loadViewmode('project-view');\">Overview</a>");
                listOfLines.Add("<span>|</span>");
                listOfLines.Add("<a title='Features List View' style='color: white;' href='#' onclick=\"loadViewmode('features-view');\">Features</a>");
                listOfLines.Add("<span>|</span>");
                listOfLines.Add("<a title='Scenarios List View' style='color: white;' href='#' onclick=\"loadViewmode('scenarios-view');\">Scenarios</a>");
                listOfLines.Add("<span>|</span>");
                listOfLines.Add("<a title='Steps List View' style='color: white;' href='#' onclick=\"loadViewmode('steps-view');\">Steps</a>");
                listOfLines.Add("<span>|</span>");
                listOfLines.Add("<a title='Analytics' style='color: white;' href='#' onclick=\"loadAnalytics('analytics');\">Analytics</a>");
                listOfLines.Add("<span>|</span>");
                listOfLines.Add("</nav>");
            }

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

            if (!includeNavigation)
            {
                listOfLines.Add("<div class='layout-column align-left'>");
                listOfLines.Add("<button title='Overview' onclick=\"loadViewmode('project-view');\">&#9776;</button>");
                listOfLines.Add("<button title='Features List View' onclick=\"loadViewmode('features-view');\">&#9782;</button>");
                listOfLines.Add("<button title='Scenarios List View' onclick=\"loadViewmode('scenarios-view');\">&#9783;</button>");
                listOfLines.Add("<button title='Analytics' onclick=\"loadAnalytics('analytics');\">&#425;</button>");
                listOfLines.Add("</div>");
            }

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

        internal List<string> GenerateBodyFooter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("</body>");

            return listOfLines;
        }
    }
}
