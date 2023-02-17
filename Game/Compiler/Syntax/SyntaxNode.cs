using System.Collections.Generic;
namespace Compiler.Syntax
{
    /// <summary>
    /// The SyntaxNode class represents a node in the syntax tree.
    /// </summary>
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}