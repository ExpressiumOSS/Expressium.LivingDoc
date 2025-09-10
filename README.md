# Expressium LivingDoc

Expressium LivingDoc is an open-source tool that generates a single
HTML test report in a Living Documentation style for ReqnRoll projects.

The report is built upon the Cucumber Messages format produced by ReqnRoll
during the execution of Behavior-Driven Development (BDD) tests.

The final HTML test report may along with linked attachments
be distributed to a public location enabling easy access by the stackholders.

<br />
<img src="ExpressiumLivingDoc.png"
     alt="Expressium LivingDoc"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />

## How-To-Use
* Add the Expressium.LivingDoc.ReqnrollPlugin NuGet package to the ReqnRoll test project...
* Setup the Expressium formatters properties in the configuration of ReqnRoll test project...
* Run the tests in the ReqnRoll test project and open the HTML report in the output directory...

### ReqnRoll Configuration
```
{
  "$schema": "https://schemas.reqnroll.net/reqnroll-config-latest.json",
  "formatters": {
    "expressium": {
      "outputFilePath": "LivingDoc.ndjson",
      "outputFileTitle": "Expressium.Coffeeshop.Web.API.Tests"
    }
  }
}
```

### Attachments Work-Around
Since the AddAttachment API in ReqnRoll doesn’t support adding attachments as links,
we need to use a workaround to enable attachments in the Expressium LivingDoc report.

```
using Reqnroll;

namespace MyCompany.MyProject.Web.API.Tests
{
    internal static class ReqnRollExtensions
    {
        internal static void AddAttachmentAsLink(this IReqnrollOutputHelper outputHelper, string path)
        {
            outputHelper.WriteLine($"[Attachment: {path}]");
        }
    }
}
```

## Expressium LivingDoc Demo Test Report
**Web:** https://expressium.dev/reqnroll/LivingDoc.html
