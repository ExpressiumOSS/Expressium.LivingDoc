using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebControl
    {
        protected IWebDriver driver;

        protected By locator;
        protected By childLocator;

        protected IWebElement element;

        protected bool cashed = false;

        public WebControl(IWebDriver driver, By locator)
        {
            this.driver = driver;
            this.locator = locator;
        }

        public WebControl(IWebDriver driver, By locator, By childLocator)
        {
            this.driver = driver;
            this.locator = locator;
            this.childLocator = childLocator;
        }

        public IWebElement GetElement()
        {
            if (element != null)
                return element;

            if (childLocator != null)
            {
                if (cashed)
                {
                    element = locator.GetChildElement(driver, childLocator);
                    return element;
                }

                return locator.GetChildElement(driver, childLocator);
            }

            if (cashed)
            {
                element = locator.GetElement(driver);
                return element;
            }

            return locator.GetElement(driver);
        }

        public virtual string GetAttribute(string value)
        {
            return GetElement().GetDomProperty(value);
        }

        public virtual bool IsVisible()
        {
            return GetElement().Displayed;
        }

        public virtual bool IsEnabled()
        {
            return GetElement().Enabled;
        }

        public virtual bool IsSelected()
        {
            return GetElement().Selected;
        }

        public void WaitForElementIsVisible()
        {
            GetElement().WaitForElementIsVisible(driver);
        }

        public void WaitForElementIsEnabled()
        {
            GetElement().WaitForElementIsEnabled(driver);
        }

        public void WaitForElementIsInvisible(int initialTimeOut, int timeOut)
        {
            GetElement().WaitForElementIsInvisible(driver, initialTimeOut, timeOut);
        }

        public void HighlightElementAsAction()
        {
            if (WebElements.Highlight)
                GetElement().HighlightAction(driver);
        }

        public void HighlightElementAsValidation()
        {
            if (WebElements.Highlight)
                GetElement().HighlightValidation(driver);
        }

        public void HighlightElementAsFailure()
        {
            if (WebElements.Highlight)
                GetElement().HighlightFailure(driver);
        }

        public void HighlightElementClear()
        {
            if (WebElements.Highlight)
                GetElement().HighlightClear(driver);
        }

        public virtual void ClickElement()
        {
            GetElement().Click();
        }

        public virtual void ClickElementContextMenu()
        {
            GetElement().ClickElementContextMenu(driver);
        }

        public virtual void ClickElementDoubleClick()
        {
            GetElement().ClickElementDoubleClick(driver);
        }

        public void ScrollElementIntoViewUp()
        {
            GetElement().ScrollIntoViewUp(driver);
        }

        public void ScrollElementIntoViewDown()
        {
            GetElement().ScrollIntoViewDown(driver);
        }

        public void MoveToElement()
        {
            GetElement().MoveToElement(driver);
        }
    }
}
