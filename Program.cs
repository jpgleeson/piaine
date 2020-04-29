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

            Scanner scanner = new Scanner(inputString);

            Parser parser = new Parser(scanner.scanTokens());

            List<string> testLines = new List<string>();
            string[] strings = File.ReadAllLines("test.txt");
            foreach (string s in strings)
            {
                testLines.Add(s);
            }

            PageConsumer pageConsumer = new PageConsumer(testLines);

            

            outputStrings = parser.writeVariablesInSource(inputString, pageConsumer.variablesInPage);
            

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
