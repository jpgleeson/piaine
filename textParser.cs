using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public class textParser
    {
        int current = 0;
        int start;
        string sourceString;
        int length;
        string outputString;

        public textParser()
        {

        }

        public string parseString(string stringToParse)
        {
            outputString = "";
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
            char nextCharacter = advance();

            switch (nextCharacter)
            {
                case '\n': newLine(); break;
                case '#': header(); break;
                default:
                    text();
                    break;
            }
        }

        private void header()
        {
            while (peek() != '\n' && !isAtEnd())
            {
                advance();
            }

            string value = subString(start + 1, current);
            outputString += "<h2>";
            outputString += value;
            outputString += "</h2>";
        }

        private void newLine()
        {
            outputString += "</p>";
            outputString += "<p>";
        }

        private void text()
        {
            while (peek() != '\n' && !isAtEnd())
            {
                advance();
            }
         
            

            string value = subString(start, current);
            outputString += value;
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

        private char peek()
        {
            if (current >= sourceString.Length)
            {
                return '\0';
            }

            return sourceString[current];
        }

        private string subString(int start, int end)
        {
            int length = end - start;
            return sourceString.Substring(start, length);
        }
    }
}
