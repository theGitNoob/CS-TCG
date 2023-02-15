namespace Compiler.Binding
{
    /// <summary>
    /// The BoundUnaryOperatorKind enum represents the kind of unary operator in the bound tree.
    /// </summary>
    internal enum BoundUnaryOperatorKind
    {
        Identity,
        Negation,
        LogicalNegation
    }
}