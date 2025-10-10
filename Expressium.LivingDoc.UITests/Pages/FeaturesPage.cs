using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class FeaturesPage : BasePage
    {
        public FilterBar FilterBar { get; private set; }
        public BaseTable Grid { get; private set; }

        private readonly By Headline = By.XPath("//*[@id='view-title' and text()='Features']");

        public FeaturesPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            FilterBar = new FilterBar(logger, driver);
            Grid = new BaseTable(logger, driver, By.XPath("//*[@id='left-section']//*[@id='table-grid']"));

            WaitForPageElementIsVisible(Headline);
        }

        public string GetHeadline()
        {
            logger.Info("GetHeadline()");
            return Headline.GetText(driver);
        }

        #region Extensions

        #endregion
    }
}
