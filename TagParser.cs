using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public class TagParser
    {
        int current = 0;
        int start;
        string sourceString;
        int length;
        List<string> tags;

        public TagParser()
        {
            tags = new List<string>();
        }

        public List<string> parseString(string stringToParse)
        {
            sourceString = stringToParse;

            current = 0;
            length = stringToParse.Length;

            tags = new List<string>();

            while (!isAtEnd())
            {
                start = current;
                scanToken();
            }

            return tags;
        }

        private void scanToken()
        {
            char nextCharacter = advance();

            switch (nextCharacter)
            {
                case ',': newTag(); break;
                default:
                    tag();
                    break;
            }
        }

        private void newTag()
        {
            advance();
        }

        private void tag()
        {
            while (peek() != ',' && !isAtEnd())
            {
                advance();
            }

            string value = subString(start, current);
            tags.Add(value);
            Console.WriteLine(value);
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
