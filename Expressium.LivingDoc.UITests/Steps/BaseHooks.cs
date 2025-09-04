using Expressium.LivingDoc.UITests.Utilities;
using Reqnroll;
using Reqnroll.BoDi;
using System.IO;
using System.Text.RegularExpressions;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class BaseHooks : BaseTestFixture
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private readonly IReqnrollOutputHelper reqnrollOutputHelper;
        private readonly IObjectContainer objectContainer;

        private BaseContext baseContext;

        public BaseHooks(FeatureContext featureContext, ScenarioContext scenarioContext, IReqnrollOutputHelper reqnrollOutputHelper, IObjectContainer objectContainer)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
            this.reqnrollOutputHelper = reqnrollOutputHelper;
            this.objectContainer = objectContainer;
        }

        private void InitializeDependencyInjection()
        {
            baseContext = new BaseContext();
            baseContext.Configuration = configuration;
            baseContext.Logger = logger;
            baseContext.Asserts = Asserts;
            baseContext.Controller = controller;

            objectContainer.RegisterInstanceAs(baseContext);
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var livingDocGenerator = new LivingDocConverter("LivingDoc.ndjson", "LivingDoc.html", "Expressium.Coffeeshop.Web.API.Tests");
            livingDocGenerator.Execute();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            InitializeFixture();
            InitializeDependencyInjection();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            FinalizeFixture();

            if (File.Exists(Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".log")))
                reqnrollOutputHelper.AddAttachment(".\\" + GetTestName() + "\\" + GetTestName() + ".log");

            if (File.Exists(Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".png")))
                reqnrollOutputHelper.AddAttachment(".\\" + GetTestName() + "\\" + GetTestName() + ".png");
        }

        [BeforeStep]
        public void BeforeStep()
        {
            logger.Info("");
            logger.Info($"// {scenarioContext.StepContext.StepInfo.StepDefinitionType} {scenarioContext.StepContext.StepInfo.Text}");
        }

        protected override string GetTestId()
        {
            foreach (string tag in scenarioContext.ScenarioInfo.Tags)
            {
                if (tag.StartsWith("Id:"))
                    return tag.Replace("Id:", "");
            }

            return null;
        }

        protected override string GetTestName()
        {
            var name = scenarioContext.ScenarioInfo.Title;

            if (scenarioContext.ScenarioInfo.Arguments.Count > 0)
            {
                var arguments = scenarioContext.ScenarioInfo.Arguments.Values;
                foreach (var argument in arguments)
                    name += " " + argument;
            }

            name = string.Join("", name.Split(Path.GetInvalidFileNameChars()));
            return Regex.Replace(name, @"^\w| \w", (match) => match.Value.Replace(" ", "").ToUpper());
        }

        protected override string GetTestCategory()
        {
            if (scenarioContext.ScenarioInfo.Tags.Length > 0)
                return scenarioContext.ScenarioInfo.Tags[0];

            return null;
        }

        protected override string GetTestDescription()
        {
            return scenarioContext.ScenarioInfo.Title;
        }
    }
}
