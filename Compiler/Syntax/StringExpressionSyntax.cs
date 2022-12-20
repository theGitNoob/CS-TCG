namespace Compiler.Syntax
{
    public sealed class StringExpressionSyntax : ExpressionSyntax
    {
        public StringExpressionSyntax(SyntaxToken openQuotes, SyntaxToken stringToken, SyntaxToken closeQuotes)
        {
            OpenQuotes = openQuotes;
            StringToken = stringToken;
            CloseQuotes = closeQuotes;
        }

        public SyntaxToken OpenQuotes { get; }
        public SyntaxToken StringToken { get; }
        public SyntaxToken CloseQuotes { get; }

        public override SyntaxKind Kind => SyntaxKind.StringExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenQuotes;
            yield return StringToken;
            yield return CloseQuotes;
        }
    }
}