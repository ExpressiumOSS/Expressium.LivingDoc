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

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("Ambiguous Step Definition..."));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Ambiguous.ToString()));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[1].Status, Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
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

            Assert.That(livingDocProject.Features[0].Scenarios[8].Examples[0].Attachments[0], Is.EqualTo("https://cucumber.io"));
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

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have $500 in my checking account"));
        }

        [Test]
        public void Converting_Data_Tables_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "data-tables.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(2));

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].DataTable.Rows.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].DataTable.Rows[0].Cells.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].DataTable.Rows[0].Cells[0], Is.EqualTo("a"));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].DataTable.Rows[0].Cells[1], Is.EqualTo("b"));
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
        public void Converting_Example_Tables_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(9));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(27));

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].DataTable.Rows.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].DataTable.Rows[0].Cells.Count, Is.EqualTo(3));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].DataTable.Rows[0].Cells[0], Is.EqualTo("start"));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].DataTable.Rows[0].Cells[1], Is.EqualTo("eat"));

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[2].Steps[2].ExceptionMessage, Does.Contain("Expected values to be strictly equal"));
        }

        [Test]
        public void Converting_Minimal_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "minimal.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(1));

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].Keyword, Is.EqualTo("Given"));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].Name, Is.EqualTo("I have 42 cukes in my belly"));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Passed.ToString()));
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

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].Message, Is.EqualTo("TODO"));
            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].Status, Is.EqualTo(LivingDocStatuses.Pending.ToString()));
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

            Assert.That(livingDocProject.Features[0].Rules[0].Name, Is.EqualTo("A sale cannot happen if the customer does not have enough money"));
            Assert.That(livingDocProject.Features[0].Scenarios[0].RuleId[0], Is.EqualTo('1'));
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

            Assert.That(livingDocProject.Features[0].Scenarios[0].Examples[0].Steps[0].ExceptionMessage, Is.EqualTo("BOOM"));
        }

        [Test]
        public void Converting_Skipped_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "skipped.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(5));
        }

        [Test]
        public void Converting_HooksErrors_Feature()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "hooks-errors.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(3));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(3));

            // TODO - Missing implementation and validation of hook errors...
        }
    }
}
