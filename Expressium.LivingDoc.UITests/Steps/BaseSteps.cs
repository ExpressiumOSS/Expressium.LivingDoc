using log4net;
using OpenQA.Selenium;

namespace Expressium.LivingDoc.UITests.Steps
{
    public class BaseSteps
    {
        protected Configuration configuration;
        protected ILog logger;
        protected WebDriverManager driverManager;
        protected Asserts Asserts;

        protected IWebDriver driver => driverManager.Driver;

        public BaseSteps(BaseContext baseContext)
        {
            configuration = baseContext.Configuration;
            logger = baseContext.Logger;
            driverManager = baseContext.DriverManager;
            Asserts = baseContext.Asserts;
        }
    }
}
