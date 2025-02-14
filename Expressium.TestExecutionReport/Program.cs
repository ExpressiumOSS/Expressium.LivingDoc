using Expressium.TestExecution.Cucumber;
using System;
using System.IO;

namespace Expressium.TestExecutionReport
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
            else if (args.Length == 3 && args[0] == "--cucumber")
            {
                var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "Intermediate.json");
                CucumberConvertor.SaveAsTestExecution(args[1], outputFile);

                var livingDocGenerator = new LivingDocGenerator(outputFile, args[2]);
                livingDocGenerator.Execute();
            }
            else
            {
                Console.WriteLine("Expressium.TestExecutionReport.exe [FILEPATH] [OUTPUTPATH]");
                Console.WriteLine("Expressium.TestExecutionReport.exe C:\\SourceCode\\company-project-tests\\TestExecution.json C:\\SourceCode\\company-project-tests\\TestReport");
            }
        }
    }
}
