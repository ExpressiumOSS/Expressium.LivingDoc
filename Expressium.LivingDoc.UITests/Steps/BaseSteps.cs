using Expressium.LivingDoc.UITests.Utilities;
using log4net;
using OpenQA.Selenium;

namespace Expressium.LivingDoc.UITests.Steps
{
    public class BaseSteps
    {
        protected Configuration configuration;
        protected ILog logger;
        protected WebDriverController driverController;
        protected Asserts Asserts;

        protected IWebDriver driver => driverController.Driver;

        public BaseSteps(BaseContext baseContext)
        {
            configuration = baseContext.Configuration;
            logger = baseContext.Logger;
            driverController = baseContext.Controller;
            Asserts = baseContext.Asserts;
        }
    }
}
