using OpenQA.Selenium;
using Expressium.Coffeeshop.Web.API.Controls;
using System;
using System.Collections.Generic;
using log4net;

namespace Expressium.Coffeeshop.Web.API.Pages
{
    public partial class LoginPage : BasePage
    {
        private WebTextBox Username => new WebTextBox(driver, By.Name("username"));
        private WebTextBox Password => new WebTextBox(driver, By.Name("password"));
        private WebButton Login => new WebButton(driver, By.Name("submit"));
        private WebLink CreateAnAccount => new WebLink(driver, By.XPath("//a[normalize-space()='Create an account']"));

        public LoginPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            WaitForPageTitleEquals("Login");
        }

        public void SetUsername(string value)
        {
            logger.InfoFormat("SetUsername({0})", value);
            Username.SetText(value);
        }

        public string GetUsername()
        {
            logger.InfoFormat("GetUsername()");
            return Username.GetText();
        }

        public void SetPassword(string value)
        {
            logger.InfoFormat("SetPassword({0})", value);
            Password.SetText(value);
        }

        public string GetPassword()
        {
            logger.InfoFormat("GetPassword()");
            return Password.GetText();
        }

        public void ClickLogin()
        {
            logger.InfoFormat("ClickLogin()");
            Login.Click();
        }

        public void ClickCreateAnAccount()
        {
            logger.InfoFormat("ClickCreateAnAccount()");
            CreateAnAccount.Click();
        }

        #region Extensions

        #endregion
    }
}
