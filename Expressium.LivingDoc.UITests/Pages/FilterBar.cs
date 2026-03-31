using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class FilterBar : BasePage
    {
        private readonly By Heading = By.Id("view-title");
        private readonly By Passed = By.XPath("//button[normalize-space()='Passed']");
        private readonly By Incomplete = By.XPath("//button[normalize-space()='Incomplete']");
        private readonly By Failed = By.XPath("//button[normalize-space()='Failed']");
        private readonly By Skipped = By.XPath("//button[normalize-space()='Skipped']");
        private readonly By Clear = By.XPath("//button[@title='Clear All Filters']");
        private readonly By FilterByKeywords = By.Id("filter-by-keywords");

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
