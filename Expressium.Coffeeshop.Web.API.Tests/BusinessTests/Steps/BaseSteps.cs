using log4net;
using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Tests.BusinessTests.Steps
{
    public class BaseSteps
    {
        protected Configuration configuration;
        protected ILog logger;
        protected IWebDriver driver;
        protected Asserts Asserts;

        public BaseSteps(ContextController contextController)
        {
            configuration = contextController.Configuration;
            logger = contextController.Logger;
            Asserts = contextController.Asserts;
            driver = contextController.Driver;
        }
    }
}
