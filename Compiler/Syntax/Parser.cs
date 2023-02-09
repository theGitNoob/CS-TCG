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
                /// <summary>
                /// Skip whitespace tokens, such as spaces, tabs, and newlines.
                /// </summary>
                token = lexer.Lex();
                if (token.Kind != SyntaxKind.WhiteSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                    tokens.Add(token);

            } while (token.Kind != SyntaxKind.EndOfFileToken);
            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }
        public DiagnosticBag Diagnostics => _diagnostics;
        /// <summary>
        /// Peek returns the token at the specified offset without consuming it.
        /// </summary>
        private SyntaxToken Peek(int offset)
        {
            var index =  _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];
            return _tokens[index];
        }
        /// <summary>
        /// NextToken consumes the current token and returns the next token.
        /// </summary>
        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;

            return current;
        }
        /// <summary>
        /// Match consumes the current token if it matches the specified kind.
        /// </summary>
        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind,kind);
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        private SyntaxToken Current => Peek(0);
        /// <summary>
        /// Parse returns the root of the generated syntax tree.
        /// </summary>
        public CompilerUnitSyntax ParseCompilationUnit()
        {
            var statement = ParseStatement();
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);

            return new CompilerUnitSyntax(statement, endOfFileToken);
        }
        /// <summary>
        /// ParseStatement parses any statement.
        /// </summary>
        private StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenBraceToken:
                    return ParseBlockStatements();
                case SyntaxKind.IfKeyword:
                    return ParseIfStatement();
            }
            return ParseExpressionStatement();
        }
        /// <summary>
        /// ParseIfStatement parses an if statement.
        /// </summary>
         private StatementSyntax ParseIfStatement()
        {
            var keyword = Match(SyntaxKind.IfKeyword);
            var condition = (ParenthesisExpressionSyntax)ParsePrimaryExpression();
            var block = (BlockStatementSyntax)ParseBlockStatements();
            var elseClause = ParseElseClause();
            return new IfStatementSyntax(keyword,condition,block,elseClause);
        }
        /// <summary>
        /// ParseElseClause parses an else clause.
        /// </summary>
        private ElseClauseSyntax ParseElseClause()
        {
            if(Current.Kind != SyntaxKind.ElseKeyword)
                return null;
            
            var keyword = Match(SyntaxKind.ElseKeyword);
            var block = (BlockStatementSyntax)ParseBlockStatements();
            return new ElseClauseSyntax(keyword, block);
        }

        /// <summary>
        /// ParseExpressionStatement parses an expression statement.
        /// </summary>
        private StatementSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntax(expression);
        }
        /// <summary>
        /// ParseBlockStatements parses a block statement.
        /// </summary>
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

        /// <summary>
        /// ParseExpression parses any expression.
        /// </summary>
        private ExpressionSyntax ParseExpression()
        {
            return ParseAssignmentExpression();
        }
        /// <summary>
        /// ParseAssignmentExpression parses an assignment expression.
        /// </summary>
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
        /// <summary>
        /// ParseBinaryExpression parses a binary expression.
        /// </summary>
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
        /// <summary>
        /// ParsePrimaryExpression parses any primary expression.
        /// </summary>
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
        /// <summary>
        /// Match checks if the current token is of the expected kind.
        /// If it is, it returns the current token and advances the lexer.
        /// If it isn't, it returns a missing token.
        /// </summary>
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