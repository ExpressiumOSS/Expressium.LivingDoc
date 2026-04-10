using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class AnalyticsPage : BasePage
    {
        private readonly By PageTitle = By.XPath("//*[@id='splitter-right']//*[@data-testid='page-title']");

        private readonly By FeaturesChartTitle = By.XPath("//*[@id='splitter-right']//*[@data-testid='features-chart-title']");
        private readonly By FeaturesChartPassed = By.XPath("//*[@id='splitter-right']//*[@data-testid='features-chart-passed']");

        private readonly By ScenariosChartTitle = By.XPath("//*[@id='splitter-right']//*[@data-testid='scenarios-chart-title']");
        private readonly By ScenariosChartPassed = By.XPath("//*[@id='splitter-right']//*[@data-testid='scenarios-chart-passed']");

        private readonly By StepsChartTitle = By.XPath("//*[@id='splitter-right']//*[@data-testid='steps-chart-title']");
        private readonly By StepsChartPassed = By.XPath("//*[@id='splitter-right']//*[@data-testid='steps-chart-passed']");

        private readonly By ProjectDuration = By.XPath("//*[@id='splitter-right']//*[@data-testid='project-duration']");

        private readonly By ChartPassedCountPercentage = By.XPath("//*[@id='splitter-right']//*[@class='color-passed chart-count']//*[@class='chart-count-percentage']");
        private readonly By ChartIncompleteCountPercentage = By.XPath("//*[@id='splitter-right']//*[@class='color-incomplete chart-count']//*[@class='chart-count-percentage']");
        private readonly By ChartFailedCountPercentage = By.XPath("//*[@id='splitter-right']//*[@class='color-failed chart-count']//*[@class='chart-count-percentage']");
        private readonly By ChartSkippedCountPercentage = By.XPath("//*[@id='splitter-right']//*[@class='color-skipped chart-count']//*[@class='chart-count-percentage']");

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

        public string GetPassedCountPercentage()
        {
            logger.Info("GetPassedCountPercentage()");
            return ChartPassedCountPercentage.GetText(driver);
        }

        public string GetIncompleteCountPercentage()
        {
            logger.Info("GetIncompleteCountPercentage()");
            return ChartIncompleteCountPercentage.GetText(driver);
        }

        public string GetFailedCountPercentage()
        {
            logger.Info("GetFailedCountPercentage()");
            return ChartFailedCountPercentage.GetText(driver);
        }

        public string GetSkippedCountPercentage()
        {
            logger.Info("GetSkippedCountPercentage()");
            return ChartSkippedCountPercentage.GetText(driver);
        }

        #region Extensions

        #endregion
    }
}
