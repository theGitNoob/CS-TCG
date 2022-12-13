namespace Compiler.Binding
{
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