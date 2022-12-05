namespace Compiler.Binding
{
    internal enum BoundNodeKind
    {
        UnaryExpression,
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        BlockStatement,
        ExpressionStatement
    }
}