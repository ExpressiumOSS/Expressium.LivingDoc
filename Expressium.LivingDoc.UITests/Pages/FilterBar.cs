using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using log4net;
using Expressium.LivingDoc.UITests.Controls;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class FilterBar : BasePage
    {
        private readonly By Heading = By.Id("view-title");
        private readonly By Passed = By.XPath("//button[text()='Passed']");
        private readonly By Incomplete = By.XPath("//button[text()='Incomplete']");
        private readonly By Failed = By.XPath("//button[text()='Failed']");
        private readonly By Skipped = By.XPath("//button[text()='Skipped']");
        private readonly By Clear = By.XPath("//button[@title='Clear Filters']");
        private readonly By FilterByKeywords = By.Id("scenario-filter");

        public FilterBar(ILog logger, IWebDriver driver) : base(logger, driver)
        {
        }

        public string GetHeading()
        {
            logger.Info("GetHeading()");
            return Heading.GetText(driver);
        }

        public void ClickPassed()
        {
            logger.Info($"ClickPassed()");
            Passed.ClickButton(driver);
        }

        public void ClickIncomplete()
        {
            logger.Info($"ClickIncomplete()");
            Incomplete.ClickButton(driver);
        }

        public void ClickFailed()
        {
            logger.Info($"ClickFailed()");
            Failed.ClickButton(driver);
        }

        public void ClickSkipped()
        {
            logger.Info($"ClickSkipped()");
            Skipped.ClickButton(driver);
        }

        public void ClickClear()
        {
            logger.Info($"ClickClear()");
            Clear.ClickButton(driver);
        }

        public void SetFilterByKeywords(string value)
        {
            logger.Info($"SetFilterByKeywords({value})");
            FilterByKeywords.SetTextBox(driver, value);
        }

        public string GetFilterByKeywords()
        {
            logger.Info("GetFilterByKeywords()");
            return FilterByKeywords.GetTextBox(driver);
        }

        #region Extensions

        public bool IsPassedActive()
        {
            logger.Info("IsPassedActive()");
            return Passed.IsActive(driver);
        }

        public bool IsIncompleteActive()
        {
            logger.Info("IsIncompleteActive()");
            return Incomplete.IsActive(driver);
        }

        public bool IsFailedActive()
        {
            logger.Info("IsFailedActive()");
            return Failed.IsActive(driver);
        }

        public bool IsSkippedActive()
        {
            logger.Info("IsSkippedActive()");
            return Skipped.IsActive(driver);
        }

        #endregion
    }
}
