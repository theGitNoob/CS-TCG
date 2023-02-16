using System.Collections.Generic;
namespace Compiler.Syntax
{
    /// <summary>
    /// The ParenthesisExpressionSyntax class represents a parenthesis expression in the syntax tree.
    /// </summary>
    public sealed class ParenthesisExpressionSyntax : ExpressionSyntax
    {
        public ParenthesisExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParenthesisToken { get; }

        public override SyntaxKind Kind => SyntaxKind.ParenthesisExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
        }
    }
}