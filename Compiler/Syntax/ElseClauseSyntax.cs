namespace Compiler.Syntax
{
    public sealed class ElseClauseSyntax : ExpressionSyntax
    {
        public ElseClauseSyntax(SyntaxToken elseClause, BlockStatementSyntax elseStatement)
        {
            ElseClause = elseClause;
            ElseStatement = elseStatement;
        }

        public SyntaxToken ElseClause { get; }
        public BlockStatementSyntax ElseStatement { get; }

        public override SyntaxKind Kind => SyntaxKind.ElseStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ElseClause;
            yield return ElseStatement;
        }
    }
}