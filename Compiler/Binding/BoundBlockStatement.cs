namespace Compiler.Binding
{
    /// <summary>
    /// The BoundBlockStatement class represents a block statement in the bound tree.
    /// </summary>
    internal sealed class BoundBlockStatement : BoundStatement
    {
        public BoundBlockStatement(IEnumerable<BoundStatement> statements)
        {
            Statements = statements;
        }

        public IEnumerable<BoundStatement> Statements { get; }

        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;
    }
}