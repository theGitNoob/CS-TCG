using System;
using System.Collections.Generic;
using Compiler.Syntax;

namespace Compiler.Binding
{
    internal sealed class Binder
    {
        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;
        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpression)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpression)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpression)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }
        private BoundExpression BindLiteralExpression(LiteralExpression syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
        private BoundExpression BindUnaryExpression(UnaryExpression syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperatorKind = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);
            if(boundOperatorKind == null)
            {
                _diagnostics.Add($"Unary operator {syntax.OperatorToken.Text} is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperatorKind,boundOperand);
        }
        private BoundExpression BindBinaryExpression(BinaryExpression syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type,boundRight.Type);
            if(boundOperator == null)
            {
                _diagnostics.Add($"Binary operator {syntax.OperatorToken.Text} is not defined for type {boundLeft.Type} and {boundRight.Type}.");
                return boundLeft;
            } 
            return new BoundBinaryExpression(boundLeft,boundOperator,boundRight);
        }
    }
}