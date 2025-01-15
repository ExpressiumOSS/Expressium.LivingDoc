using OpenQA.Selenium;
using Expressium.Coffeeshop.Web.API.Controls;
using System;
using System.Collections.Generic;
using log4net;

namespace Expressium.Coffeeshop.Web.API.Pages
{
    public partial class ProductsPage : BasePage
    {
        public BaseTable Grid { get; private set; }

        private WebControl Heading => new WebControl(driver, By.XPath("//h1[text()]"));
        private WebText Notification => new WebText(driver, By.Id("notification"));

        public ProductsPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            Grid = new BaseTable(logger, driver, By.XPath("//table[@id='products']"));

            WaitForPageTitleEquals("Products");
            WaitForPageElementIsVisible(Heading);
        }

        public string GetNotification()
        {
            logger.InfoFormat("GetNotification()");
            return Notification.GetText();
        }

        #region Extensions

        #endregion
    }
}
