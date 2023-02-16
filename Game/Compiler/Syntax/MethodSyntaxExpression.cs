namespace Compiler.Syntax
{
    public sealed class MethodSyntaxExpression : ExpressionSyntax
    {
        public MethodSyntaxExpression(SyntaxToken identifierToken, SyntaxToken openParenthesisToken,
        ExpressionSyntax cantToken, SyntaxToken commaToken, SyntaxToken variable, SyntaxToken closeParenthesisToken)
        {
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            CantToken = cantToken;
            CommaToken = commaToken;
            Variable = variable;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax CantToken { get; }
        public SyntaxToken CommaToken { get; }
        public SyntaxToken Variable { get; }
        public SyntaxToken CloseParenthesisToken { get; }

        public override SyntaxKind Kind => SyntaxKind.MethodSyntaxExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return OpenParenthesisToken;
            yield return CantToken;
            yield return CommaToken;
            yield return CloseParenthesisToken;
        }
    }
}