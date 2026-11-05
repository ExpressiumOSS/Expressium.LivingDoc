using Expressium.LivingDoc.UITests.Pages;
using Expressium.LivingDoc.UITests.Utilities;
using Reqnroll;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class PropertiesSteps : BaseSteps
    {
        public PropertiesSteps(BaseContext baseContext) : base(baseContext)
        {
        }

        [Given("I have navigated to the Properties")]
        public void GivenIHaveNavigatedToTheProperties()
        {
            var mainMenuBar = new MenuBar(logger, driver);
            mainMenuBar.ClickProperties();

            var propertiesPage = new PropertiesPage(logger, driver);
            Asserts.EqualTo(propertiesPage.GetPageTitle(), "Properties", "The Properties page headline is valid");
        }

        [Then("I should have following sections in Properties")]
        public void ThenIShouldHaveFollowingSectionsInProperties(DataTable dataTable)
        {
            var propertiesPage = new PropertiesPage(logger, driver);

            var expectedNames = dataTable.CreateSet<Objects>().Select(x => x.Name).ToList();
            var actualNames = propertiesPage.GetSectionNames();

            Asserts.EqualTo(string.Join(", ", actualNames), string.Join(", ", expectedNames), "Validating the PropertiesPage Section Names...");
        }

        [Then("I should have following properties in the {string} section of Properties")]
        public void ThenIShouldHaveFollowingPropertiesInTheSectionOfProperties(string section, DataTable dataTable)
        {
            var propertiesPage = new PropertiesPage(logger, driver);

            var objects = dataTable.CreateSet<Objects>().ToList();

            var expectedNames = objects.Select(x => x.Name).ToList();
            var actualNames = propertiesPage.GetPropertyNames(section);
            Asserts.EqualTo(string.Join(", ", actualNames), string.Join(", ", expectedNames), "Validating the PropertiesPage Property Names...");

            foreach (var item in objects)
                Asserts.EqualTo(propertiesPage.GetPropertyValue(section, item.Name), item.Value, $"Validating the PropertiesPage Property Value of '{item.Name}'...");
        }

        private class Objects
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
