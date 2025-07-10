# Expressium LivingDoc

The Expressium LivingDoc is generating a self-contained HTML test report
in a Living Documentation style for ReqnRoll projects.

The report generation is based on the Cucumber Messages format produced by ReqnRoll
during the execution of Behavior-Driven Development (BDD) tests.

The final HTML test report may be distributed together with attachment files
to a public location for easy access by the stackholders.

<br />
<img src="ExpressiumLivingDoc.png"
     alt="Expressium LivingDoc"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />

## How-To-Use
* Configure the ReqnRoll Cucumber Messages JSON file generation in the solution...
* Add the Expressium LivingDoc nuget package to your ReqnRoll test project...
* Create a Console App project for the Expressium LivingDoc Generator in the solution...
* Add a project reference from the Console App project reference to the ReqnRoll test project... 
* Execute the ReqnRoll Behavior-Driven Development tests in the solution...
* Execute the Expressium.LivingDoc.exe file with arguments from the output folder...

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