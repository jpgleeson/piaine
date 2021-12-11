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
                case '+': link(); break;
                case '|': image(); break;
                case '~': code(); break;
                case '{': quote(); break;
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

        private void link()
        {

            while (peek() != '+' && !isAtEnd())
            {
                advance();
                string debug = subString(start, current);
            }

            string linkURL = subString(start + 1, current);

            start = current;

            advance();

            while(peek() != '+' && !isAtEnd())
            {
                advance();
                string debug = subString(start, current);
            }

            advance();

            string linkText = subString(start+1, current - 1);
            linkText = linkText.Trim();

            outputString += "<a href='" + linkURL + "'>";
            outputString += linkText;
            outputString += "</a>";
        }

        private void image()
        {
            while (peek() != '|' && !isAtEnd())
            {
                advance();
                string debug = subString(start, current);
            }

            string imgSource = subString(start + 1, current);

            start = current;

            advance();

            while (peek() != '|' && !isAtEnd())
            {
                advance();
                string debug = subString(start, current);
            }

            advance();

            string altText = subString(start + 1, current - 1);
            altText = altText.Trim();

            outputString += "<img src='../content/" + imgSource + "' alt='" + altText + "'/>";
        }

        private void code()
        {
            while (peek() != '~' && !isAtEnd())
            {
                advance();
            }

            advance();

            string value = subString(start + 1, current - 1);
            outputString += "<code>" + value + "</code>";
        }

        private void quote()
        {
            while (peek() != '}' && !isAtEnd())
            {
                advance();
            }

            string quoteBody = subString(start + 1, current);

            start = current;

            advance();

            while (peek() != '}' && !isAtEnd())
            {
                advance();
            }

            advance();

            string quoteAttribution = subString(start + 1, current - 1);
            quoteAttribution = quoteAttribution.Trim();

            outputString += "<p class='quoteBody'>" + quoteBody + "</p> <p class='quoteAttribution'>" + quoteAttribution + "</p>";
        }

        private void text()
        {
            while (!textBreak(peek()) && !isAtEnd())
            {
                advance();
            }

            string value = subString(start, current);
            outputString += value;
        }

        public bool textBreak(char check)
        {
            if (check == '\n' || check == '+')
            {
                return true;
            }
            return false;
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
