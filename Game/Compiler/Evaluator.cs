using Compiler.Binding;
using Player;

namespace Compiler
{
    /// <summary>
    /// The Evaluator class evaluates an expression.
    /// </summary>
    internal sealed class Evaluator
    {
        private readonly SimplePlayer _player;
        private readonly BoundStatement _root;
        private readonly Dictionary<VariableSymbol, object> _variables;
        private object _lastValue;

        public Evaluator(BoundStatement root, Dictionary<VariableSymbol, object> variables,SimplePlayer player)
        {
            this._root = root;
            _variables = variables;
            _player = player;
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
                    EvaluateIfStatement((BoundIfStatement)node);
                    break;
                case BoundNodeKind.BlockStatement:
                    EvaluateBlockStatement((BoundBlockStatement)node);
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EvaluateExpressionStatement((BoundExpressionStatement)node);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }


        private void AttackHero(int cnt, string name)
        {
            foreach (var hero in _player.HeroZone)
            {
                if (name == hero.Name)
                {
                    hero.Attack += cnt;
                }
            }
        }

        private int GetValue(BoundNode node)
        {
            foreach (var item in node.GetChildren())
            {
                if (item.Kind == BoundNodeKind.LiteralExpression)
                {
                    return (int)((BoundLiteralExpression)item).Value;
                }
                GetValue(item);
            }
            return 0;
        }
        private void EvaluateIfStatement(BoundIfStatement node)
        {
            var condition = (bool)EvaluateExpression(node.Condition);
            if (condition)
                EvaluateStatement(node.IfStatement);
            else if (node.ElseStatement != null)
                EvaluateStatement(node.ElseStatement);
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = EvaluateExpression(node.Expression);
        }

        private void EvaluateBlockStatement(BoundBlockStatement node)
        {
            foreach (var statement in node.Statements)
                EvaluateStatement(statement);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            if (node.Kind == BoundNodeKind.MethodExpression)
            {
                var m = (BoundMethodExpression)node;
                var cant = EvaluateExpression(m.CantToken);
                if (!(cant.GetType() == typeof(int)))
                    throw new Exception("The type should be int");
                var name = m.Variable.Text;
                var identifier = m.IdentifierToken.Text;

                SimplePlayer used;
                if (name == "this") used = _player;
                else if (name == "enemy") used = _player.Enemy;
                else throw new Exception($"{name} not recognized, should be `enemy` or `this`");
                switch (identifier)
                {
                    case "Attack":
                    case "Defense":
                        {
                            foreach (var hero in used.HeroZone)
                            {
                                if (name == hero.Name)
                                {
                                    hero.Attack += (int)cant;
                                }
                                else
                                {
                                    hero.Defense += (int)cant;
                                }
                            }
                            break;
                        };
                    case "Life":
                        {
                            used.Hp += (int)cant;
                        }
                        break;
                }
                return "Compilation Correct";
            }
            if (node is BoundLiteralExpression n)
                return n.Value;
            if (node is BoundVariableExpression v)
                return _variables[v.Variable];

            if (node is BoundAssignment a)
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
                            if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
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
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.NotEquals:
                        return !Equals(left, right);
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