using System;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundUnaryExpression class represents a unary expression in the bound tree.
    /// </summary>
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator op,BoundExpression operand)
        {
            Op = op;
            Operand = operand;
        }
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Op.ResultType; 
        public BoundUnaryOperator Op{get;}
        public BoundExpression Operand{get;}
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Operand;
        }
    }
}