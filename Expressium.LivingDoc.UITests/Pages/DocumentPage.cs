using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UITests.Pages
{
    public partial class DocumentPage : BasePage
    {
        private readonly By FeatureTags = By.XPath("//*[@id='splitter-right']//div[@class='feature-tag-group']");
        private readonly By FeatureDescription = By.XPath("//*[@id='splitter-right']//ul[@class='feature-description']");
        private readonly By FeatureName = By.XPath("//*[@id='splitter-right']//span[@class='feature-name']");
        private readonly By RuleTags = By.XPath("//*[@id='splitter-right']//div[@class='rule-tag-group']");
        private readonly By RuleNames = By.XPath("//*[@id='splitter-right']//span[@class='rule-name']");
        private readonly By ScenarioTags = By.XPath("//*[@id='splitter-right']//div[@class='scenario-tag-group']");
        private readonly By ScenarioNames = By.XPath("//*[@id='splitter-right']//span[@class='scenario-name']");
        private readonly By StepNames = By.XPath("//*[@id='splitter-right']//span[@class='step-name']");

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

        public List<string> GetRuleTags()
        {
            logger.Info("GetRuleTags()");
            return RuleTags.GetTexts(driver);
        }

        public List<string> GetRuleNames()
        {
            logger.Info("GetRuleNames()");
            return RuleNames.GetTexts(driver);
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
