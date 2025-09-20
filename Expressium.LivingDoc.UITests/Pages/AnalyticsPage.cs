using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class AnalyticsPage : BasePage
    {
        private readonly By PageTitle = By.XPath("//*[@id='right-section']//*[@data-testid='page-title']");

        private readonly By FeaturesChartTitle = By.XPath("//*[@id='right-section']//*[@data-testid='features-chart-title']");
        private readonly By FeaturesChartPassed = By.XPath("//*[@id='right-section']//*[@data-testid='features-chart-passed']");

        private readonly By ScenariosChartTitle = By.XPath("//*[@id='right-section']//*[@data-testid='scenarios-chart-title']");
        private readonly By ScenariosChartPassed = By.XPath("//*[@id='right-section']//*[@data-testid='scenarios-chart-passed']");

        private readonly By StepsChartTitle = By.XPath("//*[@id='right-section']//*[@data-testid='steps-chart-title']");
        private readonly By StepsChartPassed = By.XPath("//*[@id='right-section']//*[@data-testid='steps-chart-passed']");

        private readonly By ProjectDuration = By.XPath("//*[@id='right-section']//*[@data-testid='project-duration']");

        public AnalyticsPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            WaitForPageElementIsVisible(PageTitle);
        }

        public string GetPageTitle()
        {
            logger.Info("GetPageTitle()");
            return PageTitle.GetText(driver);
        }

        public string GetFeaturesChartTitle()
        {
            logger.Info("GetFeaturesChartTitle()");
            return FeaturesChartTitle.GetText(driver);
        }

        public string GetFeaturesChartPassed()
        {
            logger.Info("GetFeaturesChartPassed()");
            return FeaturesChartPassed.GetText(driver);
        }

        public string GetScenariosChartTitle()
        {
            logger.Info("GetScenariosChartTitle()");
            return ScenariosChartTitle.GetText(driver);
        }

        public string GetScenariosChartPassed()
        {
            logger.Info("GetScenariosChartPassed()");
            return ScenariosChartPassed.GetText(driver);
        }

        public string GetStepsChartTitle()
        {
            logger.Info("GetStepsChartTitle()");
            return StepsChartTitle.GetText(driver);
        }

        public string GetStepsChartPassed()
        {
            logger.Info("GetStepsChartPassed()");
            return StepsChartPassed.GetText(driver);
        }

        public string GetProjectDuration()
        {
            logger.Info("GetProjectDuration()");
            return ProjectDuration.GetText(driver);
        }

        #region Extensions

        #endregion
    }
}
