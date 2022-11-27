using System;

namespace Compiler.Binding
{
    internal sealed class BoundAssignment : BoundExpression
    {
        public BoundAssignment(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;

        public override Type Type => Expression.Type;
    }
}