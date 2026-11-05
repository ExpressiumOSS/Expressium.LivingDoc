using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class PropertiesPage : BasePage
    {
        private readonly By PageTitle = By.XPath("//*[@id='splitter-right']//*[@data-testid='page-title']");
        private readonly By SectionNames = By.XPath("//*[@id='splitter-right']//div[contains(@class,'properties-section')]/span[@class='chart-name']");

        public PropertiesPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            WaitForPageElementIsVisible(PageTitle);
        }

        public string GetPageTitle()
        {
            logger.Info("GetPageTitle()");
            return PageTitle.GetText(driver);
        }

        public List<string> GetSectionNames()
        {
            logger.Info("GetSectionNames()");
            return SectionNames.GetTexts(driver);
        }

        public List<string> GetPropertyNames(string section)
        {
            logger.Info($"GetPropertyNames({section})");
            return By.XPath(GetSectionRowsXPath(section) + "/td[1]").GetTexts(driver);
        }

        public string GetPropertyValue(string section, string name)
        {
            logger.Info($"GetPropertyValue({section}, {name})");
            return By.XPath(GetSectionRowsXPath(section) + $"[td[1][normalize-space()='{name}']]/td[2]").GetText(driver);
        }

        #region Extensions

        private static string GetSectionRowsXPath(string section)
        {
            return $"//*[@id='splitter-right']//div[contains(@class,'properties-section')][span[@class='chart-name' and normalize-space()='{section}']]//table[@class='properties-list']/tbody/tr";
        }

        #endregion
    }
}
