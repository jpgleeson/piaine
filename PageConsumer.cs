using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public class PageConsumer
    {
        public List<Variable> variablesInPage;
        public PageConsumer(List<string> inputLines)
        {
            variablesInPage = new List<Variable>();

            foreach (string s in inputLines)
            {
                string[] splitString = s.Split(':');
                Variable v = new Variable(splitString[0], splitString[1]);
                v.name = v.name.Trim();
                v.literal = v.literal.Trim();
                variablesInPage.Add(v);
            }

            foreach (Variable v in variablesInPage)
            {
                if (v.name == "text")
                {
                    //use markdown parser to output html that will be saved as the literal.
                }
            }
        }
    }
}
