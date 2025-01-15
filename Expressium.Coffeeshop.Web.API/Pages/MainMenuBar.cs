using OpenQA.Selenium;
using Expressium.Coffeeshop.Web.API.Controls;
using System;
using System.Collections.Generic;
using log4net;

namespace Expressium.Coffeeshop.Web.API.Pages
{
    public partial class MainMenuBar : BasePage
    {
        private WebLink Home => new WebLink(driver, By.XPath("//nav[@id='mainmenu']//a[text()='Home']"));
        private WebLink Products => new WebLink(driver, By.XPath("//nav[@id='mainmenu']//a[text()='Products']"));
        private WebLink ContactUs => new WebLink(driver, By.XPath("//nav[@id='mainmenu']//a[text()='Contact Us']"));
        private WebLink Logout => new WebLink(driver, By.XPath("//nav[@id='mainmenu']//a[text()='Logout']"));

        public MainMenuBar(ILog logger, IWebDriver driver) : base(logger, driver)
        {
        }

        public void ClickHome()
        {
            logger.InfoFormat("ClickHome()");
            Home.Click();
        }

        public void ClickProducts()
        {
            logger.InfoFormat("ClickProducts()");
            Products.Click();
        }

        public void ClickContactUs()
        {
            logger.InfoFormat("ClickContactUs()");
            ContactUs.Click();
        }

        public void ClickLogout()
        {
            logger.InfoFormat("ClickLogout()");
            Logout.Click();
        }

        #region Extensions

        #endregion
    }
}
