using NUnit.Framework;
using  Expressium.Coffeeshop.Web.API.Pages;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    public class BaseTest : BaseTestFixture
    {
        [OneTimeSetUp]
        protected void InitializeTestcase()
        {
            logger.InfoFormat("");
            logger.InfoFormat("// Initialize Testcase");
        }

        public void InitializeBrowserWithLogin()
        {
            InitializeBrowser();

            var loginPage = new LoginPage(logger, driver);
            loginPage.SetUsername(configuration.Username);
            loginPage.SetPassword(configuration.Password);
            loginPage.ClickLogin();
        }
    }
}
