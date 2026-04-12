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

        [Then("I should have following number of scenarios in the Document View")]
        public void ThenIShouldHaveFollowingNumberOfScenariosInTheDocumentView(DataTable dataTable)
        {
            var documentPage = new DocumentPage(logger, driver);

            var objects = dataTable.CreateSet<Objects>();

            foreach (var item in objects)
            {
                Asserts.EqualTo(item.Numbers, documentPage.GetScenarioNames().Count, "Validating the DocumentPage Scenario Numbers...");
            }
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

        [Then("I should have following rule properties in the Document View")]
        public void ThenIShouldHaveFollowingRulePropertiesInTheDocumentView(DataTable dataTable)
        {
            var documentPage = new DocumentPage(logger, driver);

            var objects = dataTable.CreateSet<Objects>();

            foreach (var item in objects)
            {
                if (!string.IsNullOrWhiteSpace(item.Tags))
                    Asserts.IsTrue(documentPage.GetRuleTags().Contains(item.Tags), "Validating the DocumentPage Rule Tags...");

                if (!string.IsNullOrWhiteSpace(item.Name))
                    Asserts.IsTrue(documentPage.GetRuleNames().Contains(item.Name), "Validating the DocumentPage Rule Name...");
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

        [When("I toggle the attachments in the Document View")]
        public void WhenIToggleTheAttachmentsInTheDocumentView()
        {
            var documentPage = new DocumentPage(logger, driver);
            documentPage.ToggleAttachments();
        }

        [Then("I should have attachments visible in the Document View")]
        public void ThenIShouldHaveAttachmentsVisibleInTheDocumentView()
        {
            var documentPage = new DocumentPage(logger, driver);
            Asserts.IsTrue(documentPage.IsAttachmentsVisible(), "Validating the DocumentPage Attachments are visible...");
        }

        [Then("I should have attachments hidden in the Document View")]
        public void ThenIShouldHaveAttachmentsHiddenInTheDocumentView()
        {
            var documentPage = new DocumentPage(logger, driver);
            Asserts.IsFalse(documentPage.IsAttachmentsVisible(), "Validating the DocumentPage Attachments are hidden...");
        }

        [When("I toggle the stacktraces in the Document View")]
        public void WhenIToggleTheStacktracesInTheDocumentView()
        {
            var documentPage = new DocumentPage(logger, driver);
            documentPage.ToggleStacktraces();
        }

        [Then("I should have stacktraces visible in the Document View")]
        public void ThenIShouldHaveStacktracesVisibleInTheDocumentView()
        {
            var documentPage = new DocumentPage(logger, driver);
            Asserts.IsTrue(documentPage.IsStacktracesVisible(), "Validating the DocumentPage Stacktraces are visible...");
        }

        [Then("I should have stacktraces hidden in the Document View")]
        public void ThenIShouldHaveStacktracesHiddenInTheDocumentView()
        {
            var documentPage = new DocumentPage(logger, driver);
            Asserts.IsFalse(documentPage.IsStacktracesVisible(), "Validating the DocumentPage Stacktraces are hidden...");
        }

        private class Objects
        {
            public int Numbers { get; set; }
            public string Tags { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
