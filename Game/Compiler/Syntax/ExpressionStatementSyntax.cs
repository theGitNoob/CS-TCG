namespace Compiler.Syntax
{
    /// <summary>
    /// The ExpressionStatementSyntax class represents an expression statement in the syntax tree.
    /// </summary>
    public sealed class ExpressionStatementSyntax : StatementSyntax
    {
        public ExpressionStatementSyntax(ExpressionSyntax expression)
        {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
        }
    }
}