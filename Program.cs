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
            bool buildTagIndexes = false;

            Scanner scanner = new Scanner(inputString);

            if (!Directory.Exists("posts"))
            {
                Console.WriteLine("Posts directory does not exist. Creating...");
                Directory.CreateDirectory("posts");
            }
            List<string> sourceDirectory = new List<string>(Directory.GetFiles(Directory.GetCurrentDirectory() + "/posts/"));

            StreamWriter[] files = new StreamWriter[sourceDirectory.Count];
            List<Post> posts = new List<Post>();
            List<string> tags = new List<string>();

            //For some reason parser needs to be used outside of the foreach scope for the source files. I've no idea why, but this works right now.
            Parser parser = new Parser(scanner.scanTokens());

            foreach (string argument in args)
            {
                if (argument == "-tags")
                {
                    buildTagIndexes = true;
                }
            }

            int i = 0;

            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }
            if (!Directory.Exists("output/posts"))
            {
                Directory.CreateDirectory("output/posts");
            }

            foreach (string s in sourceDirectory)
            {
                //Strip each one to get just the local path in the posts folder i.e. "test.txt"
                //From each of these a html file will be created with the same name in a folder.
                //These html files will then be used to build the index
                outputStrings.Clear();

                Post post = new Post();
                post.path = Path.GetRelativePath("output", "output/posts/" + Path.GetFileNameWithoutExtension(s).Replace(' ', '_') + ".html");
                post.date = DateTime.Today;
                post.tags = new List<string>();

                bool skip = false;

                inputLines.Clear();
                string[] strings = File.ReadAllLines(s);
                foreach (string st in strings)
                {
                    inputLines.Add(st);
                }

                PageConsumer pageConsumer = new PageConsumer(inputLines);

                post.name = pageConsumer.getPageTitle();
                post.date = pageConsumer.getPageDate();
                post.tags = pageConsumer.getPageTags();


                if (pageConsumer.getPageTemplate() != null)
                {
                    if (post.typeOfPage != pageType.post)
                    {
                        post.typeOfPage = pageType.staticpage;
                        if (File.Exists(pageConsumer.getPageTemplate()))
                        {
                            inputString = readTemplateFile(pageConsumer.getPageTemplate());
                            scanner.refreshSource(inputString);
                            parser.refreshTokens(scanner.scanTokens());
                        }
                        else
                        {
                            Console.WriteLine("Template {0} does not exist. Exiting.", pageConsumer.getPageTemplate());
                            skip = true;
                        }
                    }
                }
                else
                {
                    inputString = readTemplateFile("post.html");
                    scanner.refreshSource(inputString);
                    parser.refreshTokens(scanner.scanTokens());
                    post.typeOfPage = pageType.post;
                }

                if (!skip)
                {
                    if (post.tags != null)
                    {
                        foreach (string tag in post.tags)
                        {
                            if (!tags.Contains(tag))
                            {
                                tags.Add(tag);
                            }
                        }
                    }

                    outputStrings = parser.writeVariablesInSource(inputString, pageConsumer.variablesInPage);

                    if (post.typeOfPage == pageType.post)
                    {
                        var outputFile = File.Create("output/posts/" + Path.GetFileNameWithoutExtension(s).Replace(' ', '_') + ".html");
                        files[i] = new StreamWriter(outputFile);
                    }
                    else
                    {
                        var outputFile = File.Create("output/" + Path.GetFileNameWithoutExtension(s).Replace(' ', '_') + ".html");
                        files[i] = new StreamWriter(outputFile);
                    }



                    foreach (string st in outputStrings)
                    {
                        files[i].WriteLine(st);
                    }

                    files[i].Flush();

                    Console.WriteLine("{0} written.", Path.GetFileNameWithoutExtension(s).Replace(' ', '_'));

                    posts.Add(post);
                }
                

                i++;
            }

            

            if (buildTagIndexes)
            {
                buildTagFiles(posts, tags);
                buildIndexFile(posts, tags);
            }
            else
            {
                buildIndexFile(posts);
            }

            buildAtomFile(posts);

            Console.WriteLine("Files generated. Press any key to exit.");

            Console.ReadKey();
        }

        static void buildIndexFile(List<Post> posts, List<string> tagCloud = null)
        {
            if (File.Exists("index.html"))
            {
                string inputString = readTemplateFile("index.html");
                Scanner scanner = new Scanner(inputString);
                Parser parser = new Parser(scanner.scanTokens());
                List<string> outputStrings = new List<string>();
                var indexFile = File.Create("output/index.html");

                posts.Sort((x, y) => x.date.CompareTo(y.date));

                posts.Reverse();

                List<Post> justPosts = new List<Post>();

                foreach (Post p in posts)
                {
                    if (p.typeOfPage == pageType.post)
                    {
                        justPosts.Add(p);
                    }
                }

                //Make an index here.
                if (tagCloud != null)
                {
                    outputStrings = parser.writeVariablesInSource(inputString, justPosts, tagCloud);
                }
                else
                {
                    outputStrings = parser.writeVariablesInSource(inputString, justPosts);
                }

                StreamWriter indexWriter = new StreamWriter(indexFile);

                foreach (string st in outputStrings)
                {
                    indexWriter.WriteLine(st);
                }

                indexWriter.Flush();

                Console.WriteLine("Index written.");
            }
            else
            {
                Console.WriteLine("No index template. Exiting.");
            }
        }

        static string readTemplateFile(string path)
        {
            string holderString = File.ReadAllText(path);
            return holderString;
        }

        static void buildAtomFile(List<Post> posts)
        {
            if (File.Exists("atom.xml"))
            {
                string inputString = readTemplateFile("atom.xml");
                Scanner scanner = new Scanner(inputString);
                Parser parser = new Parser(scanner.scanTokens());
                List<string> outputStrings = new List<string>();
                var atomFile = File.Create("output/feed.atom.xml");

                posts.Sort((x, y) => x.date.CompareTo(y.date));

                posts.Reverse();

                //Make an index here.
                outputStrings = parser.writeAtomFeed(inputString, posts);

                StreamWriter atomWriter = new StreamWriter(atomFile);

                foreach (string st in outputStrings)
                {
                    atomWriter.WriteLine(st);
                }

                atomWriter.Flush();

                Console.WriteLine("Atom feed written.");
            }
            else
            {
                Console.WriteLine("No atom template. Exiting.");
            }
        }

        static void buildTagFiles(List<Post> posts, List<string> tags)
        {
            if (File.Exists("tagIndex.html"))
            {
                string inputString = readTemplateFile("tagIndex.html");
                Scanner scanner = new Scanner(inputString);
                Parser parser = new Parser(scanner.scanTokens());
                List<string> outputStrings = new List<string>();

                foreach (string tag in tags)
                {
                    Console.WriteLine("{0} index being built.", tag);

                    string filePath = "output/tags/" + tag + ".html";
                    if (!Directory.Exists("output/tags/"))
                    {
                        Directory.CreateDirectory("output/tags/");
                    }
                    var tagIndex = File.Create(filePath);

                    List<Post> taggedPosts = new List<Post>();

                    posts.Sort((x, y) => x.date.CompareTo(y.date));

                    posts.Reverse();

                    foreach (Post p in posts)
                    {
                        if (p.typeOfPage == pageType.post)
                        {
                            if (p.tags != null)
                            {
                                if (p.tags.Contains(tag))
                                {
                                    taggedPosts.Add(p);
                                }
                            }
                        }
                    }

                    outputStrings = parser.writeVariablesInSource(inputString, taggedPosts, tag);

                    StreamWriter tagIndexWriter = new StreamWriter(tagIndex);

                    foreach (string st in outputStrings)
                    {
                        tagIndexWriter.WriteLine(st);
                    }

                    tagIndexWriter.Flush();
                }

                Console.WriteLine("{0} tag indices written.", tags.Count.ToString());
            }
            else
            {
                Console.WriteLine("No tag index template. Exiting.");
            }
        }
    }
}
