using NUnit.Framework;
using Expressium.Coffeeshop.Web.API.Pages;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.UITests
{
    [Category("UITests")]
    [Description("Validate Navigation & Content of HomePage")]
    public class HomePageTests : BaseTest
    {
        private HomePage homePage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InitializeBrowserWithLogin();

            // TODO - Implement potential missing page navigations...

            var mainMenuBar = new MainMenuBar(logger, driver);
            mainMenuBar.ClickHome();

            homePage = new HomePage(logger, driver);
        }

        [Test]
        public void Validate_Page_Title()
        {
            Asserts.EqualTo(homePage.GetTitle(), "Home", "Validating the HomePage Title...");
        }
    }
}
