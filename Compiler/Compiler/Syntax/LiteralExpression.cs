namespace Compiler.Syntax
{
    public sealed class LiteralExpression : ExpressionSyntax
    {
        public LiteralExpression(SyntaxToken literalToken)
            : this(literalToken,literalToken.Value)
        {
            
        }
        public LiteralExpression(SyntaxToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
        }
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

        public SyntaxToken LiteralToken { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}