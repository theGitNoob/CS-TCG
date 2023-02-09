namespace Compiler.Binding
{
    /// <summary>
    /// The BoundIfStatement class represents an if statement in the bound tree.
    /// </summary>
    internal sealed class BoundIfStatement : BoundStatement
    {
        public BoundIfStatement(BoundExpression condition, BoundStatement ifStatement, BoundStatement elseStatement)
        {
            Condition = condition;
            IfStatement = ifStatement;
            ElseStatement = elseStatement;
        }

        public BoundExpression Condition { get; }
        public BoundStatement IfStatement { get; }
        public BoundStatement ElseStatement { get; }

        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
    }
}