using NUnit.Framework;
using Expressium.Coffeeshop.Web.API.Pages;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.UITests
{
    [Category("UITests")]
    [Description("Validate Navigation & Content of LoginPage")]
    public class LoginPageTests : BaseTest
    {
        private LoginPage loginPage;

        private string username = "as4rHs";
        private string password = "j9aaUg";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InitializeBrowser();

            // TODO - Implement potential missing page navigations...

            loginPage = new LoginPage(logger, driver);
            loginPage.SetUsername(username);
            loginPage.SetPassword(password);
        }

        [Test]
        public void Validate_Page_Title()
        {
            Asserts.EqualTo(loginPage.GetTitle(), "Login", "Validating the LoginPage Title...");
        }

        [Test]
        public void Validate_Page_Property_Username()
        {
            Asserts.EqualTo(loginPage.GetUsername(), username, "Validating the LoginPage property Username...");
        }

        [Test]
        public void Validate_Page_Property_Password()
        {
            Asserts.EqualTo(loginPage.GetPassword(), password, "Validating the LoginPage property Password...");
        }
    }
}
