using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Compiler.Syntax;

namespace Compiler.Binding
{
    internal sealed class Binder
    {
        public DiagnosticBag Diagnostics => _diagnostics;

        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private BoundScope _scope;

        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilerUnitSyntax syntax)
        {
            var parentScope = CreateParentScopes(previous);
            var binder = new Binder(parentScope);
            var statement = binder.BindStatement(syntax.Statement);
            var variables = binder._scope.GetDeclaredVariables();
            var diagnostics = binder.Diagnostics.ToImmutableArray();

            if(previous != null)
                diagnostics.InsertRange(0,previous.Diagnostics);

            return new BoundGlobalScope(previous, diagnostics,variables,statement);
        }

        private static BoundScope CreateParentScopes(BoundGlobalScope previous)
        {
            var stack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }
            BoundScope parent = null;
            while (stack.Count > 1)
            {
                previous = stack.Pop();
                var scope = new BoundScope(parent);
                foreach (var v in previous.Variables)
                    scope.TryDeclare(v);
                
                parent = scope;
            }
            return parent;
        }

        public BoundStatement BindStatement(StatementSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement((BlockStatementSyntax)syntax);
                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement((ExpressionStatementSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundStatement BindBlockStatement(BlockStatementSyntax syntax)
        {
            var statements = new List<BoundStatement>();
            _scope = new BoundScope(_scope);
            foreach (var statementSyntax in syntax.Statements)
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }
            _scope = _scope.Parent;
            return new BoundBlockStatement(statements);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
            var expression = BindExpression(syntax.Expression);
            return new BoundExpressionStatement(expression);
        }

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.StringExpression:
                    return BindStringExpression((StringExpressionSyntax) syntax);
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

        private BoundExpression BindStringExpression(StringExpressionSyntax syntax)
        {
            var value = syntax.StringToken.Text ?? "";
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindParenthesisExpression(ParenthesisExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }
        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if(!_scope.TryLookup(name,out var variable))
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
            if(!_scope.TryLookup(name, out var variable))
            {
                variable = new VariableSymbol(name, boundExpression.Type);
                _scope.TryDeclare(variable);
            }
            if(boundExpression.Type != variable.Type)
                _diagnostics.ReportCannotConvert(syntax.IdentifierToken.Span, variable.Type, boundExpression.Type);
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