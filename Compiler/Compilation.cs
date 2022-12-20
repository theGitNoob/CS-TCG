using System;
using System.Collections.Generic;
using Compiler.Syntax;
using Compiler.Binding;
namespace Compiler
{
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
        public Compilation ContinueWith(SyntaxTree syntaxTree)
        {
            return new Compilation(this, syntaxTree);
        }

        public SyntaxTree Syntax { get; }
        public Compilation Previous { get; }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol,object> variables)
        {
            var diagnostics = Syntax.Diagnostics.Concat(GlobalScope.Diagnostics).ToArray();
            if(diagnostics.Any())
                return new EvaluationResult(diagnostics,null);
            var evaluator = new Evaluator(GlobalScope.Statement,variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(),value);
        }
    }
}