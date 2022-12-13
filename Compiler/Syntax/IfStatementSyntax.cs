namespace Compiler.Syntax
{
    public sealed class IfStatementSyntax : StatementSyntax
    {
        public IfStatementSyntax(SyntaxToken ifToken,ParenthesisExpressionSyntax parenthesisExpression, 
        BlockStatementSyntax blockIfStatement, ElseClauseSyntax elseClauseStatement)
        {
            IfToken = ifToken;
            ParenthesisExpression = parenthesisExpression;
            BlockIfStatement = blockIfStatement;
            ElseClauseStatement = elseClauseStatement;
        }

        public SyntaxToken IfToken { get; }
        public ParenthesisExpressionSyntax ParenthesisExpression { get; }
        public BlockStatementSyntax BlockIfStatement { get; }
        public ElseClauseSyntax ElseClauseStatement { get; }

        public override SyntaxKind Kind => SyntaxKind.IfStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IfToken;
            yield return ParenthesisExpression;
            yield return BlockIfStatement;
            yield return ElseClauseStatement;
        }
    }
}