namespace Compiler.Syntax
{
    /// <summary>
    /// The BlockStatementSyntax class represents a block statement in the syntax tree.
    /// </summary>
    public sealed class BlockStatementSyntax : StatementSyntax
    {
        public BlockStatementSyntax(SyntaxToken openBraceToken, IEnumerable<StatementSyntax> statements, SyntaxToken closeBraceToken)
        {
            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;
        }

        public SyntaxToken OpenBraceToken { get; }
        public IEnumerable<StatementSyntax> Statements { get; }
        public SyntaxToken CloseBraceToken { get; }

        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenBraceToken;
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return CloseBraceToken;
        }
    }
}