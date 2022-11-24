namespace Compiler.Syntax
{
    public enum SyntaxKind
    {
        BangEqualsToken,
        EqualsEqualsToken,
        PipePipeToken,
        AmpersandAmpersandToken,
        BangToken,
        IndentifierToken,
        FalseKeyword,
        TrueKeyword,
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,
        LiteralExpression,
        BinaryExpression,
        ParenthesisExpression,
        UnaryExpression
    }
}