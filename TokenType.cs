using System;
using System.Collections.Generic;
using System.Text;

namespace piaine
{
    public enum TokenType
    {
        /// <summary>
        /// No token defined.
        /// </summary>
        Undefined,
        /// <summary>
        /// {
        /// Variables will be enclosed in parenthesis. This is the opening of a parentesi
        /// </summary>
        LeftParenthesis,
        /// <summary>
        /// }
        /// </summary>
        RightParenthesis,
        /// <summary>
        /// !
        /// I am using bangs to identify variables
        /// </summary>
        Variable,
        /// <summary>
        /// This is not the best way to do this.
        /// </summary>
        foreachLoop,
        /// <summary>
        /// For html not in scope of the processing.
        /// </summary>
        Unscoped,
        /// <summary>
        /// End of file token.
        /// </summary>
        EndOfFile
    }
}
