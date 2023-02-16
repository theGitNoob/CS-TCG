namespace Compiler.Binding
{
    /// <summary>
    /// The BoundScope class represents a scope in the bound tree.
    /// </summary>
    internal sealed class BoundScope
    {
        /// <summary>
        /// The _variables field represents the variables declared in the scope.
        /// </summary>
        private Dictionary<string,VariableSymbol> _variables = new Dictionary<string, VariableSymbol>();

        public BoundScope(BoundScope parent)
        {
            Parent = parent;
        }

        public BoundScope Parent { get; }

        /// <summary>
        /// The TryDeclare method attempts to declare a variable in the scope.
        /// </summary>
        /// <param name="variable">The variable to declare.</param>
        /// <returns>True if the variable was declared, false otherwise.</returns>
        public bool TryDeclare(VariableSymbol variable)
        {
            if(_variables.ContainsKey(variable.Name))
                return false;
            _variables.Add(variable.Name, variable);
            return true;
        }
        /// <summary>
        /// The TryLookup method attempts to lookup a variable in the scope.
        /// </summary>
        /// <param name="name">The name of the variable to lookup.</param>
        /// <param name="variable">The variable if found, null otherwise.</param>
        /// <returns>True if the variable was found, false otherwise.</returns>
        public bool TryLookup(string name, out VariableSymbol variable)
        {
            if(_variables.TryGetValue(name,out variable!))
                return true;
            if(Parent == null)
                return false;
            return Parent.TryLookup(name, out variable);
        }
        /// <summary>
        /// The GetDeclaredVariables method returns the variables declared in the scope.
        /// </summary>
        public IEnumerable<VariableSymbol> GetDeclaredVariables()
        {
            return _variables.Values.ToArray();
        }
    }
}