using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    public class WebDriverFactory
    {
        public enum BrowserTypes
        {
            Chrome,
            Firefox,
            Edge
        }

        public static IWebDriver Initialize(string browserType, string url = null, bool maximized = true, bool headless = false, bool windowSize = false, int windowWidth = 1920, int windowHeight = 1080)
        {
            if (browserType == BrowserTypes.Chrome.ToString())
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--disable-notifications");
                options.AddExcludedArgument("enable-automation");
                options.AddUserProfilePreference("credentials_enable_service", false);

                if (maximized)
                    options.AddArguments("start-maximized");

                if (headless)
                {
                    options.AddArgument($"--headless");
                    options.AddArgument($"--window-size={windowWidth},{windowHeight}");
                }
                else
                {
                    options.AddArgument("--disable-search-engine-choice-screen");

                    if (windowSize)
                    {
                        options.AddArgument($"--window-size={windowWidth},{windowHeight}");
                        options.AddArgument($"--window-position={10},{10}");
                    }
                }

                var driver = new ChromeDriver(service, options);

                if (url != null)
                    driver.Navigate().GoToUrl(url);

                return driver;
            }
            else if (browserType == BrowserTypes.Firefox.ToString())
            {
                var service = FirefoxDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                service.Host = "::1";

                var options = new FirefoxOptions();

                if (headless)
                {
                    options.AddArguments($"--headless");
                    options.AddArgument($"--width={windowWidth}");
                    options.AddArgument($"--height={windowHeight}");
                }
                else
                {
                    if (windowSize)
                    {
                        options.AddArgument($"--width={windowWidth}");
                        options.AddArgument($"--height={windowHeight}");
                    }
                }

                var driver = new FirefoxDriver(service, options);

                if (url != null)
                    driver.Navigate().GoToUrl(url);

                return driver;
            }
            else if (browserType == BrowserTypes.Edge.ToString())
            {
                var service = EdgeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new EdgeOptions();
                options.AddArgument("--disable-notifications");
                options.AddExcludedArgument("enable-automation");
                options.AddUserProfilePreference("credentials_enable_service", false);

                if (maximized)
                    options.AddArguments("start-maximized");

                if (headless)
                {
                    options.AddArgument($"--headless");
                    options.AddArgument($"--window-size={windowWidth},{windowHeight}");
                }
                else
                {
                    if (windowSize)
                    {
                        options.AddArgument($"--window-size={windowWidth},{windowHeight}");
                        options.AddArgument($"--window-position={10},{10}");
                    }
                }

                var driver = new EdgeDriver(service, options);

                if (url != null)
                    driver.Navigate().GoToUrl(url);

                return driver;
            }
            else
            {
                throw new ArgumentException($"Specified browser type '{browserType}' is unknown...");
            }
        }
    }
}
