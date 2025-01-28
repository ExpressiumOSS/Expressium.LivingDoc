using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    public class LazyWebDriver
    {
        private Configuration configuration;
        private IWebDriver driver;

        public LazyWebDriver(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public IWebDriver Driver
        {
            get
            {
                if (driver == null)
                {
                    driver = WebDriverFactory.Initialize(configuration.BrowserType, configuration.Url,
                        configuration.Maximize, configuration.Headless, configuration.WindowSize,
                        configuration.WindowWidth, configuration.WindowHeight);
                }

                return driver;
            }
        }

        public bool IsInitialized()
        {
            return driver != null;
        }

        public void Quit()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}


//public class BaseSteps
//{
//    protected Configuration configuration;
//    protected ILog logger;
//    protected LazyWebDriver lazyWebDriver;
//    protected Asserts asserts;

//    protected IWebDriver driver => lazyWebDriver.Driver;

//    public BaseSteps(ContextController contextController)
//    {
//        configuration = contextController.Configuration;
//        logger = contextController.Logger;
//        lazyWebDriver = contextController.Driver;
//        asserts = contextController.asserts;
//    }
//}

