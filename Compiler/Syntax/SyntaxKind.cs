namespace Compiler.Syntax
{
    /// <summary>
    /// The SyntaxKind enum represents the kind of syntax node in the syntax tree.
    /// </summary>
    public enum SyntaxKind
    {
        //Expression
        StringExpression,
        LiteralExpression,
        BinaryExpression,
        ParenthesisExpression,
        UnaryExpression,
        NameExpression,
        AssignmentExpression,

        //Statement
        ExpressionStatement,
        ElseStatement,
        IfStatement,
        BlockStatement,

        //BinaryOperator
        BangEqualsToken,
        EqualsEqualsToken,
        PipePipeToken,
        AmpersandAmpersandToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        GreatToken,
        GreaterOrEqualsToken,
        LessToken,
        LessOrEqualsToken,

        //UnaryOperator
        BangToken,

        //KeywordToken
        FalseKeyword,
        TrueKeyword,
        IfKeyword,
        ElseKeyword,

        //OtherToken
        IndentifierToken,
        NumberToken,
        WhiteSpaceToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,
        EqualsToken,
        OpenBraceToken,
        CloseBraceToken,
        CompilerUnit,
        QuotesToken,
        StringToken,
        DotToken,
        QuestionToken,
        CommaToken,
        SemicolonToken
    }
}