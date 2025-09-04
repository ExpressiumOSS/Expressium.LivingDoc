using log4net;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Utilities
{
    public class BaseTestFixture
    {
        protected readonly Configuration configuration;
        protected readonly WebDriverController controller;

        protected ILog logger;
        protected Asserts Asserts;

        protected IWebDriver driver => controller.Driver;

        public BaseTestFixture()
        {
            configuration = Configuration.GetCurrentConfiguation();
            controller = new WebDriverController(configuration);
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

            logger.Info("");
            logger.Info("// Initialize Test Fixture");
            logger.Info($"Company: {configuration.Company}");
            logger.Info($"Project: {configuration.Project}");
            logger.Info($"Environment: {configuration.Environment}");
            logger.Info($"Url: {configuration.Url}");
            logger.Info($"Id: {GetTestId()}");
            logger.Info($"Name: {GetTestName()}");
            logger.Info($"Category: {GetTestCategory()}");
            logger.Info($"Description: {GetTestDescription()}");
            logger.Info($"Date: {DateTime.Now.ToString("dd-MM-yyyy HH:mm")}");
        }

        [OneTimeTearDown]
        protected virtual void FinalizeFixture()
        {
            logger.Info("");
            logger.Info("// Finalize Test Fixture");

            if (controller.IsInitialized())
            {
                if (configuration.Screenshot)
                {
                    var filePath = Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".png");
                    controller.SaveScreenshot(filePath);
                    logger.Info($"Screenshot has been saved: {Path.GetFileName(filePath)}");
                }

                controller.Quit();
            }

            LogMessageAsError(GetTestResultMessage());
            LogMessageAsDebug(GetTestResultStackTrace());

            logger.Info($"Number of passed tests: {GetTestPassCount()}");
            logger.Info($"Number of skipped tests: {GetTestSkipCount()}");
            logger.Info($"Number of inconclusive tests: {GetTestInconclusiveCount()}");
            logger.Info($"Number of failed tests: {GetTestFailCount()}");

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

            logger.Info(statusCode + "\r\n");

            if (configuration.Logging)
                logger.Logger.Repository.Shutdown();
        }

        [SetUp]
        protected virtual void InitializeTest()
        {
            logger.Info("");
            logger.Info($"// {GetTestMethodName()}");
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
            var listOfTokens = message.Split(delimiterChars).ToList();
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
