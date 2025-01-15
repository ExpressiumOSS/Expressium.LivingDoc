using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebButton : WebControl
    {
        public WebButton(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebButton(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }

        public virtual void Click()
        {
            WaitForElementIsEnabled();
            HighlightElementAsAction();
            HighlightElementClear();
            ClickElement();
        }
    }
}
