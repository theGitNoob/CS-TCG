using System;
using System.Collections.Generic;

namespace Compiler.Syntax
{
    public sealed partial class BinaryExpression : ExpressionSyntax
    {
        public BinaryExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public ExpressionSyntax Right { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Left { get; }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}