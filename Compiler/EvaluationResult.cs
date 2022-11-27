using System.Collections.Generic;
namespace Compiler
{
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