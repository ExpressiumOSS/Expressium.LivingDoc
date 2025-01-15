using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Expressium.Coffeeshop.Web.API
{
    public static class WebElements
    {
        public static int ElementTimeOut { get; set; }

        public static bool Highlight { get; set; }
        public static int HighlightTimeOut { get; set; }
        public static int HighlightWidth { get; set; }
        public static string HighlightStyle { get; set; }
        public static string HighlightActionColor { get; set; }
        public static string HighlightValidationColor { get; set; }
        public static string HighlightFailureColor { get; set; }

        static WebElements()
        {
            ElementTimeOut = 10000;

            Highlight = false;
            HighlightTimeOut = 150;
            HighlightWidth = 2;
            HighlightStyle = "dashed";
            HighlightActionColor = "orange";
            HighlightValidationColor = "mediumseagreen";
            HighlightFailureColor = "darkred";
        }

        public static bool IsPresent(this By locator, IWebDriver driver)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsVisible(this By locator, IWebDriver driver)
        {
            return locator.WaitForElementExist(driver).Displayed;
        }

        public static bool IsVisible(this IWebElement element)
        {
            return element.Displayed;
        }

        public static bool IsEnabled(this By locator, IWebDriver driver)
        {
            return locator.WaitForElementExist(driver).Enabled;
        }

        public static bool IsEnabled(this IWebElement element)
        {
            return element.Enabled;
        }

        public static void HighlightAction(this IWebElement element, IWebDriver driver)
        {
            if (Highlight)
            {
                try
                {
                    var javaScriptExecuter = (IJavaScriptExecutor)driver;
                    var attribute = element.GetDomProperty("style");
                    attribute += $"; outline: {HighlightWidth}px {HighlightStyle} {HighlightActionColor}; outline-offset: -1px;";
                    javaScriptExecuter.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, attribute);
                }
                catch
                {
                }
            }
        }

        public static void HighlightValidation(this IWebElement element, IWebDriver driver)
        {
            if (Highlight)
            {
                try
                {
                    var javaScriptExecuter = (IJavaScriptExecutor)driver;
                    var attribute = element.GetDomProperty("style");
                    attribute += $"; outline: {HighlightWidth}px {HighlightStyle} {HighlightValidationColor}; outline-offset: -1px;";
                    javaScriptExecuter.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, attribute);
                }
                catch
                {
                }
            }
        }

        public static void HighlightFailure(this IWebElement element, IWebDriver driver)
        {
            if (Highlight)
            {
                try
                {
                    var javaScriptExecuter = (IJavaScriptExecutor)driver;
                    var attribute = element.GetDomProperty("style");
                    attribute += $"; outline: {HighlightWidth}px {HighlightStyle} {HighlightFailureColor}; outline-offset: -1px;";
                    javaScriptExecuter.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, attribute);
                }
                catch
                {
                }
            }
        }

        public static void HighlightClear(this IWebElement element, IWebDriver driver)
        {
            if (Highlight)
            {
                try
                {
                    Thread.Sleep(HighlightTimeOut);
                    var javaScriptExecutor = (IJavaScriptExecutor)driver;
                    javaScriptExecutor.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, "");
                }
                catch
                {
                }
            }
        }

        public static IWebElement WaitForElementExist(this By locator, IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                return wait.Until(WebConditions.ElementExists(locator));
            }
            catch
            {
                throw new ApplicationException($"Page element '{locator}' was expected to exist within {ElementTimeOut} milliseconds...");
            }
        }

        public static IWebElement WaitForElementIsVisible(this By locator, IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                return wait.Until(WebConditions.ElementIsVisible(locator));
            }
            catch
            {
                throw new ApplicationException($"Page element '{locator}' was expected to be visible within {ElementTimeOut} milliseconds...");
            }
        }

        public static void WaitForElementIsVisible(this IWebElement element, IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                wait.Until(WebConditions.ElementIsVisible(element));
            }
            catch
            {
                throw new ApplicationException($"Page element was expected to be visible within {ElementTimeOut} milliseconds...");
            }
        }

        public static IWebElement WaitForElementIsEnabled(this By locator, IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                return wait.Until(WebConditions.ElementIsEnabled(locator));
            }
            catch
            {
                throw new ApplicationException($"Page element '{locator}' was expected to be enabled within {ElementTimeOut} milliseconds...");
            }
        }

        public static void WaitForElementIsEnabled(this IWebElement element, IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                wait.Until(WebConditions.ElementIsEnabled(element));
            }
            catch
            {
                throw new ApplicationException($"Page element was expected to be enabled within {ElementTimeOut} milliseconds...");
            }
        }

        public static void WaitForElementIsInvisible(this By locator, IWebDriver driver, int initialTimeOut, int timeOut)
        {
            try
            {
                Thread.Sleep(initialTimeOut);
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeOut));
                wait.Until(WebConditions.ElementIsInvisible(locator));
            }
            catch
            {
                throw new ApplicationException($"Page element '{locator}' was expected to be invisible within {timeOut} milliseconds...");
            }
        }

        public static void WaitForElementIsInvisible(this IWebElement element, IWebDriver driver, int initialTimeOut, int timeOut)
        {
            try
            {
                Thread.Sleep(initialTimeOut);
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeOut));
                wait.Until(WebConditions.ElementIsInvisible(element));
            }
            catch
            {
                throw new ApplicationException($"Page element was expected to be invisible within {timeOut} milliseconds...");
            }
        }

        public static void WaitForSelectOptionIsPopulated(this SelectElement element, IWebDriver driver, string value)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                wait.Until(WebConditions.SelectOptionIsPopulated(element, value));
            }
            catch
            {
                throw new ApplicationException($"Page element option '{value}' was expected to be populated within {ElementTimeOut} milliseconds...");
            }
        }

        public static void ScrollIntoViewUp(this IWebElement element, IWebDriver driver)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView(false);", element);
        }

        public static void ScrollIntoViewDown(this IWebElement element, IWebDriver driver)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public static void MoveToElement(this IWebElement element, IWebDriver driver)
        {
            var actions = new Actions(driver);
            actions.MoveToElement(element).Build().Perform();
        }

        public static void ClickElementDoubleClick(this IWebElement element, IWebDriver driver)
        {
            var actions = new Actions(driver);
            actions.DoubleClick(element).Perform();
        }

        public static void ClickElementContextMenu(this IWebElement element, IWebDriver driver)
        {
            var actions = new Actions(driver);
            actions.ContextClick(element).Perform();
        }

        public static IWebElement GetElement(this By locator, IWebDriver driver)
        {
            return locator.WaitForElementExist(driver);
        }

        public static IWebElement GetVisibleElement(this By locator, IWebDriver driver)
        {
            return locator.WaitForElementIsVisible(driver);
        }

        public static IWebElement GetEnabledElement(this By locator, IWebDriver driver)
        {
            return locator.WaitForElementIsEnabled(driver);
        }

        public static IWebElement GetChildElement(this IWebElement element, IWebDriver driver, By childLocator)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                return wait.Until(_ => element.FindElement(childLocator));
            }
            catch
            {
                throw new ApplicationException($"Page child element '{childLocator}' was expected to be found within {ElementTimeOut} milliseconds...");
            }
        }

        public static List<IWebElement> GetChildElements(this IWebElement element, IWebDriver driver, By childLocator)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                return wait.Until(_ => element.FindElements(childLocator).ToList());
            }
            catch
            {
                throw new ApplicationException($"Page child element '{childLocator}' was expected to be found within {ElementTimeOut} milliseconds...");
            }
        }

        public static IWebElement GetChildElement(this By locator, IWebDriver driver, By childLocator)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                return wait.Until(_ => driver.FindElement(locator).FindElement(childLocator));
            }
            catch
            {
                throw new ApplicationException($"Page child element '{childLocator}' was expected to be found within {ElementTimeOut} milliseconds...");
            }
        }

        public static List<IWebElement> GetChildElements(this By locator, IWebDriver driver, By childLocator)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(ElementTimeOut));
                return wait.Until(_ => driver.FindElement(locator).FindElements(childLocator).ToList());
            }
            catch
            {
                throw new ApplicationException($"Page child element '{childLocator}' was expected to be found within {ElementTimeOut} milliseconds...");
            }
        }
    }
}
