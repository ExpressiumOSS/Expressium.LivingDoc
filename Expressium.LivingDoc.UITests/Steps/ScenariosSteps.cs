using Expressium.LivingDoc.UITests.Pages;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class ScenariosSteps : BaseSteps
    {
        public ScenariosSteps(BaseContext baseContext) : base(baseContext)
        {
        }

        [Given("I have navigated to the Scenarios List")]
        public void GivenIHaveNavigatedToTheScenariosList()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickScenarios();

            var scenariosPage = new ScenariosPage(logger, driver);
            Asserts.EqualTo(scenariosPage.GetHeadline(), "Scenarios", "The Scenarios page headline");
        }

        [Then("I should have following visible objects in the Scenarios List")]
        public void ThenIShouldHaveFollowingVisibleObjectsInTheScenariosList(DataTable dataTable)
        {
            var objects = dataTable.CreateSet<Objects>();

            var listOfScenarios = new List<string>();
            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Scenarios))
                    listOfScenarios.Add(item.Scenarios);
            }

            var scenariosPage = new ScenariosPage(logger, driver);

            Asserts.EqualTo(scenariosPage.Grid.GetNumberOfScenarios(), listOfScenarios.Count(), "Validating the ScenariosPage Number Of Scenarios...");
            Asserts.EqualTo(scenariosPage.Grid.GetScenarioNames(), listOfScenarios, "Validating the ScenariosPage Scenario Names...");
        }

        [Then("I should have following number of visible objects in the Scenarios List")]
        public void ThenIShouldHaveFollowingNumberOfVisibleObjectsInTheScenariosList(DataTable dataTable)
        {
            var objects = dataTable.CreateInstance<Objects>();

            var scenariosPage = new ScenariosPage(logger, driver);
            Asserts.EqualTo(scenariosPage.Grid.GetNumberOfScenarios(), int.Parse(objects.Scenarios), "Validating the ScenariosPage Number Of Scenarios...");
        }

        [When("I sort the scenarios by (.*) column in the Scenarios List")]
        public void WhenISortTheScenariosByDescriptionColumnInTheScenariosList(string value)
        {
            var scenariosPage = new ScenariosPage(logger, driver);
            scenariosPage.Grid.SortByColumn(value);
        }

        private class Objects
        {
            public string Scenarios { get; set; }
        }
    }
}
