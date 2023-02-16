using System.Collections.Generic;

namespace Compiler.Syntax
{
    /// <summary>
    /// The AssignmentExpression class represents an assignment expression in the syntax tree.
    /// </summary>
    public sealed class AssignmentExpression : ExpressionSyntax
    {
        public AssignmentExpression(SyntaxToken identifierToken, SyntaxToken equalsOperator, ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            EqualsOperator = equalsOperator;
            Expression = expression;
        }
        public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqualsOperator { get; }
        public ExpressionSyntax Expression { get; }
       
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return EqualsOperator;
            yield return Expression;
        }
    }
}