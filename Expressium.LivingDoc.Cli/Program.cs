using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Parsers;
using System;
using System.CommandLine;
using System.Collections.Generic;

namespace Expressium.LivingDoc
{
    public class Program
    {
          static int Main(string[] args)
        {
            var inputOption = CreateInputOption("Input ndjson file(s) or native JSON file");
            var outputOption = CreateOutputOption("Output html file");
            var titleOption = CreateTitleOption("Report title");
            var nativeOption = new Option<bool>(
                name: "--native",
                description: "Use native JSON input mode"
            );

            var rootCommand = new RootCommand("Expressium LivingDoc CLI")
            {
                inputOption,
                outputOption,
                titleOption,
                nativeOption
            };

            rootCommand.SetHandler((inputs, output, title, native) =>
            {
                if (native)
                {
                    if (inputs == null || inputs.Count != 1)
                    {
                        Console.Error.WriteLine("Error: --native requires exactly one --input file.");
                        Environment.Exit(1);
                    }
                    if (!string.IsNullOrEmpty(title))
                    {
                        Console.Error.WriteLine("Error: --native cannot be combined with --title.");
                        Environment.Exit(1);
                    }
                    GenerateNativeReport(inputs[0], output);
                }
                else
                {
                    if (inputs == null || inputs.Count < 1)
                    {
                        Console.Error.WriteLine("Error: At least one --input file is required.");
                        Environment.Exit(1);
                    }
                    if (string.IsNullOrEmpty(title))
                    {
                        Console.Error.WriteLine("Error: --title is required (except with --native).");
                        Environment.Exit(1);
                    }
                    GenerateStandardReport(inputs, output, title);
                }
            }, inputOption, outputOption, titleOption, nativeOption);

            return rootCommand.InvokeAsync(args).Result;
        }
        
        private static void PrintReportStart(string input, string output, string title = null)
        {
            Console.WriteLine("");
            Console.WriteLine("Generating LivingDoc Test Report...");
            Console.WriteLine("Input: " + input);
            Console.WriteLine("Output: " + output);
            if (!string.IsNullOrEmpty(title))
                Console.WriteLine("Title: " + title);
        }

        private static void PrintReportEnd()
        {
            Console.WriteLine("LivingDoc Report generation completed");
            Console.WriteLine("");
        }

        private static Option<List<string>> CreateInputOption(string description)
        {
            var opt = new Option<List<string>>(
                name: "--input",
                description: description
            )
            {
                Arity = ArgumentArity.OneOrMore,
                IsRequired = true
            };
            return opt;
        }

        private static Option<string> CreateOutputOption(string description)
        {
            var opt = new Option<string>(
                name: "--output",
                description: description
            )
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true
            };
            return opt;
        }

        private static Option<string> CreateTitleOption(string description)
        {
            var opt = new Option<string>(
                name: "--title",
                description: description
            )
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true
            };
            return opt;
        }

        private static void GenerateStandardReport(List<string> inputs, string output, string title)
        {
            PrintReportStart(string.Join(", ", inputs), output, title);
            var livingDocConverter = new LivingDocConverter();
            livingDocConverter.Generate(inputs, output, title);
            PrintReportEnd();
        }

        private static void GenerateNativeReport(string input, string output)
        {
            PrintReportStart(input, output);
            var livingDocGenerator = new LivingDocNativeConverter();
            livingDocGenerator.Generate(input, output);
            PrintReportEnd();
        }
    }
}
