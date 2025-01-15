using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebComboBox : WebControl
    {
        public WebComboBox(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebComboBox(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }

        public virtual void SelectByText(string value)
        {
            if (value == null)
                return;

            WaitForElementIsEnabled();
            HighlightElementAsAction();
            SelectElementByText(value);
            HighlightElementClear();
        }

        public virtual void SelectByIndex(int value)
        {
            WaitForElementIsEnabled();
            HighlightElementAsAction();
            SelectElementByIndex(value);
            HighlightElementClear();
        }

        public virtual void SelectByValue(string value)
        {
            if (value == null)
                return;

            WaitForElementIsEnabled();
            HighlightElementAsAction();
            SelectElementByValue(value);
            HighlightElementClear();
        }

        public virtual string GetSelectedText()
        {
            WaitForElementIsVisible();
            HighlightElementAsValidation();
            return GetSelectedElementOptionText();
        }

        protected void SelectElementByText(string value)
        {
            var selectElement = new SelectElement(GetElement());
            selectElement.WaitForSelectOptionIsPopulated(driver, value);
            selectElement.SelectByText(value);
        }

        protected void SelectElementByIndex(int value)
        {
            var selectElement = new SelectElement(GetElement());
            selectElement.SelectByIndex(value);
        }

        protected void SelectElementByValue(string value)
        {
            var selectElement = new SelectElement(GetElement());
            selectElement.SelectByValue(value);
        }

        protected string GetSelectedElementOptionText()
        {
            var selectElement = new SelectElement(GetElement());
            if (selectElement.SelectedOption != null)
                return selectElement.SelectedOption.Text.Trim();

            return null;
        }
    }
}
