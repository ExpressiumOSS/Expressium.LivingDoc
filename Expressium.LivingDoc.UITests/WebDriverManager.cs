using OpenQA.Selenium;

namespace Expressium.LivingDoc.UITests
{
    public class WebDriverManager
    {
        private Configuration configuration;
        private IWebDriver driver;

        public WebDriverManager(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public IWebDriver Driver
        {
            get
            {
                if (driver == null)
                {
                    driver = WebDriverFactory.Initialize(configuration);
                }

                return driver;
            }
        }

        public bool IsInitialized()
        {
            return driver != null;
        }

        public void SaveScreenshot(string filePath)
        {
            if (driver != null)
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(filePath);
            }
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
