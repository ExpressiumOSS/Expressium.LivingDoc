using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebLink : WebButton
    {
        public WebLink(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebLink(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }
    }
}
