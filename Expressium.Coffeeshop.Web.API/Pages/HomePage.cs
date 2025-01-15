using OpenQA.Selenium;
using Expressium.Coffeeshop.Web.API.Controls;
using System;
using System.Collections.Generic;
using log4net;

namespace Expressium.Coffeeshop.Web.API.Pages
{
    public partial class HomePage : BasePage
    {
        public MainMenuBar Menu { get; private set; }

        private WebControl Heading => new WebControl(driver, By.XPath("//h1[text()]"));

        public HomePage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            Menu = new MainMenuBar(logger, driver);

            WaitForPageTitleEquals("Home");
            WaitForPageElementIsVisible(Heading);
        }

        #region Extensions

        #endregion
    }
}
