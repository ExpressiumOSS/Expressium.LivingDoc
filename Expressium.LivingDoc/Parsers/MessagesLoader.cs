using Io.Cucumber.Messages.Types;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc.Parsers
{
    internal class MessagesLoader
    {
        protected List<Meta> listOfMeta;
        protected List<GherkinDocument> listOfGherkinDocuments;
        protected List<Pickle> listOfPickles;
        protected List<TestCase> listOfTestCases;
        protected List<TestStepFinished> listOfTestStepFinished;
        protected List<TestCaseStarted> listOfTestCaseStarted;
        protected List<TestCaseFinished> listOfTestCaseFinished;
        protected List<TestRunStarted> listOfTestRunStarted;
        protected List<TestRunFinished> listOfTestRunFinished;
        protected List<Attachment> listOfAttachment;
        protected List<Hook> listOfHook;

        internal MessagesLoader()
        {
            listOfMeta = new List<Meta>();
            listOfGherkinDocuments = new List<GherkinDocument>();
            listOfPickles = new List<Pickle>();
            listOfTestCases = new List<TestCase>();
            listOfTestStepFinished = new List<TestStepFinished>();
            listOfTestCaseStarted = new List<TestCaseStarted>();
            listOfTestCaseFinished = new List<TestCaseFinished>();
            listOfTestRunStarted = new List<TestRunStarted>();
            listOfTestRunFinished = new List<TestRunFinished>();
            listOfAttachment = new List<Attachment>();
            listOfHook = new List<Hook>();
        }

        internal void LoadCucumberMessages(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                var enumerator = new MessagesReader(fileStream).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var envelope = enumerator.Current;

                    if (envelope.Meta != null)
                        listOfMeta.Add(envelope.Meta);

                    if (envelope.GherkinDocument != null)
                        listOfGherkinDocuments.Add(envelope.GherkinDocument);

                    if (envelope.Pickle != null)
                        listOfPickles.Add(envelope.Pickle);

                    if (envelope.TestCase != null)
                        listOfTestCases.Add(envelope.TestCase);

                    if (envelope.TestStepFinished != null)
                        listOfTestStepFinished.Add(envelope.TestStepFinished);

                    if (envelope.TestCaseStarted != null)
                        listOfTestCaseStarted.Add(envelope.TestCaseStarted);

                    if (envelope.TestCaseFinished != null)
                        listOfTestCaseFinished.Add(envelope.TestCaseFinished);

                    if (envelope.TestRunStarted != null)
                        listOfTestRunStarted.Add(envelope.TestRunStarted);

                    if (envelope.TestRunFinished != null)
                        listOfTestRunFinished.Add(envelope.TestRunFinished);

                    if (envelope.Attachment != null)
                        listOfAttachment.Add(envelope.Attachment);

                    if (envelope.Hook != null)
                        listOfHook.Add(envelope.Hook);
                }
            }
        }
    }
}
