using Compiler.Syntax;
using Compiler.Binding;
using Player;

namespace Compiler
{
    /// <summary>
    /// The Compilation class represents a compilation of a single source file.
    /// </summary>
    public sealed class Compilation
    {
        private BoundGlobalScope _globalScope;
        public Compilation(SyntaxTree syntax):
            this(null,syntax)
        {
            Syntax = syntax;
        }
        private Compilation(Compilation previous, SyntaxTree syntax)
        {
            Previous = previous;
            Syntax = syntax;
        }
        /// <summary>
        /// Gets the syntax tree of the compilation.
        /// </summary>
        internal BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope == null)
                {
                    var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, Syntax.Root);
                    Interlocked.CompareExchange(ref _globalScope,globalScope,null);
                }
                return _globalScope;
            }
        }
        /// <summary>
        /// Continues the compilation with the specified syntax tree.
        /// </summary>
        public Compilation ContinueWith(SyntaxTree syntaxTree)
        {
            return new Compilation(this, syntaxTree);
        }

        public SyntaxTree Syntax { get; }
        public Compilation Previous { get; }

        /// <summary>
        /// Evaluates the compilation.
        /// </summary>
        public EvaluationResult Evaluate(Dictionary<VariableSymbol,object> variables,SimplePlayer player)
        {
            var diagnostics = Syntax.Diagnostics.Concat(GlobalScope.Diagnostics).ToArray();
            if(diagnostics.Any())
                return new EvaluationResult(diagnostics,null);
            var evaluator = new Evaluator(GlobalScope.Statement,variables, player);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(),value);
        }
    }
}