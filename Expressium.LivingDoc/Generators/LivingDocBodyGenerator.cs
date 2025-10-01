using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocBodyGenerator
    {
        private bool includeEditor = false;

        private LivingDocProject project;

        internal LivingDocBodyGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> Generate()
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateBodyHeader());
            listOfLines.AddRange(GenerateHeader());
            listOfLines.AddRange(GenerateNavigation());
            listOfLines.AddRange(GenerateContent());
            listOfLines.AddRange(GenerateFooter());
            listOfLines.AddRange(GenerateDataOverview());
            listOfLines.AddRange(GenerateDataListViews());
            listOfLines.AddRange(GenerateDataObjects());
            listOfLines.AddRange(GenerateDataAnalytics());
            listOfLines.AddRange(GenerateDataEditor());
            listOfLines.AddRange(GenerateBodyFooter());

            return listOfLines;
        }

        internal List<string> GenerateBodyHeader()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<body onload=\"loadViewmode('project-view','Overview'); loadAnalytics()\">");

            return listOfLines;
        }

        internal List<string> GenerateHeader()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Header Section -->");
            listOfLines.Add("<header>");
            listOfLines.Add($"<span class='project-name'>{project.Title}</span><br />");
            listOfLines.Add($"<span class='project-date'>Generated {project.GetDate()}</span>");
            listOfLines.Add("</header>");

            return listOfLines;
        }

        internal List<string> GenerateNavigation()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Project Navigation Section -->");
            listOfLines.Add("<nav class='navigation'>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Overview' href='#' onclick=\"loadViewmode('project-view','Overview');\">Overview</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Features List View' href='#' onclick=\"loadViewmode('features-view','Features');\">Features</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Scenarios List View' href='#' onclick=\"loadViewmode('scenarios-view','Scenarios');\">Scenarios</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Steps List View' href='#' onclick=\"loadViewmode('steps-view','Steps');\">Steps</a>");
            listOfLines.Add("<span>|</span>");
            listOfLines.Add("<a class='navigation-link' title='Analytics' href='#' onclick=\"loadAnalytics()\">Analytics</a>");
            listOfLines.Add("<span>|</span>");

            if (includeEditor)
            {
                listOfLines.Add("<a class='navigation-link' title='Gherkin Script Editor' href='#' onclick=\"loadEditor(); filterStepDefinitions();\">Editor</a>");
                listOfLines.Add("<span>|</span>");
            }

            listOfLines.Add("</nav>");

            return listOfLines;
        }

        internal List<string> GenerateContent()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Content Wrapper Section -->");
            listOfLines.Add("<div id='content-wrapper'>");

            listOfLines.Add("<!-- Left Content Section -->");
            listOfLines.Add("<div id='left-section' class='bg-light p-3'>");

            listOfLines.AddRange(GenerateViewPreFilters());
            listOfLines.AddRange(GenerateFilter());
            listOfLines.Add("<div id='list-view'></div>");
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

        internal List<string> GenerateViewPreFilters()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<div class='section layout-row'>");

            listOfLines.Add("<!-- View Title Section -->");
            listOfLines.Add("<div class='layout-column align-left'>");
            listOfLines.Add("<span id='view-title' class='page-name'>Overview</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<!-- PreFilters Section -->");
            listOfLines.Add("<div class='layout-column align-right'>");
            listOfLines.Add("<button data-prefilter='Passed' title='Preset Filter with Passed' class='status-option' onclick='togglePrefilter(this)'>Passed</button>");
            listOfLines.Add("<button data-prefilter='Incomplete' title='Preset Filter with Incomplete' class='status-option' onclick='togglePrefilter(this)'>Incomplete</button>");
            listOfLines.Add("<button data-prefilter='Failed' title='Preset Filter with Failed' class='status-option' onclick='togglePrefilter(this)'>Failed</button>");
            listOfLines.Add("<button data-prefilter='Skipped' title='Preset Filter with Skipped' class='status-option' onclick='togglePrefilter(this)'>Skipped</button>");
            listOfLines.Add("<button title='Clear Filters' onclick='clearPrefilters()'>Clear</button>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFilter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Filter Section -->");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<input class='filter' onkeyup='filterView()' id='scenario-filter' type='text' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFooter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Footer Section -->");
            listOfLines.Add("<footer>");
            listOfLines.Add("©2025 Expressium All Rights Reserved");
            listOfLines.Add("</footer>");

            return listOfLines;
        }

        internal List<string> GenerateDataOverview()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataOverviewGenerator(project);
            listOfLines.AddRange(generator.Generate());

            return listOfLines;
        }

        internal List<string> GenerateDataListViews()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataListViewsGenerator(project);
            listOfLines.AddRange(generator.Generate());

            return listOfLines;
        }

        internal List<string> GenerateDataObjects()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataObjectsGenerator(project);
            listOfLines.AddRange(generator.Generate());

            return listOfLines;
        }

        internal List<string> GenerateDataAnalytics()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataAnalyticsGenerator(project);
            listOfLines.AddRange(generator.Generate());

            return listOfLines;
        }

        internal List<string> GenerateDataEditor()
        {
            var listOfLines = new List<string>();

            if (includeEditor)
            {
                var generator = new LivingDocDataEditorGenerator(project);
                listOfLines.AddRange(generator.Generate());
            }

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
