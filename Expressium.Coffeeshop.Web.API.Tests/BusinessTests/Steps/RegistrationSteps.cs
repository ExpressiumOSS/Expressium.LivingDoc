using Expressium.Coffeeshop.Web.API.Models;
using Expressium.Coffeeshop.Web.API.Pages;
using Expressium.Coffeeshop.Web.API.Tests.Factories;
using Reqnroll;

namespace Expressium.Coffeeshop.Web.API.Tests.BusinessTests.Steps
{
    [Binding]
    public class RegistrationSteps : BaseSteps
    {
        public RegistrationSteps(ContextController contextController) : base(contextController)
        {
        }

        [When("I complete and submit the Registration formular")]
        public void WhenICompleteAndSubmitTheRegistrationFormular()
        {
            var loginPage = new LoginPage(logger, driver);
            loginPage.ClickCreateAnAccount();

            var registrationPageModel = RegistrationPageModelFactory.Default();

            var registrationPage = new RegistrationPage(logger, driver);
            registrationPage.FillForm(registrationPageModel);
            registrationPage.ClickSubmit();
        }

        [When("I complete and cancel the Registration formular")]
        public void WhenICompleteAndCancelTheRegistrationFormular()
        {
            var loginPage = new LoginPage(logger, driver);
            loginPage.ClickCreateAnAccount();

            var registrationPageModel = RegistrationPageModelFactory.Default();

            var registrationPage = new RegistrationPage(logger, driver);
            registrationPage.FillForm(registrationPageModel);
            registrationPage.ClickCancel();
        }
    }
}
