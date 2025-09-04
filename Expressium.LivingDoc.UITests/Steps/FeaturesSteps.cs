using Expressium.LivingDoc.UITests.Pages;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class FeaturesSteps : BaseSteps
    {
        public FeaturesSteps(BaseContext baseContext) : base(baseContext)
        {
        }

        [Given("I have navigated to the Features List")]
        public void GivenIHaveNavigatedToTheFeaturesList()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickFeatures();

            var featuresPage = new FeaturesPage(logger, driver);
            Asserts.EqualTo(featuresPage.GetHeadline(), "Features", "The Features page headline");
        }

        [Then("I should have following visible objects in the Features List")]
        public void ThenIShouldHaveFollowingVisibleObjectsInTheFeaturesList(DataTable dataTable)
        {
            var objects = dataTable.CreateSet<Objects>();

            var listOfFeatures = new List<string>();
            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Features))
                    listOfFeatures.Add(item.Features);
            }

            var featuresPage = new FeaturesPage(logger, driver);

            Asserts.EqualTo(featuresPage.Grid.GetNumberOfFeatures(), listOfFeatures.Count(), "Validating the FeaturesPage Number Of Features...");
            Asserts.EqualTo(featuresPage.Grid.GetFeatureNames(), listOfFeatures, "Validating the FeaturesPage Feature Names...");
        }

        [Then("I should have following number of visible objects in the Features List")]
        public void ThenIShouldHaveFollowingNumberOfVisibleObjectsInTheFeaturesList(DataTable dataTable)
        {
            var objects = dataTable.CreateInstance<Objects>();

            var featuresPage = new FeaturesPage(logger, driver);
            Asserts.EqualTo(featuresPage.Grid.GetNumberOfFeatures(), int.Parse(objects.Features), "Validating the FeaturesPage Number Of Features...");
        }

        [When("I sort the features by (.*) column in the Features List")]
        public void WhenISortTheFeaturesByDescriptionColumnInTheFeaturesList(string value)
        {
            var featuresPage = new FeaturesPage(logger, driver);
            featuresPage.Grid.SortByColumn(value);
        }

        private class Objects
        {
            public string Features { get; set; }
        }
    }
}
