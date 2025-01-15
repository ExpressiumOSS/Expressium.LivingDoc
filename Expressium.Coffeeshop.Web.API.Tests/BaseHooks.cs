using Reqnroll;
using Reqnroll.BoDi;
using ReqnRoll.TestExecution;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    [Binding]
    public class BaseHooks : BaseTestFixture
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private readonly IReqnrollOutputHelper reqnrollOutputHelper;
        private readonly IObjectContainer objectContainer;

        private ContextController contextController;

        private static ExecutionContext executionContext;
        private DateTime executionStartTime;
        private DateTime executionEndTime;

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
            contextController.Driver = driver;

            objectContainer.RegisterInstanceAs(contextController);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            AddExecutionFeature();
            AddExecutionScenario();

            InitializeFixture();

            InitializeBrowser();
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
            logger.InfoFormat("");
            logger.InfoFormat("// " + scenarioContext.StepContext.StepInfo.StepDefinitionType + " " + scenarioContext.StepContext.StepInfo.Text);
        }

        [AfterStep]
        public void AfterStep()
        {
            ExecutionStep();

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
            string name = scenarioContext.ScenarioInfo.Title;
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

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            executionContext = new ExecutionContext();
            executionContext.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            executionContext.ExecutionTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            System.Threading.Thread.Sleep(1000);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ExecutionUtilities.SerializeAsJson(Path.Combine(Directory.GetCurrentDirectory(), "TestExecution.json"), executionContext);
        }

        private void AddExecutionFeature()
        {
            if (!executionContext.IsFeatureAdded(featureContext.FeatureInfo.Title))
            {
                executionContext.Features.Add(new ExecutionFeature()
                {
                    Tags = string.Join(", ", featureContext.FeatureInfo.Tags),
                    Title = featureContext.FeatureInfo.Title,
                    Description = featureContext.FeatureInfo.Description,
                    FolderPath = featureContext.FeatureInfo.FolderPath
                });
            }
        }

        private void AddExecutionScenario()
        {
            if (executionContext.IsFeatureAdded(featureContext.FeatureInfo.Title))
            {
                executionStartTime = DateTime.Now;

                var executionFeature = executionContext.GetFeature(featureContext.FeatureInfo.Title);
                executionFeature.Scenarios.Add(new ExecutionScenario()
                {
                    Tags = string.Join(", ", scenarioContext.ScenarioInfo.Tags),
                    Title = scenarioContext.ScenarioInfo.Title,
                    Description = scenarioContext.ScenarioInfo.Description
                });
            }
        }

        private void ExecutionStep()
        {
            if (executionContext.IsFeatureAdded(featureContext.FeatureInfo.Title))
            {
                executionEndTime = DateTime.Now;

                var executionFeature = executionContext.GetFeature(featureContext.FeatureInfo.Title);
                if (executionFeature.IsScenarioAdded(scenarioContext.ScenarioInfo.Title))
                {
                    var executionScenario = executionFeature.GetScenario(scenarioContext.ScenarioInfo.Title);
                    executionScenario.Status = scenarioContext.ScenarioExecutionStatus.ToString();
                    executionScenario.Duration = (executionEndTime - executionStartTime).ToString();

                    executionScenario.Steps.Add(new ExecutionStep()
                    {
                        Type = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString(),
                        Text = scenarioContext.StepContext.StepInfo.Text,
                        Status = scenarioContext.StepContext.Status.ToString(),
                        Error = scenarioContext.TestError?.Message
                    });
                }
            }
        }
    }
}
