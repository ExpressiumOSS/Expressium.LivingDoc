using log4net;
using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Tests.BusinessTests.Steps
{
    public class BaseSteps
    {
        protected Configuration configuration;
        protected ILog logger;
        protected LazyWebDriver lazyWebDriver;
        protected Asserts Asserts;

        protected IWebDriver driver => lazyWebDriver.Driver;

        public BaseSteps(ContextController contextController)
        {
            configuration = contextController.Configuration;
            logger = contextController.Logger;
            lazyWebDriver = contextController.Driver;
            Asserts = contextController.Asserts;
        }
    }
}
