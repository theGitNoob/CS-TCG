namespace Compiler.Binding
{
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