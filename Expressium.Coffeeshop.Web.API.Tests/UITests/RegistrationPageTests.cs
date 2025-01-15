using NUnit.Framework;
using Expressium.Coffeeshop.Web.API.Models;
using Expressium.Coffeeshop.Web.API.Pages;
using Expressium.Coffeeshop.Web.API.Tests.Factories;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.UITests
{
    [Category("UITests")]
    [Description("Validate Navigation & Content of RegistrationPage")]
    public class RegistrationPageTests : BaseTest
    {
        private RegistrationPage registrationPage;
        private RegistrationPageModel registrationPageModel = RegistrationPageModelFactory.Default();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InitializeBrowser();

            // TODO - Implement potential missing page navigations...

            var loginPage = new LoginPage(logger, driver);
            loginPage.ClickCreateAnAccount();

            registrationPage = new RegistrationPage(logger, driver);
            registrationPage.FillForm(registrationPageModel);
        }

        [Test]
        public void Validate_Page_Title()
        {
            Asserts.EqualTo(registrationPage.GetTitle(), "Registration", "Validating the RegistrationPage Title...");
        }

        [Test]
        public void Validate_Page_Property_FirstName()
        {
            Asserts.EqualTo(registrationPage.GetFirstName(), registrationPageModel.FirstName, "Validating the RegistrationPage property FirstName...");
        }

        [Test]
        public void Validate_Page_Property_LastName()
        {
            Asserts.EqualTo(registrationPage.GetLastName(), registrationPageModel.LastName, "Validating the RegistrationPage property LastName...");
        }

        [Test]
        public void Validate_Page_Property_Country()
        {
            Asserts.EqualTo(registrationPage.GetCountry(), registrationPageModel.Country, "Validating the RegistrationPage property Country...");
        }

        [Test]
        public void Validate_Page_Property_Vehicle()
        {
            Asserts.EqualTo(registrationPage.GetVehicle(), registrationPageModel.Vehicle, "Validating the RegistrationPage property Vehicle...");
        }

        [Test]
        public void Validate_Page_Property_Male()
        {
            Asserts.EqualTo(registrationPage.GetMale(), registrationPageModel.Male, "Validating the RegistrationPage property Male...");
        }

        [Test]
        public void Validate_Page_Property_Female()
        {
            Asserts.EqualTo(registrationPage.GetFemale(), registrationPageModel.Female, "Validating the RegistrationPage property Female...");
        }

        [Test]
        public void Validate_Page_Property_IAgreeToTheTermsOfUse()
        {
            Asserts.EqualTo(registrationPage.GetIAgreeToTheTermsOfUse(), registrationPageModel.IAgreeToTheTermsOfUse, "Validating the RegistrationPage property IAgreeToTheTermsOfUse...");
        }
    }
}
