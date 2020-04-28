using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public class MarkdownParser
    {
        int current = 0;
        int start;
        string sourceString;
        int length;
        public MarkdownParser()
        {

        }

        public string parseString(string stringToParse)
        {
            string outputString = "";
            sourceString = stringToParse;

            current = 0;
            length = stringToParse.Length;

            List<string> tokens = new List<string>();

            while (!isAtEnd())
            {
                start = current;
                scanToken();
            }

            outputString += "</p>";

            return outputString;
        }

        private void scanToken()
        {
            char nextCharacter = Advance();

            switch (nextCharacter)
            {
                case '\n': newLine(); break;
                case '#': header(); break;
                default:
                    paragraph()
                    break;
            }
        }

        public bool isAtEnd()
        {
            if (current >= sourceString.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private char advance()
        {
            current++;
            return sourceString[current - 1];
        }
    }
}
