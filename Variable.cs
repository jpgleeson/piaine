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

    public class TagCollectionVariable : Variable
    {
        public List<string> literalList;

        public TagCollectionVariable()
        {

        }
        public TagCollectionVariable(string desName, List<string> desList)
        {
            name = desName;
            literalList = desList;
        }
    }
}
