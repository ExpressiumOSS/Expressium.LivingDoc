using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Messages
{
    internal class MessagesConvertorTests
    {
        [Test]
        public void Converting_Ambiguous_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "ambiguous.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("Ambiguous Step Definition..."));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Ambiguous.ToString()));
        }

        [Test]
        public void Converting_Attachments_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "attachments.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(9));

            var scenario = livingDocProject.Features[0].Scenarios[8];

            Assert.That(scenario.Examples[0].Attachments[0], Is.EqualTo("https://cucumber.io"));
        }

        [Test]
        public void Converting_Background_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "background.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(13));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Background.Steps[0].Name, Is.EqualTo("I have $500 in my checking account"));
            Assert.That(feature.Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have $500 in my checking account"));
        }

        [Test]
        public void Converting_DataTables_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "data-tables.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));

            var dataTable = livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].DataTable;

            Assert.That(dataTable.Rows.Count, Is.EqualTo(2));

            Assert.That(dataTable.Rows[0].Cells.Count, Is.EqualTo(2));
            Assert.That(dataTable.Rows[0].Cells[0], Is.EqualTo("a"));
            Assert.That(dataTable.Rows[0].Cells[1], Is.EqualTo("b"));

            Assert.That(dataTable.Rows[1].Cells.Count, Is.EqualTo(2));
            Assert.That(dataTable.Rows[1].Cells[0], Is.EqualTo("1"));
            Assert.That(dataTable.Rows[1].Cells[1], Is.EqualTo("2"));
        }

        [Test]
        public void Converting_Empty_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "empty.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(0));
        }

        [Test]
        public void Converting_ExampleTables_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(27));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].DataTable.Rows.Count, Is.EqualTo(2));
            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells.Count, Is.EqualTo(3));

            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells[0], Is.EqualTo("start"));
            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells[1], Is.EqualTo("eat"));
            Assert.That(scenario.Examples[0].DataTable.Rows[0].Cells[2], Is.EqualTo("left"));

            Assert.That(scenario.Examples[0].DataTable.Rows[1].Cells[0], Is.EqualTo("12"));
            Assert.That(scenario.Examples[0].DataTable.Rows[1].Cells[1], Is.EqualTo("5"));
            Assert.That(scenario.Examples[0].DataTable.Rows[1].Cells[2], Is.EqualTo("7"));

            Assert.That(scenario.Examples[2].Steps[2].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(scenario.Examples[2].Steps[2].ExceptionType, Is.EqualTo("AssertionError"));
            Assert.That(scenario.Examples[2].Steps[2].ExceptionMessage, Does.Contain("Expected values to be strictly equal"));
        }

        [Test]
        public void Converting_Minimal_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Name, Is.EqualTo("cukes"));
            Assert.That(scenario.GetStatus, Is.EqualTo(LivingDocStatuses.Passed.ToString()));

            Assert.That(scenario.Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(scenario.Examples[0].Steps[0].Name, Is.EqualTo("I have 42 cukes in my belly"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
        }

        [Test]
        public void Converting_Pending_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "pending.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(5));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[0].Message, Is.EqualTo("TODO"));
            Assert.That(scenario.Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
        }

        [Test]
        public void Converting_Rules_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "rules.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(12));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Uri, Is.EqualTo("samples/rules/rules.feature"));

            Assert.That(feature.Rules[0].Name, Is.EqualTo("A sale cannot happen if the customer does not have enough money"));
            Assert.That(feature.Rules[0].Id, Is.EqualTo("16"));
            Assert.That(feature.Scenarios[0].RuleId, Is.EqualTo("16"));

            Assert.That(feature.Rules[1].Name, Is.EqualTo("a sale cannot happen if there is no stock"));
            Assert.That(feature.Rules[1].Id, Is.EqualTo("23"));
            Assert.That(feature.Scenarios[2].RuleId, Is.EqualTo("23"));
        }

        [Test]
        public void Converting_StackTraces_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "stack-traces.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            Assert.That(scenario.Examples[0].Steps[0].Message, Is.EqualTo("BOOM\nsamples/stack-traces/stack-traces.feature:9"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo("Error"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("BOOM"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Is.EqualTo(null));
        }

        [Test]
        public void Converting_Skipped_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "skipped.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(5));

            var scenario = livingDocProject.Features[0].Scenarios[1];

            Assert.That(scenario.Examples[0].Steps[1].Keyword, Is.EqualTo("And"));
            Assert.That(scenario.Examples[0].Steps[1].Name, Is.EqualTo("I skip a step"));
            Assert.That(scenario.Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
        }

        [Test]
        public void Converting_HooksErrors_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "hooks-errors.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(3));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Uri, Is.EqualTo("samples/hooks-conditional/hooks-conditional.feature"));

            var scenario = livingDocProject.Features[0].Scenarios[0];

            // TODO - Missing implementation and validation of hook errors...
            //Assert.That(scenario.ExceptionType, Is.EqualTo("Error"));
            //Assert.That(scenario.ExceptionMessage, Is.EqualTo("BOOM"));
            //Assert.That(scenario..ExceptionStackTrace, Is.EqualTo("xxx"));
        }

        [Test]
        public void Converting_Retry_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "retry.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(5));

            var feature = livingDocProject.Features[0];

            Assert.That(feature.Uri, Is.EqualTo("samples/retry/retry.feature"));

            var scenario = livingDocProject.Features[0].Scenarios[3];

            Assert.That(scenario.Examples[0].Steps[0].Message, Is.EqualTo("Exception in step\nsamples/retry/retry.feature:18"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionType, Is.EqualTo("Error"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("Exception in step"));
            Assert.That(scenario.Examples[0].Steps[0].ExceptionStackTrace, Is.EqualTo(null));
        }
    }
}
