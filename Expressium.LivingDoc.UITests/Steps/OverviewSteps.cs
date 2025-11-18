using Expressium.LivingDoc.UITests.Pages;
using Expressium.LivingDoc.UITests.Utilities;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class OverviewSteps : BaseSteps
    {
        public OverviewSteps(BaseContext baseContext) : base(baseContext)
        {
        }

        [Given("I have navigated to the Overview List")]
        public void GivenIHaveNavigatedToTheOverviewList()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickOverview();

            var overviewPage = new OverviewPage(logger, driver);
            Asserts.EqualTo(overviewPage.GetHeadline(), "Overview", "The Overview page headline is valid");
        }

        [Given("I have navigated with deep link argument to the Overview List")]
        public void GivenIHaveNavigatedWithDeepLinkArgumentToTheOverviewList(DataTable dataTable)
        {
            var filters = dataTable.CreateSet<Filters>();

            foreach (var filter in filters)
                driver.Navigate().GoToUrl(driver.Url + "?filterByKeywords=" + filter.Keywords);
        }

        [Then("I should have following number of visible objects in the Overview")]
        public void ThenIShouldHaveFollowingNumberOfVisibleObjectsInTheOverview(DataTable dataTable)
        {
            var objects = dataTable.CreateInstance<Objects>();

            var overviewPage = new OverviewPage(logger, driver);
            Asserts.EqualTo(overviewPage.Grid.GetNumberOfFolders(), int.Parse(objects.Folders), "Validating the OverviewPage Number Of Folders...");
            Asserts.EqualTo(overviewPage.Grid.GetNumberOfFeatures(), int.Parse(objects.Features), "Validating the OverviewPage Number Of Features...");
            Asserts.EqualTo(overviewPage.Grid.GetNumberOfScenarios(), int.Parse(objects.Scenarios), "Validating the OverviewPage Number Of Scenarios...");
        }

        [Then("I should have following visible objects in the Overview")]
        public void ThenIShouldHaveFollowingVisibleObjectsInTheOverview(DataTable dataTable)
        {
            var objects = dataTable.CreateSet<Objects>();

            var overviewPage = new OverviewPage(logger, driver);

            // Validate the folders in the overview page
            var listOfFolders = new List<string>();
            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Folders))
                    listOfFolders.Add(item.Folders);
            }

            Asserts.EqualTo(overviewPage.Grid.GetNumberOfFolders(), listOfFolders.Count, "Validating the OverviewPage Number Of Folders...");
            Asserts.EqualTo(overviewPage.Grid.GetFolderDataNames(), listOfFolders, "Validating the OverviewPage Folder Data Names...");

            // Validate the features in the overview page
            var listOfFeatures = new List<string>();
            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Features))
                    listOfFeatures.Add(item.Features);
            }

            Asserts.EqualTo(overviewPage.Grid.GetNumberOfFeatures(), listOfFeatures.Count, "Validating the OverviewPage Number Of Features...");
            Asserts.EqualTo(overviewPage.Grid.GetFeatureDataNames(), listOfFeatures, "Validating the OverviewPage Feature Data Names...");

            // Validate the scenarios in the overview page
            var listOfScenarios = new List<string>();
            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Scenarios))
                    listOfScenarios.Add(item.Scenarios);
            }

            Asserts.EqualTo(overviewPage.Grid.GetNumberOfScenarios(), listOfScenarios.Count(), "Validating the OverviewPage Number Of Scenarios...");
            Asserts.EqualTo(overviewPage.Grid.GetScenarioDataNames(), listOfScenarios, "Validating the OverviewPage Scenario Data Names...");
        }

        [When("I select the collapse all features in the Overview")]
        public void WhenISelectTheCollapseAllFeaturesInTheOverview()
        {
            var overviewPage = new OverviewPage(logger, driver);
            overviewPage.Grid.CollapseAllFeatures();
        }

        [When("I select the expand all features in the Overview")]
        public void WhenISelectTheExpandAllFeaturesInTheOverview()
        {
            var overviewPage = new OverviewPage(logger, driver);
            overviewPage.Grid.ExpandAllFeatures();
        }

        [When("I select the collapse a feature in the Overview")]
        public void WhenISelectTheCollapseAFeatureInTheOverview()
        {
            var overviewPage = new OverviewPage(logger, driver);
            overviewPage.Grid.ToggleExpandFeatures();
        }

        [When("I select the expand a feature in the Overview")]
        public void WhenISelectTheExpandAFeatureInTheOverview()
        {
            var overviewPage = new OverviewPage(logger, driver);
            overviewPage.Grid.ToggleExpandFeatures();
        }

        [When("I load the feature document in the Overview List")]
        public void WhenILoadTheFeatureDocumentInTheOverviewList()
        {
            var overviewPage = new OverviewPage(logger, driver);
            overviewPage.Grid.ClickCellByDataRole("feature");
        }

        [When("I load the scenario document in the Overview List")]
        public void WhenILoadTheScenarioDocumentInTheOverviewList()
        {
            var overviewPage = new OverviewPage(logger, driver);
            overviewPage.Grid.ClickCellByDataRole("scenario");
        }

        private class Objects
        {
            public string Folders { get; set; }
            public string Features { get; set; }
            public string Scenarios { get; set; }
        }

        private class Filters
        {
            public string Keywords { get; set; }
        }
    }
}
