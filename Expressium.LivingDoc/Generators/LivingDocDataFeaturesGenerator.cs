using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc
{
    internal class LivingDocDataFeaturesGenerator
    {
        private LivingDocProject project;

        internal LivingDocDataFeaturesGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> Generate()
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateDataFeatures());

            return listOfLines;
        }

        internal List<string> GenerateDataFeatures()
        {
            var listOfLines = new List<string>();

            foreach (var feature in project.Features)
            {
                listOfLines.Add("<!-- Data Feature -->");
                listOfLines.Add($"<div class='data-item' id='{feature.Id}'>");

                listOfLines.Add("<!-- Feature Section -->");
                listOfLines.Add("<div class='section'>");
                listOfLines.AddRange(GenerateDataFeatureTags(feature));
                listOfLines.AddRange(GenerateDataFeatureName(feature));
                listOfLines.AddRange(GenerateDataFeatureDescription(feature));
                listOfLines.AddRange(GenerateDataFeatureBackground(feature));
                listOfLines.Add("</div>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureTags(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Feature Tags -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<span class='tag-names'>" + feature.GetTags() + "</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureName(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Feature Name -->");
            listOfLines.Add("<div>");
            listOfLines.Add($"<span class='status-dot bgcolor-{feature.GetStatus().ToLower()}'></span>");
            listOfLines.Add($"<span class='feature-keyword'>Feature: </span><span class='feature-name'>{feature.Name}</span>");
            //listOfLines.Add($"<span class='feature-duration'>&nbsp;{feature.GetDuration()}</span>");
            listOfLines.Add("</div>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureDescription(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            if (feature.Description != null)
            {
                listOfLines.Add("<!-- Data Feature Description -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<ul class='feature-description'>");
                var listOfDescription = feature.Description.Trim().Split("\n");
                foreach (var line in listOfDescription)
                    listOfLines.Add("<li>" + line.Trim() + "</li>");
                listOfLines.Add("</ul>");
                listOfLines.Add("</div>");
            }

            listOfLines.Add("<hr>");

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureBackground(LivingDocFeature feature)
        {
            var listOfLines = new List<string>();

            if (feature.Background != null && feature.Background.Steps.Count > 0)
            {
                listOfLines.Add("<!-- Data Feature Background -->");
                listOfLines.Add("<div>");
                listOfLines.Add("<span class='background-keyword'>Background:</span>");
                listOfLines.AddRange(GenerateDataFeatureBackgroundSteps(feature.Background.Steps));
                listOfLines.Add("<hr>");
                listOfLines.Add("</div>");
            }

            return listOfLines;
        }

        internal List<string> GenerateDataFeatureBackgroundSteps(List<LivingDocStep> steps)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("<!-- Data Background Steps -->");
            listOfLines.Add("<div>");
            listOfLines.Add("<ul class='scenario-steps'>");

            foreach (var step in steps)
            {
                listOfLines.Add("<li>");
                listOfLines.Add($"<span class='color-skipped'></span>");
                listOfLines.Add($"<span class='step-keyword'>" + step.Keyword + "</span>");
                listOfLines.Add($"<span>" + step.Name + "</span>");
                listOfLines.Add("</li>");
            }

            listOfLines.Add("</ul>");
            listOfLines.Add("</div>");

            return listOfLines;
        }
    }
}
