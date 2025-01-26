using Expressium.Coffeeshop.Web.API.Models;
using Expressium.Coffeeshop.Web.API.Pages;
using Expressium.Coffeeshop.Web.API.Tests.Factories;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Reqnroll;

namespace Expressium.Coffeeshop.Web.API.Tests.BusinessTests.Steps
{
    [Binding]
    public class LoginSteps : BaseSteps
    {
        public LoginSteps(ContextController contextController) : base(contextController)
        {
        }

        [Given(@"I have logged in with valid user credentials")]
        public void GivenIHaveLoggedInWithValidUserCredentials()
        {
            var loginPage = new LoginPage(logger, driver);
            loginPage.SetUsername(configuration.Username);
            loginPage.SetPassword(configuration.Password);
            loginPage.ClickLogin();
        }

        [Then(@"I should be redirected to the Home page")]
        public void ThenIShouldBeRedirectedToTheHomePage()
        {
            var homePage = new HomePage(logger, driver);
            Asserts.EqualTo(homePage.GetTitle(), "XHome", "Validate the HomePage title property...");
        }

        [Given(@"I have logged in with invalid user credentials")]
        public void GivenIHaveLoggedInWithInvalidUserCredentials()
        {
            var loginPage = new LoginPage(logger, driver);
            loginPage.SetUsername(configuration.Username);
            loginPage.SetPassword("");
            loginPage.ClickLogin();
        }

        [Then("I should have an error message on the Login page")]
        public void ThenIShouldHaveAnErrorMessageOnTheLoginPage()
        {
            throw new PendingStepException();
        }
    }
}
