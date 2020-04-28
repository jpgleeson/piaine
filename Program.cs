using System;
using System.Collections.Generic;
using System.IO;

namespace piaine
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputString = readTemplateFile();
            List<string> outputStrings = new List<string>();
            string checkThis = "<html>{ !thjehh }</html>";

            Scanner scanner = new Scanner(inputString);

            Parser parser = new Parser(scanner.scanTokens());

            outputStrings = parser.writeVariablesInSource(checkThis);

            Console.WriteLine(checkThis);

            var outputFile = File.Create("test.html");
            StreamWriter fileWriter = new StreamWriter(outputFile);

            foreach (string s in outputStrings)
            {
                fileWriter.WriteLine(s);
            }

            fileWriter.Dispose();

            Console.ReadKey();
        }

        static string readTemplateFile()
        {
            string holderString = File.ReadAllText("holdingThis.html");

            return holderString;
        }
    }
}
