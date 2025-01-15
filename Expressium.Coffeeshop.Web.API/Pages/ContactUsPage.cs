using OpenQA.Selenium;
using Expressium.Coffeeshop.Web.API.Controls;
using System;
using System.Collections.Generic;
using log4net;

namespace Expressium.Coffeeshop.Web.API.Pages
{
    public partial class ContactUsPage : BasePage
    {
        private WebControl Heading => new WebControl(driver, By.XPath("//h1[text()]"));
        private WebTextBox Name => new WebTextBox(driver, By.Name("your-name"));
        private WebTextBox Email => new WebTextBox(driver, By.Name("your-email"));
        private WebTextBox Subject => new WebTextBox(driver, By.Name("your-subject"));
        private WebTextBox Message => new WebTextBox(driver, By.Name("your-message"));
        private WebButton Send => new WebButton(driver, By.XPath("//input[@type='button' and @value='Send']"));
        private WebText Notification => new WebText(driver, By.XPath("//h4[@id='notification']"));

        public ContactUsPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            WaitForPageTitleEquals("Contact Us");
            WaitForPageElementIsVisible(Heading);
        }

        public void SetName(string value)
        {
            logger.InfoFormat("SetName({0})", value);
            Name.SetText(value);
        }

        public string GetName()
        {
            logger.InfoFormat("GetName()");
            return Name.GetText();
        }

        public void SetEmail(string value)
        {
            logger.InfoFormat("SetEmail({0})", value);
            Email.SetText(value);
        }

        public string GetEmail()
        {
            logger.InfoFormat("GetEmail()");
            return Email.GetText();
        }

        public void SetSubject(string value)
        {
            logger.InfoFormat("SetSubject({0})", value);
            Subject.SetText(value);
        }

        public string GetSubject()
        {
            logger.InfoFormat("GetSubject()");
            return Subject.GetText();
        }

        public void SetMessage(string value)
        {
            logger.InfoFormat("SetMessage({0})", value);
            Message.SetText(value);
        }

        public string GetMessage()
        {
            logger.InfoFormat("GetMessage()");
            return Message.GetText();
        }

        public void ClickSend()
        {
            logger.InfoFormat("ClickSend()");
            Send.Click();
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
