using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebRadioButton : WebControl
    {
        public WebRadioButton(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebRadioButton(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }

        public virtual void SetSelected(bool value)
        {
            WaitForElementIsEnabled();
            if ((!IsSelected() && value))
            {
                HighlightElementAsAction();
                ClickElement();
                HighlightElementClear();
            }
        }

        public virtual bool GetSelected()
        {
            WaitForElementIsVisible();
            HighlightElementAsValidation();
            return IsSelected();
        }
    }
}
