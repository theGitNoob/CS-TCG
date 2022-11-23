namespace Compiler.Syntax
{
    public sealed class LiteralExpression : ExpressionSyntax
    {
        public LiteralExpression(SyntaxToken literalToken)
        {
            LiteralToken = literalToken;
        }
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

        public SyntaxToken LiteralToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}