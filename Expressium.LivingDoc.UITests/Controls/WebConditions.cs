using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Controls
{
    internal class WebConditions
    {
        internal static Func<IWebDriver, IWebElement> ElementExists(By locator)
        {
            return (driver) => driver.FindElement(locator);
        }

        internal static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
        {
            return delegate (IWebDriver driver)
            {
                try
                {
                    var element = driver.FindElement(locator);

                    if (element.Displayed)
                        return element;

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        internal static Func<IWebDriver, IWebElement> ElementIsVisible(IWebElement element)
        {
            return delegate (IWebDriver driver)
            {
                try
                {
                    if (element.Displayed)
                        return element;

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        internal static Func<IWebDriver, IWebElement> ElementIsEnabled(By locator)
        {
            return delegate (IWebDriver driver)
            {
                try
                {
                    var element = driver.FindElement(locator);

                    if (element.Enabled)
                        return element;

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        internal static Func<IWebDriver, IWebElement> ElementIsEnabled(IWebElement element)
        {
            return delegate (IWebDriver driver)
            {
                try
                {
                    if (element.Enabled)
                        return element;

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        internal static Func<IWebDriver, bool> ElementIsInvisible(By locator)
        {
            return delegate (IWebDriver driver)
            {
                try
                {
                    return !driver.FindElement(locator).Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            };
        }

        internal static Func<IWebDriver, bool> ElementIsInvisible(IWebElement element)
        {
            return delegate (IWebDriver driver)
            {
                try
                {
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            };
        }

        internal static Func<IWebDriver, bool> SelectOptionIsPopulated(SelectElement element, string value)
        {
            return (driver) =>
            {
                try
                {
                    if (element.Options.Count > 0)
                    {
                        if (element.Options.First(o => o.Text.Trim() == value) != null)
                            return true;
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            };
        }
    }
}
