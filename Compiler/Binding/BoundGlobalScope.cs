namespace Compiler.Binding
{
    /// <summary>
    /// The BoundGlobalScope class represents a global scope in the bound tree.
    /// </summary>
    internal sealed class BoundGlobalScope
    {
        public BoundGlobalScope(BoundGlobalScope previous, IEnumerable<Diagnostic> diagnostics, IEnumerable<VariableSymbol> variables, BoundStatement statement)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            Variables = variables;
            Statement = statement;
        }

        public BoundGlobalScope Previous { get; }
        public IEnumerable<Diagnostic> Diagnostics { get; }
        public IEnumerable<VariableSymbol> Variables { get; }
        public BoundStatement Statement { get; }
    }
}