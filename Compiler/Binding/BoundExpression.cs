using System;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundExpression class represents an expression in the bound tree.
    /// </summary>
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type {get;}
    }
}