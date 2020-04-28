namespace piaine
{
    public struct Token
    {
        public TokenType type;
        public string lexeme;
        public object literal;
        public int line;

        public Token(TokenType desType, string desLexeme, object desLiteral, int desLine)
        {
            type = desType;
            lexeme = desLexeme;
            literal = desLiteral;
            line = desLine;
        }
    }
}
