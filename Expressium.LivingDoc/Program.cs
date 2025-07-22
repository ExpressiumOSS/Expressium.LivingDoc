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
                Console.WriteLine("");
                Console.WriteLine("Generating Cucumber Messages LivingDoc Report...");
                Console.WriteLine("InputPath: " + args[0]);
                Console.WriteLine("OutputPath: " + args[1]);
                Console.WriteLine("Executing LivingDoc Code Generator");

                var livingDocGenerator = new LivingDocGenerator(args[0], args[1]);
                livingDocGenerator.Execute();

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else if (args.Length == 3 && args[0] == "--native")
            {
                Console.WriteLine("");
                Console.WriteLine("Generating Native LivingDoc Report...");
                Console.WriteLine("InputPath: " + args[1]);
                Console.WriteLine("OutputPath: " + args[2]);
                Console.WriteLine("Executing LivingDoc Code Generator");

                var livingDocGenerator = new LivingDocGenerator(args[1], args[2]);
                livingDocGenerator.Execute(true);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Expressium.LivingDoc.exe [INPUTPATH] [OUTPUTPATH]");
                Console.WriteLine("Expressium.LivingDoc.exe C:\\SourceCode\\company-project-tests\\TestExecution.json C:\\SourceCode\\company-project-tests\\LivingDoc.html");
            }
        }
    }
}
