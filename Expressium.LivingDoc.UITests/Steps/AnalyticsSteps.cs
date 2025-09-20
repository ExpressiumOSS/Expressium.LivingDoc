using Expressium.LivingDoc.UITests.Pages;
using Expressium.LivingDoc.UITests.Utilities;
using Reqnroll;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class Analytics : BaseSteps
    {
        public Analytics(BaseContext baseContext) : base(baseContext)
        {
        }

        [Given("I have navigated to the Features Analytics")]
        public void GivenIHaveNavigatedToTheFeaturesAnalytics()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickFeatures();

            var analyticsPage = new AnalyticsPage(logger, driver);
            Asserts.EqualTo(analyticsPage.GetPageTitle(), "Analytics", "The Analytics page headline is valid");
            Asserts.EqualTo(analyticsPage.GetFeaturesChartTitle(), "Features", "The Analytics page chart name is valid");
        }

        [Given("I have navigated to the Scenarios Analytics")]
        public void GivenIHaveNavigatedToTheScenariosAnalytics()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickScenarios();

            var analyticsPage = new AnalyticsPage(logger, driver);
            Asserts.EqualTo(analyticsPage.GetPageTitle(), "Analytics", "The Analytics page headline is valid");
            Asserts.EqualTo(analyticsPage.GetScenariosChartTitle(), "Scenarios", "The Analytics page chart name is valid");
        }

        [Given("I have navigated to the Steps Analytics")]
        public void GivenIHaveNavigatedToTheStepsAnalytics()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickSteps();

            var analyticsPage = new AnalyticsPage(logger, driver);
            Asserts.EqualTo(analyticsPage.GetPageTitle(), "Analytics", "The Analytics page headline is valid");
            Asserts.EqualTo(analyticsPage.GetStepsChartTitle(), "Steps", "The Analytics page chart name is valid");
        }

        [Then("I should have the passed features displayed in Analytics")]
        public void ThenIShouldHaveThePassedFeaturesDisplayedInAnalytics()
        {
            var analyticsPage = new AnalyticsPage(logger, driver);
            Asserts.EqualTo(analyticsPage.GetFeaturesChartPassed(), "25%", "The Analytics page passed is valid");
        }

        [Then("I should have the passed scenarios displayed in Analytics")]
        public void ThenIShouldHaveThePassedScenariosDisplayedInAnalytics()
        {
            var analyticsPage = new AnalyticsPage(logger, driver);
            Asserts.EqualTo(analyticsPage.GetScenariosChartPassed(), "50%", "The Analytics page passed is valid");
        }

        [Then("I should have the passed steps displayed in Analytics")]
        public void ThenIShouldHaveThePassedStepsDisplayedInAnalytics()
        {
            var analyticsPage = new AnalyticsPage(logger, driver);
            Asserts.EqualTo(analyticsPage.GetStepsChartPassed(), "69%", "The Analytics page passed is valid");
        }

        [Then("I should have the duration displayed in Analytics")]
        public void ThenIShouldHaveTheDurationDisplayedInAnalytics()
        {
            var analyticsPage = new AnalyticsPage(logger, driver);
            Asserts.IsNotEmpty(analyticsPage.GetProjectDuration(), "The Analytics page duration is valid");
        }
    }
}
