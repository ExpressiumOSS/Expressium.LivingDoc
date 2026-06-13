using Io.Cucumber.Messages.Types;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Parsers
{
    internal class CucumberMessages
    {
        internal List<Meta> Meta { get; }
        internal List<GherkinDocument> GherkinDocuments { get; }
        internal List<Pickle> Pickles { get; }
        internal List<TestCase> TestCases { get; }
        internal List<TestStepFinished> TestStepFinished { get; }
        internal List<TestCaseStarted> TestCaseStarted { get; }
        internal List<TestCaseFinished> TestCaseFinished { get; }
        internal List<TestRunStarted> TestRunStarted { get; }
        internal List<TestRunFinished> TestRunFinished { get; }
        internal List<Attachment> Attachment { get; }
        internal List<Hook> Hook { get; }

        internal CucumberMessages()
        {
            Meta = new List<Meta>();
            GherkinDocuments = new List<GherkinDocument>();
            Pickles = new List<Pickle>();
            TestCases = new List<TestCase>();
            TestStepFinished = new List<TestStepFinished>();
            TestCaseStarted = new List<TestCaseStarted>();
            TestCaseFinished = new List<TestCaseFinished>();
            TestRunStarted = new List<TestRunStarted>();
            TestRunFinished = new List<TestRunFinished>();
            Attachment = new List<Attachment>();
            Hook = new List<Hook>();
        }
    }
}
