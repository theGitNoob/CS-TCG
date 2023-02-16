namespace Compiler.Syntax
{
    /// <summary>
    /// The UnaryExpression class represents a unary expression in the syntax tree.
    /// </summary>
    public sealed class UnaryExpression : ExpressionSyntax
    {
        public UnaryExpression(SyntaxToken operatorToken, ExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Operand { get; }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}