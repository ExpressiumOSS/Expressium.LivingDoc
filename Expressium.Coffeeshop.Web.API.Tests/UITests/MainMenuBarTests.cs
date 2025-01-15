using NUnit.Framework;
using Expressium.Coffeeshop.Web.API.Pages;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.UITests
{
    [Category("UITests")]
    [Description("Validate Navigation & Content of MainMenuBar")]
    public class MainMenuBarTests : BaseTest
    {
        private MainMenuBar mainMenuBar;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InitializeBrowserWithLogin();

            // TODO - Implement potential missing page navigations...

            mainMenuBar = new MainMenuBar(logger, driver);
        }

        [Test]
        public void Validate_Page_Title()
        {
            Asserts.EqualTo(mainMenuBar.GetTitle(), "Home", "Validating the MainMenuBar Title...");
        }
    }
}
