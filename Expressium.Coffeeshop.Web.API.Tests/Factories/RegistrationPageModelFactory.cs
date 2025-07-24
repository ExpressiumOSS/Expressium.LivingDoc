using Expressium.Coffeeshop.Web.API.Models;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.Factories
{
    public class RegistrationPageModelFactory
    {
        public static RegistrationPageModel Default()
        {
            var model = new RegistrationPageModel();

            model.FirstName = "John";
            model.LastName = "Doe";
            model.Country = "Denmark";
            model.Vehicle = "Volvo";
            model.Male = false;
            model.Female = true;
            model.IAgreeToTheTermsOfUse = true;

            return model;
        }
    }
}
