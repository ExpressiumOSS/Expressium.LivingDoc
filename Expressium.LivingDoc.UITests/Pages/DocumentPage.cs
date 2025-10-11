using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class DocumentPage : BasePage
    {
        private readonly By FeatureTags = By.XPath("//*[@id='right-section']//span[@class='feature-tag-names']");
        private readonly By FeatureDescription = By.XPath("//*[@id='right-section']//ul[@class='feature-description']");
        private readonly By FeatureName = By.XPath("//*[@id='right-section']//span[@class='feature-name']");
        private readonly By ScenarioTags = By.XPath("//*[@id='right-section']//span[@class='scenario-tag-names']");
        private readonly By ScenarioNames = By.XPath("//*[@id='right-section']//span[@class='scenario-name']");
        private readonly By StepNames = By.XPath("//*[@id='right-section']//span[@class='step-name']");

        public DocumentPage(ILog logger, IWebDriver driver) : base(logger, driver)
        {
        }

        public string GetFeatureTags()
        {
            logger.Info("GetFeatureTags()");
            return FeatureTags.GetText(driver);
        }

        public string GetFeatureDescription()
        {
            logger.Info("GetFeatureDescription()");
            return FeatureDescription.GetText(driver);
        }

        public string GetFeatureName()
        {
            logger.Info("GetFeatureName()");
            return FeatureName.GetText(driver);
        }

        public List<string> GetScenarioTags()
        {
            logger.Info("GetScenarioTags()");
            return ScenarioTags.GetTexts(driver);
        }

        public List<string> GetScenarioNames()
        {
            logger.Info("GetScenarioNames()");
            return ScenarioNames.GetTexts(driver);
        }

        public List<string> GetStepNames()
        {
            logger.Info("GetStepNames()");
            return StepNames.GetTexts(driver);
        }
    }
}
