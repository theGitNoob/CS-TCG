using System;
using System.Collections.Generic;
using Compiler.Syntax;
using Compiler.Binding;
namespace Compiler
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public SyntaxTree Syntax { get; }
        public EvaluationResult Evaluate(Dictionary<VariableSymbol,object> variables)
        {
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(Syntax.Root);
            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if(diagnostics.Any())
                return new EvaluationResult(diagnostics,null);
            var evaluator = new Evaluator(boundExpression,variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(),value);
        }
    }
}