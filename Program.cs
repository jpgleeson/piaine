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
            testLines.Add("title: Running it back");
            testLines.Add("text:This is a test to see if the lines will extend and how long it will take before the line gets cut off and what happens then because it would be nice if it was all handled gracefully right now but really who knows.{ 1paraBreak } Do newlines work as it is right now? If so that would be useful.");
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
