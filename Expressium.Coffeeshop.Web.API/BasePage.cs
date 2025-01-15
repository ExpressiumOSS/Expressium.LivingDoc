using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using Expressium.Coffeeshop.Web.API.Controls;

namespace Expressium.Coffeeshop.Web.API
{
    public class BasePage
    {
        protected ILog logger;
        protected IWebDriver driver;

        public static int PageTimeOut { get; set; }

        public static By PageSpinnerLocator { get; set; }
        public static int PageSpinnerInitialTimeOut { get; set; }
        public static int PageSpinnerTimeOut { get; set; }

        static BasePage()
        {
            PageTimeOut = 10000;

            PageSpinnerLocator = null;
            PageSpinnerInitialTimeOut = 150;
            PageSpinnerTimeOut = 10000;
        }

        public BasePage(ILog logger, IWebDriver driver)
        {
            this.logger = logger;
            this.driver = driver;

            WaitForPageDocumentReadyStateEqualsComplete();
            WaitForPageSpinnerIsCompleted();
        }

        public string GetTitle()
        {
            logger.InfoFormat("GetTitle()");
            return driver.Title;
        }

        public string GetURL()
        {
            logger.InfoFormat("GetURL()");
            return new Uri(driver.Url).LocalPath;
        }

        protected void WaitForPageDocumentReadyStateEqualsComplete()
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(PageTimeOut));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch
            {
                throw new ApplicationException($"Page document ready state was expected to be completed within {PageTimeOut} milliseconds...");
            }
        }

        protected void WaitForPageSpinnerIsCompleted()
        {
            if (PageSpinnerLocator != null)
            {
                PageSpinnerLocator.WaitForElementIsInvisible(driver, PageSpinnerInitialTimeOut, PageSpinnerTimeOut);
            }
        }

        protected void WaitForPageTitleEquals(string title)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(PageTimeOut));
                wait.Until(d => d.Title.Equals(title));
            }
            catch
            {
                throw new ApplicationException($"Page title was expected to be equal to '{title}' but was '{driver.Title}' within {PageTimeOut} milliseconds...");
            }
        }

        protected void WaitForPageTitleContains(string title)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(PageTimeOut));
                wait.Until(d => d.Title.Contains(title));
            }
            catch
            {
                throw new ApplicationException($"Page title was expected to contain '{title}' but was '{driver.Title}' within {PageTimeOut} milliseconds...");
            }
        }

        protected void WaitForPageUrlEquals(string url)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(PageTimeOut));
                wait.Until(d => new Uri(d.Url).LocalPath.Equals(url));
            }
            catch
            {
                throw new ApplicationException($"Page URL was expected to be equal to '{url}' but was '{new Uri(driver.Url).LocalPath}' within {PageTimeOut} milliseconds...");
            }
        }

        protected void WaitForPageUrlContains(string url)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(PageTimeOut));
                wait.Until(d => new Uri(d.Url).LocalPath.Contains(url));
            }
            catch
            {
                throw new ApplicationException($"Page URL was expected to contain '{url}' but was '{new Uri(driver.Url).LocalPath}' within {PageTimeOut} milliseconds...");
            }
        }

        protected void WaitForPageElementIsVisible(WebControl control)
        {
            control.WaitForElementIsVisible();
        }

        protected void WaitForPageElementIsEnabled(WebControl control)
        {
            control.WaitForElementIsEnabled();
        }

        public void NavigateBack()
        {
            logger.InfoFormat("NavigateBack()");
            driver.Navigate().Back();
        }

        public void NavigateForward()
        {
            logger.InfoFormat("NavigateForward()");
            driver.Navigate().Forward();
        }

        public void NavigateRefresh()
        {
            logger.InfoFormat("NavigateRefresh()");
            driver.Navigate().Refresh();
        }

        public void ScrollToTop(IWebDriver driver)
        {
            logger.InfoFormat("ScrollToTop()");
            var javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("window.scrollTo(0, 0);");
        }

        public void ScrollToBottom(IWebDriver driver)
        {
            logger.InfoFormat("ScrollToBottom()");
            var javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }

        public void SwitchToAlertAccept()
        {
            logger.InfoFormat("SwitchToAlertAccept()");
            driver.SwitchTo().Alert().Accept();
        }

        public void SwitchToAlertDismiss()
        {
            logger.InfoFormat("SwitchToAlertDismiss()");
            driver.SwitchTo().Alert().Dismiss();
        }

        protected int GetNumberOfBrowserTabs()
        {
            var value = driver.WindowHandles.Count;
            logger.InfoFormat("GetNumberOfBrowserTabs() [%d]", value);
            return value;
        }

        protected void WaitForNewBrowserTabIsOpened(int numberOfCurrentTabs)
        {
            var found = false;

            int retries = 10;
            int delay = 500;

            for (int i = 0; i < retries; i++)
            {
                if (driver.WindowHandles.Count > numberOfCurrentTabs)
                {
                    found = true;
                    break;
                }
                else
                {
                    Thread.Sleep(delay);
                }
            }

            if (!found)
            {
                throw new ApplicationException($"WaitForNewBrowserTabIsOpened() has Failed - Expected {numberOfCurrentTabs + 1}, Found {driver.WindowHandles.Count}");
            }
        }

        protected void SelectBrowserTab(int index)
        {
            driver.SwitchTo().Window(driver.WindowHandles[index]);
        }
    }
}
