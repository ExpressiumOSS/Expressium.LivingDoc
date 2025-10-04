using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc.Cli
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 6)
            {
                // Generating a LivingDoc Test Report based on a Cucumber Messages JSON file...
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("Input: " + args[1]);
                Console.WriteLine("Output: " + args[3]);
                Console.WriteLine("Title: " + args[5]);

                var livingDocConverter = new LivingDocConverter();
                livingDocConverter.Generate(args[1], args[3], args[5]);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else if (args.Length == 3 && args[0] == "--native")
            {
                // Generating a LivingDoc Test Report based on a Native JSON file...
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("Input: " + args[1]);
                Console.WriteLine("Output: " + args[2]);

                var livingDocNativeConverter = new LivingDocNativeConverter();
                livingDocNativeConverter.Generate(args[1], args[2]);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else if (args.Length == 5 && args[0] == "--merge")
            {
                // Generating a LivingDoc Test Report based on Two Cucumber Messages JSON files...
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("InputMaster: " + args[1]);
                Console.WriteLine("InputSlave: " + args[2]);
                Console.WriteLine("Output: " + args[3]);
                Console.WriteLine("Title: " + args[4]);

                var livingDocConverter = new LivingDocConverter();
                livingDocConverter.Generate(new List<string>() { args[1], args[2] }, args[3], args[4]);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Expressium.LivingDoc.Cli.exe --input [INPUTFILE] --output [OUTPUTFILE] --title [TITLE]");
                Console.WriteLine("Expressium.LivingDoc.Cli.exe --input .\\ReqnRoll.ndjson --output .\\LivingDoc.html --title \"Expressium CoffeeShop Report\"");
            }
        }
    }
}