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
            else if (args.Length == 3 && args[0] == "--native")
            {
                var livingDocGenerator = new LivingDocGenerator(args[1], args[2]);
                livingDocGenerator.Execute(true);
            }
            else
            {
                Console.WriteLine("Expressium.LivingDoc.exe [INPUTPATH] [OUTPUTPATH]");
                Console.WriteLine("Expressium.LivingDoc.exe C:\\SourceCode\\company-project-tests\\TestExecution.json C:\\SourceCode\\company-project-tests\\LivingDoc.html");
            }
        }
    }
}
