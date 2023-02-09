using System;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundBinaryExpression class represents a binary expression in the bound tree.
    /// </summary>
    
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left,BoundBinaryOperator op,BoundExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public BoundExpression Left { get; }
        public BoundBinaryOperator Op { get; }
        public BoundExpression Right { get; }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;

        public override Type Type => Op.ResultType;
    }
}