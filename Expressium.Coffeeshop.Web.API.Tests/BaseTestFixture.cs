using log4net;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    public class BaseTestFixture
    {
        protected readonly Configuration configuration;
        protected readonly LazyWebDriver lazyWebDriver;

        protected ILog logger;
        protected Asserts Asserts;

        protected IWebDriver driver => lazyWebDriver.Driver;

        public BaseTestFixture()
        {
            configuration = Configuration.GetCurrentConfiguation();
            lazyWebDriver = new LazyWebDriver(configuration);
        }

        [OneTimeSetUp]
        protected virtual void InitializeFixture()
        {
            if (configuration.Logging)
            {
                if (string.IsNullOrWhiteSpace(configuration.LoggingPath))
                    throw new ArgumentException("Property LoggingPath is undefined...");

                var loggingPath = Path.Combine(configuration.LoggingPath, GetTestName());
                if (Directory.Exists(loggingPath))
                    Directory.Delete(loggingPath, true);
                Directory.CreateDirectory(loggingPath);

                var loggingFilePath = Path.Combine(loggingPath, GetTestName() + ".log");
                logger = Logger.Initialize(GetTestName(), loggingFilePath);
                Asserts = new Asserts(logger);
            }
            else
            {
                logger = Logger.Initialize(GetTestName());
                Asserts = new Asserts(logger);
            }

            logger.InfoFormat("");
            logger.InfoFormat("// Initialize Test Fixture");
            logger.InfoFormat("Company: {0}", configuration.Company);
            logger.InfoFormat("Project: {0}", configuration.Project);
            logger.InfoFormat("Environment: {0}", configuration.Environment);
            logger.InfoFormat("Url: {0}", configuration.Url);
            logger.InfoFormat("Id: {0}", GetTestId());
            logger.InfoFormat("Name: {0}", GetTestName());
            logger.InfoFormat("Category: {0}", GetTestCategory());
            logger.InfoFormat("Description: {0}", GetTestDescription());
            logger.InfoFormat("Date: {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
        }

        [OneTimeTearDown]
        protected virtual void FinalizeFixture()
        {
            logger.InfoFormat("");
            logger.InfoFormat("// Finalize Test Fixture");

            if (driver != null)
            {
                if (configuration.Screenshot)
                {
                    var filePath = Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".png");
                    SaveScreenshot(filePath);
                }

                if (configuration.ShowAlerts)
                {
                    var message = $"Test has been executed with the status {GetTestStatus().ToUpper()}";
                    ShowAlert(message, configuration.ShowAlertsTimeOut);
                }

                driver.Quit();
            }

            LogMessageAsError(GetTestResultMessage());
            LogMessageAsDebug(GetTestResultStackTrace());

            logger.InfoFormat("Number of passed tests: {0}", GetTestPassCount());
            logger.InfoFormat("Number of skipped tests: {0}", GetTestSkipCount());
            logger.InfoFormat("Number of inconclusive tests: {0}", GetTestInconclusiveCount());
            logger.InfoFormat("Number of failed tests: {0}", GetTestFailCount());

            string statusCode;

            if (GetTestStatus() == TestStatus.Failed.ToString())
                statusCode = "Testcase has failed during execution...";
            else if (GetTestStatus() == TestStatus.Inconclusive.ToString())
                statusCode = "Testcase was inconclusive executed...";
            else if (GetTestStatus() == TestStatus.Skipped.ToString())
                statusCode = "Testcase was skipped from execution...";
            else
            {
                statusCode = "Testcase was successfully executed...";
            }

            logger.InfoFormat(statusCode + "\r\n");

            if (configuration.Logging)
                logger.Logger.Repository.Shutdown();
        }

        [SetUp]
        protected virtual void InitializeTest()
        {
            logger.InfoFormat("");
            logger.InfoFormat("// " + GetTestMethodName());
        }

        [TearDown]
        protected virtual void FinalizeTest()
        {
            if (GetTestStatus() == TestStatus.Failed.ToString())
            {
                LogMessageAsError(GetTestResultMessage());
                LogMessageAsDebug(GetTestResultStackTrace());
            }
        }

        protected virtual void InitializeBrowser()
        {
            if (configuration.ShowAlerts)
            {
                var message = GetTestDescription();
                ShowAlert(message, configuration.ShowAlertsTimeOut);
            }
        }

        protected virtual string GetTestId()
        {
            if (TestContext.CurrentContext.Test.Properties.Keys.Contains("Id"))
                return TestContext.CurrentContext.Test.Properties.Get("Id").ToString();

            return null;
        }

        protected virtual string GetTestName()
        {
            var name = TestContext.CurrentContext.Test.Name;
            return string.Join("", name.Split(Path.GetInvalidFileNameChars()));
        }

        protected virtual string GetTestMethodName()
        {
            return TestContext.CurrentContext.Test.Name.Replace("_", " ");
        }

        protected virtual string GetTestCategory()
        {
            if (TestContext.CurrentContext.Test.Properties.Keys.Contains("Category"))
                return TestContext.CurrentContext.Test.Properties.Get("Category").ToString();

            return null;
        }

        protected virtual string GetTestDescription()
        {
            if (TestContext.CurrentContext.Test.Properties.Keys.Contains("Description"))
                return TestContext.CurrentContext.Test.Properties.Get("Description").ToString();

            return null;
        }

        protected virtual string GetTestStatus()
        {
            return TestContext.CurrentContext.Result.Outcome.Status.ToString();
        }

        protected virtual string GetTestResultMessage()
        {
            return TestContext.CurrentContext.Result.Message;
        }

        protected virtual string GetTestResultStackTrace()
        {
            return TestContext.CurrentContext.Result.StackTrace;
        }

        protected virtual int GetTestPassCount()
        {
            return TestContext.CurrentContext.Result.PassCount;
        }

        protected virtual int GetTestSkipCount()
        {
            return TestContext.CurrentContext.Result.SkipCount;
        }

        protected virtual int GetTestInconclusiveCount()
        {
            return TestContext.CurrentContext.Result.InconclusiveCount;
        }

        protected virtual int GetTestFailCount()
        {
            return TestContext.CurrentContext.Result.FailCount;
        }

        private void SaveScreenshot(string filePath)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(filePath);
                logger.InfoFormat("Screenshot has been saved: {0}", Path.GetFileName(filePath));
            }
            catch (Exception exception)
            {
                logger.Error("SaveScreenshot has failed during execution...");
                logger.Debug(exception.Message);
            }
        }

        private void ShowAlert(string message, int timeOut)
        {
            try
            {
                var javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript($"alert('{message}');");
                System.Threading.Thread.Sleep(timeOut);
                driver.SwitchTo().Alert().Accept();
            }
            catch (Exception exception)
            {
                logger.Error("ShowAlert has failed during execution...");
                logger.Debug(exception.Message);
            }
        }

        private void LogMessageAsError(string message)
        {
            try
            {
                if (message != null)
                {
                    var listOfLines = GetStringAsListOfLines(message);
                    foreach (var line in listOfLines)
                        logger.Error(line);
                }
            }
            catch (Exception exception)
            {
                logger.Error("LogMessageAsError has failed during execution...");
                logger.Debug(exception.Message);
            }
        }

        private void LogMessageAsDebug(string message)
        {
            try
            {
                if (message != null)
                {
                    var listOfLines = GetStringAsListOfLines(message);
                    foreach (var line in listOfLines)
                        logger.Debug(line);
                }
            }
            catch (Exception exception)
            {
                logger.Error("LogMessageAsDebug has failed during execution...");
                logger.Debug(exception.Message);
            }
        }

        private List<string> GetStringAsListOfLines(string message)
        {
            var listOfLines = new List<string>();

            char[] delimiterChars = { '\r', '\n' };
            List<string> listOfTokens = message.Split(delimiterChars).ToList();
            foreach (var token in listOfTokens)
            {
                string text = token.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                    listOfLines.Add(token);
            }

            return listOfLines;
        }
    }
}
