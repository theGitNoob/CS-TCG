namespace Compiler.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, StatementSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get;}
        public StatementSyntax Root { get;}
        public SyntaxToken EndOfFileToken { get;}

        public static SyntaxTree Parse(string text)
        {
            var e = new Parser(text);
            return e.Parse();
        }
    }
}