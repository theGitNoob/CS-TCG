using System;
using System.Collections.Generic;
using Compiler.Syntax;
using Compiler.Binding;
namespace Compiler
{
    /// <summary>
    /// The Evaluator class evaluates an expression.
    /// </summary>
    internal sealed class Evaluator
    {
        private readonly BoundStatement _root;
        private readonly Dictionary<VariableSymbol, object> _variables;
        private object _lastValue;

        public Evaluator(BoundStatement root, Dictionary<VariableSymbol,object> variables)
        {
            this._root = root;
            _variables = variables;
        }


        public object Evaluate()
        {
            EvaluateStatement(_root);
            return _lastValue;
        }
        private void EvaluateStatement(BoundStatement node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.IfStatement:
                    EvaluateIfStatement((BoundIfStatement) node);
                    break;
                case BoundNodeKind.BlockStatement:
                    EvaluateBlockStatement((BoundBlockStatement) node);
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EvaluateExpressionStatement((BoundExpressionStatement) node);
                    break;    
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }

        private void EvaluateIfStatement(BoundIfStatement node)
        {
            var condition = (bool)EvaluateExpression(node.Condition);
            if(condition)
                EvaluateStatement(node.IfStatement);
            else if(node.ElseStatement != null) 
                EvaluateStatement(node.ElseStatement);    
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = EvaluateExpression(node.Expression);
        }

        private void EvaluateBlockStatement(BoundBlockStatement node)
        {
            foreach(var statement in node.Statements)
                EvaluateStatement(statement);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteralExpression n)
                return n.Value;
            if  (node is BoundVariableExpression v)
                return _variables[v.Variable];

            if  (node is BoundAssignment a)
            {
                var value = EvaluateExpression(a.Expression);
                _variables[a.Variable] = value;
                return value;
            }    
            
            if (node is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.Op.Kind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int)operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -(int)operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool)operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.Op}");
                }         
            }
            if (node is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                switch (b.Op.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        {
                            if(left.GetType() == typeof(string) && right.GetType() == typeof(string))
                                return (string)left + (string)right;
                            return (int)left + (int)right;    
                        }
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int)left - (int)right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int)left * (int)right;
                    case BoundBinaryOperatorKind.Division:
                        return (int)left / (int)right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)left && (bool)right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)left || (bool)right;
                    case BoundBinaryOperatorKind.Equals:
                        return Equals(left,right);
                    case BoundBinaryOperatorKind.NotEquals:
                        return !Equals(left,right);
                    case BoundBinaryOperatorKind.Less:
                        return (int)left < (int)right;
                    case BoundBinaryOperatorKind.LessOrEquals:
                        return (int)left <= (int)right;
                    case BoundBinaryOperatorKind.Great:
                        return (int)left > (int)right;
                    case BoundBinaryOperatorKind.GreaterOrEquals:
                        return (int)left >= (int)right;
                    default:
                        throw new Exception($"Unexpected binary operator {b.Op}");
                }                
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}