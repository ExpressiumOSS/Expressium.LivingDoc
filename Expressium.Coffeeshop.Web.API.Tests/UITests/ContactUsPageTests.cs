using NUnit.Framework;
using Expressium.Coffeeshop.Web.API.Pages;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.UITests
{
    [Category("UITests")]
    [Description("Validate Navigation & Content of ContactUsPage")]
    public class ContactUsPageTests : BaseTest
    {
        private ContactUsPage contactUsPage;

        private string name = "Peter Parker";
        private string email = "peter.parker@expressium.dev";
        private string subject = "Detailed Information";
        private string message = "Please send some detailed product information...";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InitializeBrowserWithLogin();

            // TODO - Implement potential missing page navigations...

            var mainMenuBar = new MainMenuBar(logger, driver);
            mainMenuBar.ClickContactUs();

            contactUsPage = new ContactUsPage(logger, driver);
            contactUsPage.SetName(name);
            contactUsPage.SetEmail(email);
            contactUsPage.SetSubject(subject);
            contactUsPage.SetMessage(message);
        }

        [Test]
        public void Validate_Page_Title()
        {
            Asserts.EqualTo(contactUsPage.GetTitle(), "Contact Us", "Validating the ContactUsPage Title...");
        }

        [Test]
        public void Validate_Page_Property_Name()
        {
            Asserts.EqualTo(contactUsPage.GetName(), name, "Validating the ContactUsPage property Name...");
        }

        [Test]
        public void Validate_Page_Property_Email()
        {
            Asserts.EqualTo(contactUsPage.GetEmail(), email, "Validating the ContactUsPage property Email...");
        }

        [Test]
        public void Validate_Page_Property_Subject()
        {
            Asserts.EqualTo(contactUsPage.GetSubject(), subject, "Validating the ContactUsPage property Subject...");
        }

        [Test]
        public void Validate_Page_Property_Message()
        {
            Asserts.EqualTo(contactUsPage.GetMessage(), message, "Validating the ContactUsPage property Message...");
        }
    }
}
