using System;
using System.Collections.Generic;
using System.IO;

namespace piaine
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputString = readTemplateFile("post.html");
            List<string> outputStrings = new List<string>();
            List<string> inputLines = new List<string>();

            Scanner scanner = new Scanner(inputString);

            List<string> sourceDirectory = new List<string>(Directory.GetFiles(Directory.GetCurrentDirectory() + "/posts/"));

            StreamWriter[] files = new StreamWriter[sourceDirectory.Count];
            List<Post> posts = new List<Post>();

            //For some reason parser needs to be used outside of the foreach scope for the source files. I've no idea why, but this works right now.
            Parser parser = new Parser(scanner.scanTokens());

            int i = 0;

            foreach(string s in sourceDirectory)
            {
                //Strip each one to get just the local path in the posts folder i.e. "test.txt"
                //From each of these a html file will be created with the same name in a folder.
                //These html files will then be used to build the index
                outputStrings.Clear();

                Post post = new Post();
                post.path = Path.GetRelativePath("output", "output/posts/" + Path.GetFileNameWithoutExtension(s) + ".html");
                post.date = DateTime.Today;

                inputLines.Clear();
                string[] strings = File.ReadAllLines(s);
                foreach (string st in strings)
                {
                    inputLines.Add(st);
                }

                PageConsumer pageConsumer = new PageConsumer(inputLines);

                post.name = pageConsumer.getPageTitle();



                outputStrings = parser.writeVariablesInSource(inputString, pageConsumer.variablesInPage);

                var outputFile = File.Create("output/posts/" + Path.GetFileNameWithoutExtension(s) + ".html");
                files[i] = new StreamWriter(outputFile);

                foreach (string st in outputStrings)
                {
                    files[i].WriteLine(st);
                }

                files[i].Flush();

                Console.WriteLine("{0} written.", Path.GetFileNameWithoutExtension(s));

                posts.Add(post);

                i++;
            }

            inputString = readTemplateFile("index.html");
            scanner = new Scanner(inputString);
            parser = new Parser(scanner.scanTokens());
            outputStrings.Clear();
            var indexFile = File.Create("output/index.html");
            
            //Make an index here.
            outputStrings = parser.writeVariablesInSource(inputString, posts);

            StreamWriter indexWriter = new StreamWriter(indexFile);

            foreach (string st in outputStrings)
            {
                indexWriter.WriteLine(st);
            }

            indexWriter.Flush();

            Console.WriteLine("Index written.");

            Console.ReadKey();
        }

        static string readTemplateFile(string path)
        {
            string holderString = File.ReadAllText(path);

            return holderString;
        }
    }
}
