using System;
using System.Collections.Generic;

namespace Expressium.Coffeeshop.Web.API.Models
{
    public class RegistrationPageModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Vehicle { get; set; }
        public bool Male { get; set; }
        public bool Female { get; set; }
        public bool IAgreeToTheTermsOfUse { get; set; }

        #region Extensions

        #endregion
    }
}
