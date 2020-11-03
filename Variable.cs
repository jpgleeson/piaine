using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public class Variable
    {
        public string name;
        public string literal;

        public Variable()
        {
            
        }

        public Variable(string desName, string desLiteral)
        {
            name = desName;
            literal = desLiteral;
        }
    }

    public class TagVariable : Variable
    {
        public List<string> items;

        public TagVariable(string desName, List<string> desList)
        {
            name = desName;
            items = desList;
        }
    }
}
