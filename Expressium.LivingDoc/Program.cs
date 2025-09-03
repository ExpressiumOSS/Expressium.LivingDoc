using System;

namespace Expressium.LivingDoc
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 6)
            {
                // Generating a LivingDoc Test Report based on Cucumber Messages JSON file...
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
            else if (args.Length == 3 && args[0] == "--native")
            {
                // Generating a LivingDoc Test Report based on Native JSON file...
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("Input: " + args[1]);
                Console.WriteLine("Output: " + args[2]);

                var livingDocGenerator = new LivingDocNativeConverter(args[1], args[2]);
                livingDocGenerator.Execute();

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Expressium.LivingDoc.exe --input [INPUTFILE] --output [OUTPUTFILE] --title [TITLE]");
                Console.WriteLine("Expressium.LivingDoc.exe --input .\\ReqnRoll.ndjson --output .\\LivingDoc.html --title \"Expressium CoffeeShop Report\"");
            }
        }
    }
}
