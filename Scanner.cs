using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace piaine
{
    public class Scanner
    {
        List<Token> tokens;
        string source;
        int start;
        int current;
        int line;

        public Scanner(string desSource)
        {
            tokens = new List<Token>();
            source = desSource;
            start = 0;
            current = 0;
            line = 0;
        }

        public bool isAtEnd()
        {
            if (current >= source.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Token> scanTokens()
        {
            tokens = new List<Token>();

            while(!isAtEnd())
            {
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EndOfFile, "", null, line));

            return tokens;
        }

        private void scanToken()
        {
            char nextCharacter = Advance();

            switch(nextCharacter)
            {
                case '{': addToken(TokenType.LeftParenthesis); break;
                case '}': addToken(TokenType.RightParenthesis); break;
                case '!': variable(); break;
                default:
                    if (tokens.Count > 0)
                    {
                        if (tokens[tokens.Count - 1].type != TokenType.LeftParenthesis)
                        {
                            notInScope();
                        }
                    }
                    else
                    {
                        notInScope();
                    }
                    break;
            }
        }

        private void addToken(TokenType tokenToAdd)
        {
            addToken(tokenToAdd, null);
        }

        private void variable()
        {
            while (peek() != ' ' && !isAtEnd())
            {
                Advance();
            }

            Advance();

            string value = subString(start + 1, current - 1);
            addToken(TokenType.Variable, value);
        }

        private void notInScope()
        {
            while (peek() != '{'  && !isAtEnd())
            {
                Advance();
            }

            string value = subString(start, current);
            addToken(TokenType.Unscoped, value);
        }

        private void addToken(TokenType tokenToAdd, object desLiteral)
        {
            string text = subString(start, current);
            tokens.Add(new Token(tokenToAdd, text, desLiteral, line));
        }

        private char Advance()
        {
            current++;
            return source[current - 1];
        }

        private char peek()
        {
            if (current >= source.Length)
            {
                return '\0';
            }

            return source[current];
        }

        private string subString(int start, int end)
        {
            int length = end - start;
            return source.Substring(start, length);
        }
    }
}
