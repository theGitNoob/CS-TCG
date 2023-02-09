using System;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundVariableExpression class represents a variable expression in the bound tree.
    /// </summary>
    internal sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        public VariableSymbol Variable { get; }

        public override Type Type => Variable.Type;

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
    }
}