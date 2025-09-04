using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using log4net;
using Expressium.LivingDoc.UITests.Controls;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class MenuBar : BasePage
    {
        private readonly By Overview = By.XPath("//a[text()='Overview']");
        private readonly By Features = By.XPath("//a[text()='Features']");
        private readonly By Scenarios = By.XPath("//a[text()='Scenarios']");
        private readonly By Steps = By.XPath("//a[text()='Steps']");
        private readonly By Analytics = By.XPath("//a[text()='Analytics']");

        public MenuBar(ILog logger, IWebDriver driver) : base(logger, driver)
        {
        }

        public void ClickOverview()
        {
            logger.Info($"ClickOverview()");
            Overview.ClickLink(driver);
        }

        public void ClickFeatures()
        {
            logger.Info($"ClickFeatures()");
            Features.ClickLink(driver);
        }

        public void ClickScenarios()
        {
            logger.Info($"ClickScenarios()");
            Scenarios.ClickLink(driver);
        }

        public void ClickSteps()
        {
            logger.Info($"ClickSteps()");
            Steps.ClickLink(driver);
        }

        public void ClickAnalytics()
        {
            logger.Info($"ClickAnalytics()");
            Analytics.ClickLink(driver);
        }

        #region Extensions

        #endregion
    }
}
