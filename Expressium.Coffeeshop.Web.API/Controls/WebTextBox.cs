using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebTextBox : WebControl
    {
        public WebTextBox(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebTextBox(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }

        public virtual void SetText(string value)
        {
            if (value == null)
                return;

            WaitForElementIsEnabled();
            HighlightElementAsAction();
            ClearElementValue();
            SetElementValue(value);
            HighlightElementClear();
        }

        public virtual string GetText()
        {
            WaitForElementIsVisible();
            HighlightElementAsValidation();
            return GetElementValue();
        }

        protected virtual void ClearElementValue()
        {
            GetElement().Clear();
        }

        protected virtual void SetElementValue(string value)
        {
            GetElement().SendKeys(value);
        }

        protected virtual string GetElementValue()
        {
            return GetElement().GetDomProperty("value").Trim();
        }
    }
}
