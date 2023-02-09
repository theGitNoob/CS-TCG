namespace Compiler.Binding
{
    /// <summary>
    /// The BoundNodeKind enum represents the kind of node in the bound tree.
    /// </summary>
    internal abstract class BoundNode
    {
        public abstract BoundNodeKind Kind{get;}
    }
}