using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataPropertiesGenerator
    {
        private LivingDocProject project;

        internal LivingDocDataPropertiesGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> GenerateDataProjectPropertiesView()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Properties -->");
            listOfLines.Add("<div id='properties'>");

            listOfLines.AddRange(GenerateDataPropertiesTitle());
            listOfLines.AddRange(GenerateDataPropertiesSections());

            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataPropertiesTitle()
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<span class='page-name' data-testid='page-title'>Properties</span>");
            listOfLines.Add("</div>");

            listOfLines.Add("<hr>");
            listOfLines.Add("<hr>");

            return listOfLines;
        }

        internal List<string> GenerateDataPropertiesSections()
        {
            var listOfLines = new List<string>();

            if (!project.HasProperties())
                return listOfLines;

            foreach (var section in project.Properties.Sections)
            {
                if (section.Properties.Count == 0)
                    continue;

                listOfLines.AddRange(GenerateDataPropertiesSection(section));
            }

            return listOfLines;
        }

        internal List<string> GenerateDataPropertiesSection(LivingDocPropertySection section)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<hr>");
            listOfLines.Add("<!-- Data Properties Section -->");
            listOfLines.Add("<div class='section properties-section'>");
            listOfLines.Add($"<span class='chart-name'>{section.Name}</span>");
            listOfLines.Add("<div class='section'>");
            listOfLines.Add("<table class='properties-list'>");
            listOfLines.Add("<thead>");
            listOfLines.Add("<tr>");
            listOfLines.Add("<th width='50%'>Name</th>");
            listOfLines.Add("<th width='50%'>Value</th>");
            listOfLines.Add("</tr>");
            listOfLines.Add("</thead>");
            listOfLines.Add("<tbody>");

            foreach (var property in section.Properties)
                listOfLines.Add($"<tr><td>{property.Name}</td><td>{property.Value}</td></tr>");

            listOfLines.Add("</tbody>");
            listOfLines.Add("</table>");
            listOfLines.Add("</div>");
            listOfLines.Add("</div>");

            return listOfLines;
        }
    }
}
