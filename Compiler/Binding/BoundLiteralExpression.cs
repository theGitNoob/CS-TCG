using System;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundLiteralExpression class represents a literal expression in the bound tree.
    /// </summary>
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;
        }
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;

        public override Type Type => Value.GetType();

        public object Value { get; }
    }
}