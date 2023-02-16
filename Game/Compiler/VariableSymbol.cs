using System;
namespace Compiler
{
    /// <summary>
    /// The VariableSymbol class represents a variable in the syntax tree.
    /// </summary>
    public sealed class VariableSymbol
    {
        internal VariableSymbol(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public Type Type { get; }
    }
}