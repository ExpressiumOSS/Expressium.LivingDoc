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
* Create a Console App project in the solution for the custom LivingDoc program...
* Add the Expressium LivingDoc NuGet package to the Console App project...
* Add a project reference from the Console App project to the ReqnRoll test project...
* Configure Cucumber Messages output file path in the ReqnRoll test project...
* Run the tests in the ReqnRoll test project to generate the Cucumber Messages file...
* Run the custom LivingDoc program to generate a LivingDoc report in the output directory...

### Console App Program
```
using System;

namespace MyCompany.LivingDoc
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 6)
            {
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("Input: " + args[1]);
                Console.WriteLine("Output: " + args[3]);
                Console.WriteLine("Title: " + args[5]);

                var livingDocGenerator = new LivingDocConverter(args[1], args[3], args[5]);
                livingDocGenerator.Execute();

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("MyCompany.LivingDoc.exe --input [INPUTFILE] --output [OUTPUTFILE] --title [TITLE]");
                Console.WriteLine("MyCompany.LivingDoc.exe --input .\\ReqnRoll.ndjson --output .\\LivingDoc.html --title \"Expressium CoffeeShop Report\"");
            }
        }
    }
}
```

### ReqnRoll Configuration
```
{
  "$schema": "https://schemas.reqnroll.net/reqnroll-config-latest.json",
  "formatters": {
    "message": {
      "outputFilePath": "ReqnRoll.ndjson"
    }
  }
}
```

### Attachments Work-Around
```
using Reqnroll;

namespace MyCompany.Coffeeshop.Web.API.Tests
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
