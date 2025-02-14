using Expressium.TestExecutionReport;
using System.IO;

namespace Expressium.CucumberMessages
{
    public class Program
    {
        static void Main(string[] args)
        {

            var inputFile = Path.Combine(Directory.GetCurrentDirectory(), "Example.json");
            var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "ExampleOutput.json");
            CucumberConvertor.SaveAsTestExecution(inputFile, outputFile);

            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "TestReport");
            var livingDocGenerator = new LivingDocGenerator(outputFile, outputPath);
            livingDocGenerator.Execute();

            //{
            //    Console.WriteLine("Expressium.TestExecutionReport.exe [FILEPATH] [OUTPUTPATH]");
            //    Console.WriteLine("Expressium.TestExecutionReport.exe C:\\SourceCode\\company-project-tests\\TestExecution.json C:\\SourceCode\\company-project-tests\\TestReport");
            //}
        }
    }
}
