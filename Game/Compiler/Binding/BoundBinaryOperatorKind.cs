namespace Compiler.Binding
{
    /// <summary>
    /// The BoundBinaryOperatorKind enum represents the kind of binary operator.
    /// </summary>
    internal enum BoundBinaryOperatorKind
    {
        /// <summary>
        /// Binary operator
        /// </summary>
        Addition,
        Subtraction,
        Multiplication,
        Division,
        LogicalAnd,
        LogicalOr,
        Equals,
        NotEquals,
        LessOrEquals,
        Less,
        Great,
        GreaterOrEquals
    }
}