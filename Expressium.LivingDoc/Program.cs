using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System;
using System.IO;

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
            else if (args.Length == 3 && args[0] == "--cucumber")
            {
                var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Cucumber.json");
                var livingDocProject = MessagesConvertor.ConvertToLivingDoc(args[1]);
                LivingDocUtilities.SerializeAsJson(outputFilePath, livingDocProject);

                var livingDocGenerator = new LivingDocGenerator(outputFilePath, args[2]);
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
