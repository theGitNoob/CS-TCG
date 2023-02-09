namespace Compiler.Binding
{
    /// <summary>
    /// The BoundExpressionStatement class represents an expression statement in the bound tree.
    /// </summary>
    internal sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpressionStatement(BoundExpression expression)
        {
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;

        public BoundExpression Expression { get; }
    }
}