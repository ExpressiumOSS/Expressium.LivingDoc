using Io.Cucumber.Messages.Types;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Parsers
{
    internal class CucumberMessages
    {
        internal List<Meta> Metas { get; }
        internal List<GherkinDocument> GherkinDocuments { get; }
        internal List<Pickle> Pickles { get; }
        internal List<TestCase> TestCases { get; }
        internal List<TestStepFinished> TestStepFinished { get; }
        internal List<TestCaseStarted> TestCaseStarted { get; }
        internal List<TestCaseFinished> TestCaseFinished { get; }
        internal List<TestRunStarted> TestRunStarted { get; }
        internal List<TestRunFinished> TestRunFinished { get; }
        internal List<Attachment> Attachments { get; }
        internal List<Hook> Hooks { get; }

        internal CucumberMessages()
        {
            Metas = new List<Meta>();
            GherkinDocuments = new List<GherkinDocument>();
            Pickles = new List<Pickle>();
            TestCases = new List<TestCase>();
            TestStepFinished = new List<TestStepFinished>();
            TestCaseStarted = new List<TestCaseStarted>();
            TestCaseFinished = new List<TestCaseFinished>();
            TestRunStarted = new List<TestRunStarted>();
            TestRunFinished = new List<TestRunFinished>();
            Attachments = new List<Attachment>();
            Hooks = new List<Hook>();
        }
    }
}
