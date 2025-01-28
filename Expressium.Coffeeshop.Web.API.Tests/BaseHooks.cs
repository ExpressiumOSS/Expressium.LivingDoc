using Reqnroll;
using Reqnroll.BoDi;
using System.IO;
using System.Text.RegularExpressions;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    [Binding]
    public partial class BaseHooks : BaseTestFixture
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private readonly IReqnrollOutputHelper reqnrollOutputHelper;
        private readonly IObjectContainer objectContainer;

        private ContextController contextController;

        public BaseHooks(FeatureContext featureContext, ScenarioContext scenarioContext, IReqnrollOutputHelper reqnrollOutputHelper, IObjectContainer objectContainer)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
            this.reqnrollOutputHelper = reqnrollOutputHelper;
            this.objectContainer = objectContainer;
        }

        private void InitializeDependencyInjection()
        {
            contextController = new ContextController();
            contextController.Configuration = configuration;
            contextController.Logger = logger;
            contextController.Asserts = Asserts;
            contextController.Driver = lazyWebDriver;

            objectContainer.RegisterInstanceAs(contextController);
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            InitializeTestExecution();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            FinalizeTestExecution();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            AddTestExecutionBeforeScenario();

            InitializeFixture();

            InitializeBrowser();
            InitializeDependencyInjection();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            FinalizeFixture();

            if (File.Exists(Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".log")))
            {
                reqnrollOutputHelper.AddAttachment(".\\" + GetTestName() + "\\" + GetTestName() + ".log");
                AddTestExecutionScenarioAttachment(Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".log"));
            }

            if (File.Exists(Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".png")))
            {
                reqnrollOutputHelper.AddAttachment(".\\" + GetTestName() + "\\" + GetTestName() + ".png");
                AddTestExecutionScenarioAttachment(Path.Combine(configuration.LoggingPath, GetTestName(), GetTestName() + ".png"));
            }

            AddTestExecutionAfterScenario();
        }

        [BeforeStep]
        public void BeforeStep()
        {
            logger.InfoFormat("");
            logger.InfoFormat("// " + scenarioContext.StepContext.StepInfo.StepDefinitionType + " " + scenarioContext.StepContext.StepInfo.Text);
        }

        [AfterStep]
        public void AfterStep()
        {
            AddTestExecutionAfterStep();

            logger.InfoFormat("");
            logger.InfoFormat("// " + scenarioContext.StepContext.StepInfo.StepDefinitionType + " " + scenarioContext.StepContext.StepInfo.Text);
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
