using System.Collections.Generic;
namespace Compiler
{
    /// <summary>
    /// The EvaluationResult class represents the result of evaluating an expression.
    /// </summary>
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<Diagnostic> _diagnostics, object value)
        {
            Diagnostics = _diagnostics;
            Value = value;
        }

        public IEnumerable<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }
}