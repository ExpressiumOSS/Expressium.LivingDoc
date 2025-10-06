using Expressium.LivingDoc.Models;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Generators
{
    internal class LivingDocDataGenerator
    {
        private LivingDocProject project;
        private LivingDocConfiguration configuration;

        internal LivingDocDataGenerator(LivingDocProject project, LivingDocConfiguration configuration)
        {
            this.project = project;
            this.configuration = configuration;
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

            if (configuration.FeaturesListView)
                listOfLines.AddRange(generator.GenerateDataFeaturesListView());

            if (configuration.ScenariosListView)
                listOfLines.AddRange(generator.GenerateDataScenariosListView());

            if (configuration.StepsListView)
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

            if (configuration.FeaturesListView)
                listOfLines.AddRange(generator.GenerateDataAnalyticsFeaturesView());

            listOfLines.AddRange(generator.GenerateDataAnalyticsScenariosView());

            if (configuration.StepsListView)
                listOfLines.AddRange(generator.GenerateDataAnalyticsStepsView());

            return listOfLines;
        }

        internal List<string> GenerateDataEditor()
        {
            var listOfLines = new List<string>();

            var generator = new LivingDocDataEditorGenerator(project);

            if (configuration.EditorView)
                listOfLines.AddRange(generator.GenerateDataEditor());

            return listOfLines;
        }
    }
}
