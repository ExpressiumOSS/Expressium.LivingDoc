using Expressium.LivingDoc.UITests.Pages;
using Expressium.LivingDoc.UITests.Utilities;
using Reqnroll;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class DocumentsSteps : BaseSteps
    {
        public DocumentsSteps(BaseContext baseContext) : base(baseContext)
        {
        }

        [Then("I should have following feature properties in the Document View")]
        public void ThenIShouldHaveFollowingFeaturePropertiesInTheDocumentView(DataTable dataTable)
        {
            var documentPage = new DocumentPage(logger, driver);

            var objects = dataTable.CreateSet<Objects>();

            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Tags))
                    Asserts.EqualTo(item.Tags, documentPage.GetFeatureTags(), "Validating the DocumentPage Feature Tags...");

                if (!string.IsNullOrWhiteSpace(item.Name))
                    Asserts.EqualTo(item.Name, documentPage.GetFeatureName(), "Validating the DocumentPage Feature Name...");

                if (!string.IsNullOrWhiteSpace(item.Description))
                    Asserts.IsTrue(documentPage.GetFeatureDescription().Contains(item.Description), "Validating the DocumentPage Feature Description...");
            }
        }

        [Then("I should have following scenario properties in the Document View")]
        public void ThenIShouldHaveFollowingScenarioPropertiesInTheDocumentView(DataTable dataTable)
        {
            var documentPage = new DocumentPage(logger, driver);

            var objects = dataTable.CreateSet<Objects>();

            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Tags))
                    Asserts.IsTrue(documentPage.GetScenarioTags().Contains(item.Tags), "Validating the DocumentPage Scenario Tags...");

                if (!string.IsNullOrWhiteSpace(item.Name))
                    Asserts.IsTrue(documentPage.GetScenarioNames().Contains(item.Name), "Validating the DocumentPage Scenario Name...");
            }
        }

        [Then("I should have following step properties in the Document View")]
        public void ThenIShouldHaveFollowingStepPropertiesInTheDocumentView(DataTable dataTable)
        {
            var documentPage = new DocumentPage(logger, driver);

            var objects = dataTable.CreateSet<Objects>();

            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                    Asserts.IsTrue(documentPage.GetStepNames().Contains(item.Name), "Validating the DocumentPage Step Name...");
            }
        }

        private class Objects
        {
            public string Tags { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
