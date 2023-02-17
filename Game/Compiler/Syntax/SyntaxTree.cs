namespace Compiler.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(string text)
        {
            var parser = new Parser(text);
            var root = parser.ParseCompilationUnit();
            var diagnostics = parser.Diagnostics.ToArray();


            Diagnostics = diagnostics;
            Root = root;
            Text = text;
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get;}
        public CompilerUnitSyntax Root { get;}
        public SyntaxToken EndOfFileToken { get;}
        public string Text { get; }

        /// <summary>
        /// Parse creates a syntax tree from the specified text.
        /// </summary>
        public static SyntaxTree Parse(string text)
        {
           return new SyntaxTree(text);
        }
    }
}