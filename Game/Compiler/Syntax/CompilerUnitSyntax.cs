namespace Compiler.Syntax
{
    /// <summary>
    /// The CompilerUnitSyntax class represents the root of the syntax tree.
    /// </summary>
    public sealed class CompilerUnitSyntax : SyntaxNode
    {
        public CompilerUnitSyntax(StatementSyntax statement, SyntaxToken endOfFileToken)
        {
            Statement = statement;
            EndOfFileToken = endOfFileToken;
        }

        public StatementSyntax Statement { get; }
        public SyntaxToken EndOfFileToken { get; }

        public override SyntaxKind Kind => SyntaxKind.CompilerUnit;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Statement;
            yield return EndOfFileToken;
        }
    }
}