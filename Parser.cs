using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public class Parser
    {
        List<Token> tokens;
        List<string> outputStrings;

        public Parser(List<Token> desTokens)
        {
            tokens = desTokens;
            outputStrings = new List<string>();
        }

        public List<string> writeVariablesInSource(string source)
        {
            foreach (Token t in tokens)
            {
                if (t.type == TokenType.Unscoped)
                {
                    outputStrings.Add(t.literal.ToString());
                }
                else if (t.type == TokenType.Variable)
                {
                    Console.WriteLine(t.literal.ToString());
                    outputStrings.Add(t.literal.ToString());
                }
            }

            return outputStrings;
        }


        /// <summary>
        /// Use this to change a variable from the token list to the string of the variable from the source file.
        /// </summary>
        /// <param name="tokenToWrite"></param>
        /// <returns></returns>
        public string writeVariable(Token tokenToWrite)
        {
            Console.WriteLine(tokenToWrite.lexeme);
            return tokenToWrite.literal.ToString();
        }
    }
}
