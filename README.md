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
* Configure ReqnRoll to enable Cucumber Messages JSON file generation...
* Create a Console App project in the solution for an Expressium LivingDoc program...
* Add the Expressium LivingDoc NuGet package to the Console App project...
* Add a project reference from the Console App project to the ReqnRoll test project...
* Run the ReqnRoll BDD tests in the solution to generate Cucumber Messages output...
* Run the Expressium LivingDoc Console App to generate a single HTML test report...

## Console App Program
```
using Expressium.LivingDoc.Generators;
using System;

namespace Expressium.LivingDoc
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                var livingDocGenerator = new LivingDocGenerator(args[0], args[1]);
                livingDocGenerator.Execute();
            }
            else
            {
                Console.WriteLine("Expressium.LivingDoc.exe [INPUTPATH] [OUTPUTPATH]");
                Console.WriteLine("Expressium.LivingDoc.exe C:\\SourceCode\\company-project-tests\\TestExecution.json C:\\SourceCode\\company-project-tests\\LivingDoc.html");
            }
        }
    }
}
```

## Command Line Arguments
```
Expressium.LivingDoc.exe stack-traces.feature.ndjson "Compatibility Kit Stack Traces.html"
```

## Demo Expressium LivingDoc Test Report
Web: https://expressium.dev/reqnroll/LivingDoc.html