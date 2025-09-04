using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;

namespace Expressium.LivingDoc.UITests
{
    public class WebDriverFactory
    {
        public enum BrowserTypes
        {
            Chrome,
            Firefox,
            Edge
        }

        public static IWebDriver Initialize(Configuration configuration)
        {
            if (configuration.BrowserType == BrowserTypes.Chrome.ToString())
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--disable-notifications");
                options.AddExcludedArgument("enable-automation");
                options.AddUserProfilePreference("credentials_enable_service", false);
                options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

                if (configuration.Maximize)
                    options.AddArguments("start-maximized");

                if (configuration.Headless)
                {
                    options.AddArgument($"--headless");
                    options.AddArgument($"--window-size={configuration.WindowWidth},{configuration.WindowHeight}");
                }
                else
                {
                    options.AddArgument("--disable-search-engine-choice-screen");

                    if (configuration.WindowSize)
                    {
                        options.AddArgument($"--window-size={configuration.WindowWidth},{configuration.WindowHeight}");
                        options.AddArgument($"--window-position={10},{10}");
                    }
                }

                var driver = new ChromeDriver(service, options);

                if (configuration.Url != null)
                    driver.Navigate().GoToUrl(configuration.Url);

                return driver;
            }
            else if (configuration.BrowserType == BrowserTypes.Firefox.ToString())
            {
                var service = FirefoxDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                service.Host = "::1";

                var options = new FirefoxOptions();

                if (configuration.Headless)
                {
                    options.AddArguments($"--headless");
                    options.AddArgument($"--width={configuration.WindowWidth}");
                    options.AddArgument($"--height={configuration.WindowHeight}");
                }
                else
                {
                    if (configuration.WindowSize)
                    {
                        options.AddArgument($"--width={configuration.WindowWidth}");
                        options.AddArgument($"--height={configuration.WindowHeight}");
                    }
                }

                var driver = new FirefoxDriver(service, options);

                if (configuration.Url != null)
                    driver.Navigate().GoToUrl(configuration.Url);

                return driver;
            }
            else if (configuration.BrowserType == BrowserTypes.Edge.ToString())
            {
                var service = EdgeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new EdgeOptions();
                options.AddArgument("--disable-notifications");
                options.AddExcludedArgument("enable-automation");
                options.AddUserProfilePreference("credentials_enable_service", false);

                if (configuration.Maximize)
                    options.AddArguments("start-maximized");

                if (configuration.Headless)
                {
                    options.AddArgument($"--headless");
                    options.AddArgument($"--window-size={configuration.WindowWidth},{configuration.WindowHeight}");
                }
                else
                {
                    if (configuration.WindowSize)
                    {
                        options.AddArgument($"--window-size={configuration.WindowWidth},{configuration.WindowHeight}");
                        options.AddArgument($"--window-position={10},{10}");
                    }
                }

                var driver = new EdgeDriver(service, options);

                if (configuration.Url != null)
                    driver.Navigate().GoToUrl(configuration.Url);

                return driver;
            }
            else
            {
                throw new ArgumentException($"Specified browser type '{configuration.BrowserType}' is unknown...");
            }
        }
    }
}
