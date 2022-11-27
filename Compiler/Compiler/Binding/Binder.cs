using System;
using System.Collections.Generic;
using Compiler.Syntax;

namespace Compiler.Binding
{
    internal sealed class Binder
    {
        public Dictionary<VariableSymbol, object> _variables { get; }
        public Binder(Dictionary<VariableSymbol, object> variables)
        {
            _variables = variables;
        }
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public DiagnosticBag Diagnostics => _diagnostics;

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
                case SyntaxKind.ParenthesisExpression:
                    return BindParenthesisExpression((ParenthesisExpressionSyntax)syntax);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntax)syntax);
                case SyntaxKind.AssignmentExpression:
                    return BindAssignmentExpression((AssignmentExpression)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }
        private BoundExpression BindParenthesisExpression(ParenthesisExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }
        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if(variable == null)
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }
            return new BoundVariableExpression(variable);
        }
        private BoundExpression BindAssignmentExpression(AssignmentExpression syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);
            var existVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if(existVariable != null)
                _variables.Remove(existVariable);
            var variable = new VariableSymbol(name,boundExpression.Type);
            _variables[variable] = null;

            return new BoundAssignment(variable,boundExpression);     
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
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span,syntax.OperatorToken.Text,boundOperand.Type);
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
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span,syntax.OperatorToken.Text,boundLeft.Type,boundRight.Type);
                return boundLeft;
            } 
            return new BoundBinaryExpression(boundLeft,boundOperator,boundRight);
        }
    }
}