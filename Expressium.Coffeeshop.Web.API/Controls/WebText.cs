using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebText : WebControl
    {
        public WebText(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebText(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }

        public string GetText()
        {
            WaitForElementIsVisible();
            HighlightElementAsValidation();
            return GetElementText();
        }

        protected virtual string GetElementText()
        {
            return GetElement().Text.Trim();
        }
    }
}
