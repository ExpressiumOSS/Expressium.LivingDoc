using System;
using System.IO;

namespace Expressium.LivingDoc.Cli
{
    /// <summary>
    /// This program file is provided as an example of how to create a custom CLI for Expressium LivingDoc.
    /// The associated batch files in this repository demonstrate how to use the CLI to generate LivingDoc test reports.
    /// It is not included in the distributed NuGet packages and may change in future releases.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 7 && args[0] == "--generate")
            {
                // Generating a LivingDoc Test Report based on a Cucumber Messages NDJSON file...
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("Input: " + args[2]);
                Console.WriteLine("Output: " + args[4]);
                Console.WriteLine("Title: " + args[6]);

                var livingDocConverter = new LivingDocConverter();
                var livingDocProject = livingDocConverter.Convert(args[2], args[6]);
                livingDocConverter.Generate(livingDocProject, args[4]);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else if (args.Length == 5 && args[0] == "--native")
            {
                // Generating a LivingDoc Test Report based on a Native JSON file...                
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("Input: " + args[2]);
                Console.WriteLine("Output: " + args[4]);

                var livingDocConverter = new LivingDocConverter();
                var livingDocProject = livingDocConverter.Import(args[2]);
                livingDocConverter.Generate(livingDocProject, args[4]);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else if (args.Length == 7 && args[0] == "--custom")
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
            else if (args.Length == 8 && args[0] == "--merge")
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
            else if (args.Length == 8 && args[0] == "--history")
            {
                // Generating a LivingDoc Test Report with History based on a Cucumber Messages NDJSON file...
                Console.WriteLine("");
                Console.WriteLine("Generating LivingDoc Test Report...");
                Console.WriteLine("Input: " + args[2]);
                Console.WriteLine("History: " + args[3]);
                Console.WriteLine("Output: " + args[5]);
                Console.WriteLine("Title: " + args[7]);

                var livingDocConverter = new LivingDocConverter();
                var livingDocProject = livingDocConverter.Convert(args[2], args[7]);

                var historyDirectory = Path.GetDirectoryName(Path.GetFullPath(args[3]));
                if (!Directory.Exists(historyDirectory))
                    Directory.CreateDirectory(historyDirectory);

                var historyFileName = Path.Combine(historyDirectory, livingDocProject.Date.ToString("yyyyMMddHHmmss") + ".ndjson");
                File.Copy(args[2], historyFileName, true);

                livingDocConverter.MergeHistory(livingDocProject, args[3]);
                livingDocConverter.Generate(livingDocProject, args[5]);

                Console.WriteLine("Generating LivingDoc Report Completed");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Examples...");
                Console.WriteLine("Expressium.LivingDoc.Cli.exe --generate --input [INPUTPATH] --output [OUTPUTPATH] --title [TITLE]");
                Console.WriteLine("Expressium.LivingDoc.Cli.exe --native --input [INPUTPATH] --output [OUTPUTPATH]");
                Console.WriteLine("Expressium.LivingDoc.Cli.exe --custom --input [INPUTPATH] --output [OUTPUTPATH] --title [TITLE]");
                Console.WriteLine("Expressium.LivingDoc.Cli.exe --merge --input [INPUTPATHMASTER] [INPUTPATHSLAVE] --output [OUTPUTPATH] --title [TITLE]");
                Console.WriteLine("Expressium.LivingDoc.Cli.exe --history --input [INPUTPATH] [HISTORYPATH] --output [OUTPUTPATH] --title [TITLE]\r\n");
            }
        }
    }
}