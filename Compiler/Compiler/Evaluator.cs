using Compiler.Syntax;
namespace Compiler
{
    public sealed class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            this._root = root;
        }
        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }
        private int EvaluateExpression(ExpressionSyntax node)
        {
            if (node is LiteralExpression n)
                return (int)n.LiteralToken.Value;
            
            if (node is UnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);
                if(u.OperatorToken == SyntaxKind.PlusToken)
                    return operand;
                else if(u.OperatorToken == SyntaxKind.MinusToken)
                    return -operand;
                else
                     throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}");
            }
            if (node is BinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                if (b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                else if (b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                else if (b.OperatorToken.Kind == SyntaxKind.StarToken)
                    return left * right;
                else if (b.OperatorToken.Kind == SyntaxKind.SlashToken)
                    return left / right;
                else
                    throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
            }
            if (node is ParenthesisExpressionSyntax p)
                return EvaluateExpression(p.Expression);

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}