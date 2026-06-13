using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.Parsers
{
    /// <summary>
    /// Coordinates conversion of a Cucumber messages NDJSON file into a LivingDocProject:
    /// loads the raw messages, builds the structural model (Gherkin phase), then overlays
    /// the test execution results (results phase).
    /// </summary>
    internal class MessagesParser
    {
        internal LivingDocProject ConvertToLivingDoc(string filePath)
        {
            var messages = new MessagesLoader().LoadCucumberMessages(filePath);

            var livingDocProject = new LivingDocProject();
            livingDocProject.Title = "Expressium LivingDoc";

            new MessagesGherkinParser().ParseGherkinDocuments(messages, livingDocProject);
            new MessagesResultParser().ParseTestResults(messages, livingDocProject);

            return livingDocProject;
        }
    }
}
