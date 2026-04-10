using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataGenerator
    {
        private LivingDocProject project;

        internal LivingDocDataGenerator(LivingDocProject project)
        {
            this.project = project;
        }

        internal List<string> GenerateDataOverview()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataOverviewGenerator(project);
            listOfLines.AddRange(generator.GenerateDataOverview());

            return listOfLines;
        }

        internal List<string> GenerateDataListViews()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataListViewsGenerator(project);
            listOfLines.AddRange(generator.GenerateDataFeaturesListView());
            listOfLines.AddRange(generator.GenerateDataScenariosListView());
            listOfLines.AddRange(generator.GenerateDataStepsListView());

            return listOfLines;
        }

        internal List<string> GenerateDataObjects()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataObjectsGenerator(project);
            listOfLines.AddRange(generator.GenerateDataFeatures());
            listOfLines.AddRange(generator.GenerateDataScenarios());

            return listOfLines;
        }

        internal List<string> GenerateDataAnalytics()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataAnalyticsGenerator(project);
            listOfLines.AddRange(generator.GenerateDataAnalyticsFeaturesView());
            listOfLines.AddRange(generator.GenerateDataAnalyticsScenariosView());
            listOfLines.AddRange(generator.GenerateDataAnalyticsStepsView());

            return listOfLines;
        }
    }
}
