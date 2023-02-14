using System;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundAssignment class represents an assignment expression in the bound tree.
    /// </summary>
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

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Expression;
        }
    }
}