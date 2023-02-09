namespace Compiler.Binding
{
    /// <summary>
    /// The BoundNodeKind enum represents the kind of node in the bound tree.
    /// </summary>
    internal enum BoundNodeKind
    {
        // Expressions
        UnaryExpression,
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
      
      
        // Statements
        BlockStatement,
        ExpressionStatement,
        IfStatement
    }
}