namespace Compiler
{
    public class WorkNumbers
    {
        enum SyntaxKind
        {
            NumberToken,
            WhiteSpaceToken,
            PlusToken,
            MinusToken,
            StarToken,
            SlashToken,
            OpenParenthesisToken,
            CloseParenthesisToken,
            BadToken,
            EndOfFileToken,
            NumberExpression,
            BinaryExpression,
            ParenthesisExpression
        }
        class SyntaxToken : SyntaxNode
        {

            public SyntaxToken(SyntaxKind kind, int position, string text, object value)
            {
                Kind = kind;
                Position = position;
                Text = text;
                Value = value;
            }
            public override SyntaxKind Kind { get; }

            public int Position { get; }
            public string Text { get; }
            public object Value { get; }

            public override IEnumerable<SyntaxNode> GetChildren()
            {
                return Enumerable.Empty<SyntaxNode>();
            }
        }
        class Lexer
        {
            private int _position;
            private readonly string _text;
            private List<string> _diagnostics = new List<string>();
            public Lexer(string text)
            {
                _text = text;
            }

            public IEnumerable<string> Diagnostics => _diagnostics;
            private char Current
            {
                get {
                    if (_position >= _text.Length)
                        return '\0';

                    return _text[_position];
                }
            }

            private void Next()
            {
                _position++;
            }
            public SyntaxToken NextToken()
            {
                /* Esto tiene (),*,+,-,/ y espacios en blacos*/
                if (_position >= _text.Length)
                {
                    return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
                }
                if (char.IsDigit(Current))
                {
                    var start = _position;

                    while (char.IsDigit(Current))
                        Next();

                    var length = _position - start;
                    var text = _text.Substring(start, length);
                    if (!int.TryParse(text, out var value))
                    {
                        _diagnostics.Add($"ERROR: The number {text} is not valid in Int32");
                    }
                    return new SyntaxToken(SyntaxKind.NumberToken, _position, text,value);
                }
                if (char.IsWhiteSpace(Current))
                {
                    var start = _position;

                    while (char.IsWhiteSpace(Current))
                        Next();

                    var length = _position - start;
                    var text = _text.Substring(start, length);
                    return new SyntaxToken(SyntaxKind.WhiteSpaceToken, _position, text, null);
                }
                if (Current == '+')
                {
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                }
                if (Current == '-')
                {
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                }
                if (Current == '*')
                {
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                }
                if (Current == '/')
                {
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                }
                if (Current == '(')
                {
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                }
                if (Current == ')')
                {
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
                }

                _diagnostics.Add($"ERROR: bad character input: '{Current}'");
                return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position-1,1), null);
            }
        }
        abstract class SyntaxNode
        {
            public abstract SyntaxKind Kind { get; }

            public abstract IEnumerable<SyntaxNode> GetChildren();
        }
        abstract class ExpressionSyntax : SyntaxNode
        {
        }
        sealed class NumberExpression : ExpressionSyntax
        {
            public NumberExpression(SyntaxToken numberToken)
            {
                NumberToken = numberToken;
            }
            public override SyntaxKind Kind => SyntaxKind.NumberExpression;

            public SyntaxToken NumberToken { get; }

            public override IEnumerable<SyntaxNode> GetChildren()
            {
                yield return NumberToken;
            }
        }
        sealed class BinaryExpression : ExpressionSyntax
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
        sealed class ParenthesisExpressionSyntax : ExpressionSyntax
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
        sealed class SyntaxTree
        {
            public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
            {
                Diagnostics = diagnostics.ToArray();
                Root = root;
                EndOfFileToken = endOfFileToken;
            }

            public IReadOnlyList<string> Diagnostics { get;}
            public ExpressionSyntax Root { get;}
            public SyntaxToken EndOfFileToken { get;}

            public static SyntaxTree Parse(string text)
            {
                var e = new Parser(text);
                return e.Parse();
            }
        }
        class Parser
        {
            private readonly SyntaxToken[] _tokens;
            private int _position;
            private List<string> _diagnostics = new List<string>();
            public Parser(string text)
            {
                var tokens = new List<SyntaxToken>();
                var lexer = new Lexer(text);
                SyntaxToken token;
                do
                {
                    token = lexer.NextToken();
                    if (token.Kind != SyntaxKind.WhiteSpaceToken &&
                        token.Kind != SyntaxKind.BadToken)
                        tokens.Add(token);

                } while (token.Kind != SyntaxKind.EndOfFileToken);
                _tokens = tokens.ToArray();
                _diagnostics.AddRange(lexer.Diagnostics);
            }
            public IEnumerable<string> Diagnostics => _diagnostics;
            private SyntaxToken Peek(int offset)
            {
                var index =  _position + offset;
                if (index >= _tokens.Length)
                    return _tokens[_tokens.Length - 1];
                return _tokens[index];
            }
            private SyntaxToken NextToken()
            {
                var current = Current;
                _position++;

                return current;
            }
            private SyntaxToken Match(SyntaxKind kind)
            {
                if (Current.Kind == kind)
                    return NextToken();

                _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}> expected <{kind}>");
                return new SyntaxToken(kind, Current.Position, null, null);
            }
            private SyntaxToken Current => Peek(0);
            public SyntaxTree Parse()
            {
                var expression = ParseTerm();
                var endOfFileToken = Match(SyntaxKind.EndOfFileToken);

                return new SyntaxTree(_diagnostics, expression, endOfFileToken);
            }
            private ExpressionSyntax ParseTerm()
            {
                var left = ParseFactor();
                while (Current.Kind == SyntaxKind.PlusToken || 
                    Current.Kind == SyntaxKind.MinusToken)
                {
                    var operatorToken = NextToken();
                    var right = ParseFactor();
                    left = new BinaryExpression(left, operatorToken, right);
                }
                return left;
            }
            private ExpressionSyntax ParseExpression()
            {
                return ParseTerm();
            }
            private ExpressionSyntax ParseFactor()
            {
                var left = ParsePrimaryExpression();
                while (Current.Kind == SyntaxKind.StarToken ||
                    Current.Kind == SyntaxKind.SlashToken)
                {
                    var operatorToken = NextToken();
                    var right = ParsePrimaryExpression();
                    left = new BinaryExpression(left, operatorToken, right);
                }
                return left;
            }
            private ExpressionSyntax ParsePrimaryExpression()
            {
                if (Current.Kind == SyntaxKind.OpenParenthesisToken)
                {
                    var left = NextToken();
                    var expression = ParseExpression();
                    var right = Match(SyntaxKind.CloseParenthesisToken);

                    return new ParenthesisExpressionSyntax(left, expression, right);
                }

                var numberToken = Match(SyntaxKind.NumberToken);
                return new NumberExpression(numberToken);
            }
        }

        class Evaluator
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
                if (node is NumberExpression n)
                    return (int)n.NumberToken.Value;
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
}