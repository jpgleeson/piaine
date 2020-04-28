using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public class Variable
    {
        public string name;
        public string literal;

        public Variable(string desName, string desLiteral)
        {
            name = desName;
            literal = desLiteral;
        }
    }
}
