using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebCheckBox : WebControl
    {
        public WebCheckBox(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebCheckBox(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }

        public virtual void SetChecked(bool value)
        {
            WaitForElementIsEnabled();
            if ((!IsSelected() && value) || (IsSelected() && !value))
            {
                HighlightElementAsAction();
                ClickElement();
                HighlightElementClear();
            }
        }

        public virtual bool GetChecked()
        {
            WaitForElementIsVisible();
            HighlightElementAsValidation();
            return IsSelected();
        }
    }
}
