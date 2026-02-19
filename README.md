# Expressium LivingDoc

## Introduction
Expressium LivingDoc is an open-source tool that generates a single
HTML test report in a Living Documentation style for ReqnRoll projects.
The report is built upon the Cucumber Messages format produced by ReqnRoll
during the execution of Behavior-Driven Development (BDD) tests.
The final HTML test report may along with linked attachments
be distributed to a public location enabling easy access by the stackholders.

<br />
<p align="center">
<img src="ExpressiumLivingDoc.png"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />
</p>

**Example:** [https://expressium.dev/reqnroll/LivingDoc.html](https://expressium.dev/reqnroll/LivingDoc.html)

## Getting Started
Once a ReqnRoll test project has been created, you can easily integrate
the Expressium LivingDoc report by adding the Expressium LivingDoc PlugIn NuGet package
to the project and customizing the ReqnRoll formatter configuration.
The formatter configuration may include relative paths and predefined ReqnRoll substitution variables.

* Add the Expressium.LivingDoc.ReqnrollPlugin NuGet package to the ReqnRoll test project...
* Setup the Expressium formatters properties in the ReqnRoll configuration in the test project...
* Run the tests in the ReqnRoll test project and open the HTML report in the output directory...

```json
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

## History Analysis
The Expressium LivingDoc report may optionally include historical test results 
based on previous Cucumber Messages files.
Historical test results are visualized as trends and failures 
in the Analyze section of the LivingDoc report
and includes a maximum of four of the most recent previous test executions.
In a pipeline, the previous Cucumber message files
must be preserved in the history folder before executing the next test run.

```json
{
  "$schema": "https://schemas.reqnroll.net/reqnroll-config-latest.json",
  "formatters": {
    "expressium": {
      "outputFilePath": "LivingDoc.ndjson",
      "outputFileTitle": "Expressium.Coffeeshop.Web.API.Tests",
      "historyFilePath": "History/*.ndjson"
    }
  }
}
```

<br />
<p align="center">
<img src="HistoryAnalysis.png"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />
</p>

## Scenario Attachments
Since the AddAttachment API in ReqnRoll doesn’t support adding attachments as external links,
we need to use a workaround to enable attachments in the Expressium LivingDoc report.
Scenario attachments, such as log files and screenshots, can be stored in a relative location
and added as links to simplify distribution afterwards.

```c#
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

<br />

```c#
[AfterScenario]
public void AfterScenario()
{
    FinalizeFixture();

    if (configuration.Loggings)
    {
        var fileName = Path.Combine(Folders.Loggings.ToString(), GetTestName() + ".log");
        reqnrollOutputHelper.AddAttachmentAsLink(fileName);
    }

    if (configuration.Screenshots)
    {
        var fileName = Path.Combine(Folders.Screenshots.ToString(), GetTestName() + ".png");
        reqnrollOutputHelper.AddAttachmentAsLink(fileName);
    }
}
```

<br />
<p align="center">
<img src="Attachments.png"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />
</p>

## Command Line Interface
For many different purposes, it may be desirable to customize the final Expressium LivingDoc test report.
You can achieve this by creating a separate custom CLI project, adding a project reference to the ReqnRoll test project
and implementing any logic needed to handle your specific reporting requirements.
For other examples of custom CLI implementations, 
please refer to the Expressium LivingDoc CLI project and batch files in this repository.

```c#
if (args.Length == 7 && args[0] == "--custom")
{
    // Generating a custom LivingDoc Test Report based on a Cucumber Messages NDJSON file...
    Console.WriteLine("");
    Console.WriteLine("Generating LivingDoc Test Report...");
    Console.WriteLine("Input: " + args[2]);
    Console.WriteLine("Output: " + args[4]);
    Console.WriteLine("Title: " + args[6]);

    var livingDocConverter = new LivingDocConverter();
    var livingDocProject = livingDocConverter.Convert(args[2], args[6]);

    // Omitting overview folders...
    foreach (var feature in livingDocProject.Features)
        feature.Uri = null;

    livingDocConverter.Generate(livingDocProject, args[4]);

    Console.WriteLine("Generating LivingDoc Report Completed");
    Console.WriteLine("");
}
```

## Merging Reports
The ReqnRoll test execution may run across multiple pipelines
and it is desirable to produce a single consolidated test report.
A merging of test reports can be achieved through a separate CLI program.
Only new and previously unknown features will be included during the merge process.

```c#
if (args.Length == 8 && args[0] == "--merge")
{
    // Generating a LivingDoc Test Report based on Two Cucumber Messages NDJSON files...
    Console.WriteLine("");
    Console.WriteLine("Generating LivingDoc Test Report...");
    Console.WriteLine("InputMaster: " + args[2]);
    Console.WriteLine("InputSlave: " + args[3]);
    Console.WriteLine("Output: " + args[5]);
    Console.WriteLine("Title: " + args[7]);

    var livingDocConverter = new LivingDocConverter();
    var livingDocProject = livingDocConverter.Convert(args[2], args[7]);
    livingDocConverter.MergeProject(livingDocProject, args[3]);
    livingDocConverter.Generate(livingDocProject, args[5]);

    Console.WriteLine("Generating LivingDoc Report Completed");
    Console.WriteLine("");
}
```

## Deep Linking
When the Expressium LivingDoc report is opened in a browser,
the onload event automatically reads URL query parameters and applies filters accordingly.
This enables direct linking to specific scenarios or features filtered by keywords within the LivingDoc report.

```bat
start chrome "file:///C://Company/Coffeeshop.html?filterByKeywords=TA-3001"
```

<br />
<p align="center">
<img src="DeepLink.png"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />
</p>

## The LivingDoc Process
The Expressium LivingDoc process uses the Reqnroll PlugIn to capture test execution results and output them as Cucumber Messages (NDJSON).
These messages are parsed into an object-oriented model, which the Generator transforms into a self-contained HTML LivingDoc test report.

<br />
<p align="center">
<img src="Process.png"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />
</p>
