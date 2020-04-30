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
            List<string> inputLines = new List<string>();

            Scanner scanner = new Scanner(inputString);

            List<string> sourceDirectory = new List<string>(Directory.GetFiles(Directory.GetCurrentDirectory() + "/posts/"));

            StreamWriter[] files = new StreamWriter[sourceDirectory.Count];

            //For some reason parser needs to be used outside of the foreach scope for the source files. I've no idea why, but this works right now.
            Parser parser = new Parser(scanner.scanTokens());

            int i = 0;

            foreach(string s in sourceDirectory)
            {
                //Strip each one to get just the local path in the posts folder i.e. "test.txt"
                //From each of these a html file will be created with the same name in a folder.
                //These html files will then be used to build the index
                outputStrings.Clear();

                

                inputLines.Clear();
                string[] strings = File.ReadAllLines(s);
                foreach (string st in strings)
                {
                    inputLines.Add(st);
                }

                PageConsumer pageConsumer = new PageConsumer(inputLines);



                outputStrings = parser.writeVariablesInSource(inputString, pageConsumer.variablesInPage);

                var outputFile = File.Create("output/" + Path.GetFileNameWithoutExtension(s) + ".html");
                files[i] = new StreamWriter(outputFile);

                foreach (string st in outputStrings)
                {
                    files[i].WriteLine(st);
                }

                files[i].Flush();

                Console.WriteLine("{0} written.", Path.GetFileNameWithoutExtension(s));
                i++;
            }
            //Console.WriteLine(sourceDirectory);
            /*
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

            fileWriter.Dispose();*/

            Console.ReadKey();
        }

        static string readTemplateFile()
        {
            string holderString = File.ReadAllText("holdingThis.html");

            return holderString;
        }
    }
}
