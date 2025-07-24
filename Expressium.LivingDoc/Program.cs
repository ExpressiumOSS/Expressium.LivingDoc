using Expressium.LivingDoc.Generators;
using System;

namespace Expressium.LivingDoc
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 6)
            {
                Console.WriteLine("");
                Console.WriteLine("Generating Cucumber Messages LivingDoc Report...");
                Console.WriteLine("Input: " + args[1]);
                Console.WriteLine("Output: " + args[3]);
                Console.WriteLine("Title: " + args[5]);
                Console.WriteLine("Executing LivingDoc Code Generator");

                var livingDocGenerator = new LivingDocGenerator(args[1], args[3], args[5]);
                livingDocGenerator.Execute();

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else if (args.Length == 3 && args[0] == "--native")
            {
                Console.WriteLine("");
                Console.WriteLine("Generating Native LivingDoc Report...");
                Console.WriteLine("Input: " + args[1]);
                Console.WriteLine("Output: " + args[2]);
                Console.WriteLine("Executing LivingDoc Code Generator");

                var livingDocGenerator = new LivingDocGenerator(args[1], args[2]);
                livingDocGenerator.Execute(true);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Expressium.LivingDoc.exe --input [INPUTFILE] --output [OUTPUTFILE] --title [TITLE]");
                Console.WriteLine("Expressium.LivingDoc.exe --input .\\TestExecution.json --output .\\LivingDoc.html --title \"Expressium CoffeeShop\"");
            }
        }
    }
}
