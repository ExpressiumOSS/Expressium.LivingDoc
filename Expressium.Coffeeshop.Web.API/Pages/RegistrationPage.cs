using OpenQA.Selenium;
using Expressium.Coffeeshop.Web.API.Controls;
using Expressium.Coffeeshop.Web.API.Models;
using System;
using System.Collections.Generic;
using log4net;

namespace Expressium.Coffeeshop.Web.API.Pages
{
    public partial class RegistrationPage : BasePage
    {
        private WebTextBox FirstName => new WebTextBox(driver, By.Name("firstname"));
        private WebTextBox LastName => new WebTextBox(driver, By.Name("lastname"));
        private WebComboBox Country => new WebComboBox(driver, By.Name("country"));
        private WebListBox Vehicle => new WebListBox(driver, By.Name("vehicle"));
        private WebRadioButton Male => new WebRadioButton(driver, By.Id("gender_0"));
        private WebRadioButton Female => new WebRadioButton(driver, By.Id("gender_1"));
        private WebCheckBox IAgreeToTheTermsOfUse => new WebCheckBox(driver, By.Name("agreement"));
        private WebButton Submit => new WebButton(driver, By.Name("submit"));
        private WebButton Reset => new WebButton(driver, By.Name("reset"));

        public RegistrationPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
            WaitForPageTitleEquals("Registration");
        }

        public void SetFirstName(string value)
        {
            logger.InfoFormat("SetFirstName({0})", value);
            FirstName.SetText(value);
        }

        public string GetFirstName()
        {
            logger.InfoFormat("GetFirstName()");
            return FirstName.GetText();
        }

        public void SetLastName(string value)
        {
            logger.InfoFormat("SetLastName({0})", value);
            LastName.SetText(value);
        }

        public string GetLastName()
        {
            logger.InfoFormat("GetLastName()");
            return LastName.GetText();
        }

        public void SetCountry(string value)
        {
            logger.InfoFormat("SetCountry({0})", value);
            Country.SelectByText(value);
        }

        public string GetCountry()
        {
            logger.InfoFormat("GetCountry()");
            return Country.GetSelectedText();
        }

        public void SetVehicle(string value)
        {
            logger.InfoFormat("SetVehicle({0})", value);
            Vehicle.SelectByText(value);
        }

        public string GetVehicle()
        {
            logger.InfoFormat("GetVehicle()");
            return Vehicle.GetSelectedText();
        }

        public void SetMale(bool value)
        {
            logger.InfoFormat("SetMale({0})", value);
            Male.SetSelected(value);
        }

        public bool GetMale()
        {
            logger.InfoFormat("GetMale()");
            return Male.GetSelected();
        }

        public void SetFemale(bool value)
        {
            logger.InfoFormat("SetFemale({0})", value);
            Female.SetSelected(value);
        }

        public bool GetFemale()
        {
            logger.InfoFormat("GetFemale()");
            return Female.GetSelected();
        }

        public void SetIAgreeToTheTermsOfUse(bool value)
        {
            logger.InfoFormat("SetIAgreeToTheTermsOfUse({0})", value);
            IAgreeToTheTermsOfUse.SetChecked(value);
        }

        public bool GetIAgreeToTheTermsOfUse()
        {
            logger.InfoFormat("GetIAgreeToTheTermsOfUse()");
            return IAgreeToTheTermsOfUse.GetChecked();
        }

        public void ClickSubmit()
        {
            logger.InfoFormat("ClickSubmit()");
            Submit.Click();
        }

        public void ClickReset()
        {
            logger.InfoFormat("ClickReset()");
            Reset.Click();
        }

        public void FillForm(RegistrationPageModel model)
        {
            SetFirstName(model.FirstName);
            SetLastName(model.LastName);
            SetCountry(model.Country);
            SetVehicle(model.Vehicle);
            SetMale(model.Male);
            SetFemale(model.Female);
            SetIAgreeToTheTermsOfUse(model.IAgreeToTheTermsOfUse);
        }

        #region Extensions

        #endregion
    }
}
