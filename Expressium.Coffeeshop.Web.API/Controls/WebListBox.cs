using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebListBox : WebComboBox
    {
        public WebListBox(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebListBox(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }
    }
}
