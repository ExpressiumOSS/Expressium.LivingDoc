using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocContentGenerator
    {
        private LivingDocProject project;

        internal LivingDocContentGenerator(LivingDocProject project)
        {
            this.project = project;
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

            if (project.ExperimentFlag)
            {
                // Navigation Facelift Version 2.0.0
                listOfLines.Add("<!-- Project Navigation Section -->");
                listOfLines.Add("<nav class='navigation'>");

                listOfLines.Add("<a class='navigation-link' title='Overview' href='#' onclick=\"loadViewMode('project-view','Overview');\">Overview</a>");
                listOfLines.Add("<a class='navigation-link' title='Features List View' href='#' onclick=\"loadViewMode('features-view','Features');\">Features</a>");
                listOfLines.Add("<a class='navigation-link' title='Scenarios List View' href='#' onclick=\"loadViewMode('scenarios-view','Scenarios');\">Scenarios</a>");
                listOfLines.Add("<a class='navigation-link' title='Steps List View' href='#' onclick=\"loadViewMode('steps-view','Steps');\">Steps</a>");
                listOfLines.Add("<a class='navigation-link' title='Analytics' href='#' onclick=\"loadAnalytics()\">Analytics</a>");

                listOfLines.Add("</nav>");
            }
            else
            {
                listOfLines.Add("<!-- Project Navigation Section -->");
                listOfLines.Add("<nav class='navigation'>");

                listOfLines.Add("<span>|</span>");
                listOfLines.Add("<a class='navigation-link' title='Overview' href='#' onclick=\"loadViewMode('project-view','Overview');\">Overview</a>");
                listOfLines.Add("<span>|</span>");

                listOfLines.Add("<a class='navigation-link' title='Features List View' href='#' onclick=\"loadViewMode('features-view','Features');\">Features</a>");
                listOfLines.Add("<span>|</span>");

                listOfLines.Add("<a class='navigation-link' title='Scenarios List View' href='#' onclick=\"loadViewMode('scenarios-view','Scenarios');\">Scenarios</a>");
                listOfLines.Add("<span>|</span>");

                listOfLines.Add("<a class='navigation-link' title='Steps List View' href='#' onclick=\"loadViewMode('steps-view','Steps');\">Steps</a>");
                listOfLines.Add("<span>|</span>");

                listOfLines.Add("<a class='navigation-link' title='Analytics' href='#' onclick=\"loadAnalytics()\">Analytics</a>");
                listOfLines.Add("<span>|</span>");

                listOfLines.Add("</nav>");
            }

            return listOfLines;
        }

        internal List<string> GenerateSplitter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Splitter Wrapper Section -->");
            listOfLines.Add("<div class='splitter-wrapper'>");

            listOfLines.Add("<!-- Left Splitter Section -->");
            if (project.ExperimentFlag)
                listOfLines.Add("<div id='splitter-left' class='bg-white p-3'>");
            else
                listOfLines.Add("<div id='splitter-left' class='bg-light p-3'>");
            listOfLines.AddRange(GenerateViewPreFilters());
            listOfLines.AddRange(GenerateFilter());
            listOfLines.Add("<div id='filter-list'></div>");
            listOfLines.Add("</div>");

            listOfLines.Add("<!-- Splitter Section -->");
            listOfLines.Add("<div id='splitter'></div>");

            listOfLines.Add("<!-- Right Splitter Section -->");
            if (project.ExperimentFlag)
                listOfLines.Add("<div id='splitter-right' class='bg-white p-3'>");
            else
                listOfLines.Add("<div id='splitter-right' class='bg-light p-3'>");
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
            listOfLines.Add("<button class='filter-option' data-prefilter='Passed' title='Preset Filter with Passed' onclick='togglePrefilter(this)'>Passed</button>");
            listOfLines.Add("<button class='filter-option' data-prefilter='Incomplete' title='Preset Filter with Incomplete' onclick='togglePrefilter(this)'>Incomplete</button>");
            listOfLines.Add("<button class='filter-option' data-prefilter='Failed' title='Preset Filter with Failed' onclick='togglePrefilter(this)'>Failed</button>");
            listOfLines.Add("<button class='filter-option' data-prefilter='Skipped' title='Preset Filter with Skipped' onclick='togglePrefilter(this)'>Skipped</button>");
            listOfLines.Add("<button class='selected' title='Clear Filters' onclick='clearPrefilters()'>Clear</button>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFilter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Filter Section -->");

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<div class='layout-row filter-group'>");

            listOfLines.Add("<div class='filter-symbol'>");
            listOfLines.Add("<span class='bi bi-search'></span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<div style='width: 100%'>");
            listOfLines.Add("<input onkeyup='filterView()' class='filter-keywords' id='filter-by-keywords' type='text' placeholder='Filter by Keywords'>");
            listOfLines.Add("</div>");

            listOfLines.Add("</div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateFooter()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Footer Section -->");
            listOfLines.Add("<footer>");
            listOfLines.Add("<a title='Expressium LivingDoc on GitHub' href='https://github.com/ExpressiumOSS/Expressium.LivingDoc' target='_blank' rel='noopener noreferrer'>Generated by Expressium LivingDoc</a>");
            listOfLines.Add("</footer>");

            return listOfLines;
        }
    }
}
