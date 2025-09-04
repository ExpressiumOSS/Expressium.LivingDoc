using Expressium.LivingDoc.UITests.Pages;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class StepsSteps : BaseSteps
    {
        public StepsSteps(BaseContext baseContext) : base(baseContext)
        {
        }

        [Given("I have navigated to the Steps List")]
        public void GivenIHaveNavigatedToTheStepsList()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickSteps();

            var stepsPage = new StepsPage(logger, driver);
            Asserts.EqualTo(stepsPage.GetHeadline(), "Steps", "The Steps page headline");
        }

        [Then("I should have following visible objects in the Steps List")]
        public void ThenIShouldHaveFollowingVisibleObjectsInTheStepsList(DataTable dataTable)
        {
            var objects = dataTable.CreateSet<Objects>();

            var listOfSteps = new List<string>();
            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Steps))
                    listOfSteps.Add(item.Steps);
            }

            var stepsPage = new StepsPage(logger, driver);

            Asserts.EqualTo(stepsPage.Grid.GetNumberOfSteps(), listOfSteps.Count(), "Validating the StepsPage Number Of Steps...");
            Asserts.EqualTo(stepsPage.Grid.GetStepNames(), listOfSteps, "Validating the StepsPage Scenario Names...");
        }

        [Then("I should have following number of visible objects in the Steps List")]
        public void ThenIShouldHaveFollowingNumberOfVisibleObjectsInTheStepsList(DataTable dataTable)
        {
            var objects = dataTable.CreateInstance<Objects>();

            var stepsPage = new StepsPage(logger, driver);
            Asserts.EqualTo(stepsPage.Grid.GetNumberOfSteps(), int.Parse(objects.Steps), "Validating the StepsPage Number Of Steps...");
        }

        [When("I sort the steps by (.*) column in the Steps List")]
        public void WhenISortTheStepsByDescriptionColumnInTheStepsList(string value)
        {
            var stepsPage = new StepsPage(logger, driver);
            stepsPage.Grid.SortByColumn(value);
        }

        private class Objects
        {
            public string Steps { get; set; }
        }
    }
}
