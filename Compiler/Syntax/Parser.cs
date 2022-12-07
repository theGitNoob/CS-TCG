using Compiler.Syntax;

namespace Compiler.Syntax
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();
            var lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.Lex();
                if (token.Kind != SyntaxKind.WhiteSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                    tokens.Add(token);

            } while (token.Kind != SyntaxKind.EndOfFileToken);
            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }
        public DiagnosticBag Diagnostics => _diagnostics;
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

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind,kind);
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        private SyntaxToken Current => Peek(0);
        public CompilerUnitSyntax ParseCompilationUnit()
        {
            var statement = ParseStatement();
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);

            return new CompilerUnitSyntax(statement, endOfFileToken);
        }
        private StatementSyntax ParseStatement()
        {
            if(Current.Kind == SyntaxKind.OpenBraceToken)
                return ParseBlockStatements();

            return ParseExpressionStatement();
        }

        private StatementSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntax(expression);
        }

        private StatementSyntax ParseBlockStatements()
        {
            var openBraceToken = Match(SyntaxKind.OpenBraceToken);
            var statements = new List<StatementSyntax>();
            while (Current.Kind != SyntaxKind.EndOfFileToken &&
                   Current.Kind != SyntaxKind.CloseBraceToken )
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }
            var closeBraceToken = Match(SyntaxKind.CloseBraceToken);
            return new BlockStatementSyntax(openBraceToken,statements,closeBraceToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseAssignmentExpression();
        }
        private ExpressionSyntax ParseAssignmentExpression()
        {
            if(Peek(0).Kind == SyntaxKind.IndentifierToken
            && Peek(1).Kind == SyntaxKind.EqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssignmentExpression();

                return new AssignmentExpression(identifierToken,operatorToken,right);
            }

            return ParseBinaryExpression();
        }
        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;

            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
                left = new UnaryExpression(operatorToken,operand);
            }else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if(precedence <= parentPrecedence || precedence == 0)
                    break;
                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpression(left,operatorToken,right);             
            }

            return left;
        }
        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.QuotesToken:
                {
                    var left = NextToken();
                    var stringToken = new SyntaxToken(SyntaxKind.StringToken,0,CreateString(),CreateString());
                    var right = Match(SyntaxKind.QuotesToken);

                    return new StringExpressionSyntax(left,stringToken,right);
                }
                case SyntaxKind.OpenParenthesisToken:
                {
                    var left = NextToken();
                    var expression = ParseExpression();
                    var right = Match(SyntaxKind.CloseParenthesisToken);

                    return new ParenthesisExpressionSyntax(left, expression, right);
                }

                case SyntaxKind.FalseKeyword:
                case SyntaxKind.TrueKeyword:
                {
                    var keywordToken = NextToken();
                    var value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpression(keywordToken, value);
                }
                case SyntaxKind.IndentifierToken:
                {
                    var identifierToken = NextToken();
                    return new NameExpressionSyntax(identifierToken);
                }
               default:
               {
                    var numberToken = Match(SyntaxKind.NumberToken);
                    return new LiteralExpression(numberToken);
               }
            }
        }

        private string CreateString()
        {
            string stringToken = "";
            while (Current.Kind != SyntaxKind.QuotesToken &&
                    Current.Kind != SyntaxKind.EndOfFileToken)
            {
                stringToken += $"{_tokens[_position].Text} ";
                _position++;
            }
            return stringToken;
        }
    }
}