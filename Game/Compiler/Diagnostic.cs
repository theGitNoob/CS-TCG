using System.Collections;

namespace Compiler
{
    /// <summary>
    /// The Diagnostic class represents a diagnostic in the syntax tree.
    /// </summary>
    public class Diagnostic
    {
        public Diagnostic(TextSpan span, string message)
        {
            Span = span;
            Message = message;
        }

        public TextSpan Span { get; }
        public string Message { get; }

        public override string ToString() => Message;
    }
}