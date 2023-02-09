namespace Compiler.Syntax
{
    /// <summary>
    /// The SyntaxFacts class contains helper methods for working with syntax nodes.
    /// </summary>
    internal static class SyntaxFacts
    {
        /// <summary>
        /// Gets the precedence of the specified binary operator.
        /// </summary>
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Gets the precedence of the specified binary operator.
        /// </summary>
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                case SyntaxKind.LessToken:
                case SyntaxKind.LessOrEqualsToken:
                case SyntaxKind.GreatToken:
                case SyntaxKind.GreaterOrEqualsToken:
                    return 3;
                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;
                case SyntaxKind.PipePipeToken:
                    return 1;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Gets the keyword kind for the specified text.
        /// </summary>
        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "if":
                    return SyntaxKind.IfKeyword;
                case "else":
                    return SyntaxKind.ElseKeyword;
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IndentifierToken;
            }
        } 
    }
}