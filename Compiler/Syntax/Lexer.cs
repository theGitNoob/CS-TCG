namespace Compiler.Syntax
{
    /// <summary>
    /// The class Lexer represents a lexical analyzer.
    /// </summary>
    internal sealed class Lexer
    {
        private int _position;
        private readonly string _text;
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        public Lexer(string text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;
        private char Current => Peek(0);
        
        private char LoookaHead => Peek(1);
        /// <summary>
        /// Peek returns the character at the specified offset without consuming it.
        /// </summary>
        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';

            return _text[index];
        }

        private void Next()
        {
            _position++;
        }
        /// <summary>
        /// Lex returns the next token from the source text.
        /// </summary>
        public SyntaxToken Lex()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null!);
            }
            var start = _position;
            /// <summary>
            /// Lex a number token.
            /// </summary>
            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                {
                    _diagnostics.ReportInvalidNumber(new TextSpan(start,length),_text,typeof(int));
                }
                return new SyntaxToken(SyntaxKind.NumberToken, start, text,value);
            }
            /// <summary>
            /// Lex a whitespace token.
            /// </summary>
            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null!);
            }
            /// <summary>
            /// Lex a identifier or keyword token.
            /// </summary>
            if (char.IsLetter(Current))
            {

                while (char.IsLetter(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null!);
            }
            /// <summary>
            /// Lex a single character token.
            /// </summary>
            switch (Current)
            {
                case ',':
                    return new SyntaxToken(SyntaxKind.CommaToken, _position++, ",", null!);
                case ';':
                    return new SyntaxToken(SyntaxKind.SemicolonToken, _position++, ";", null!);
                case '?':
                    return new SyntaxToken(SyntaxKind.QuestionToken, _position++, "?", null!);
                case '.':
                    return new SyntaxToken(SyntaxKind.DotToken, _position++, ".", null!);
                case '"':
                    return new SyntaxToken(SyntaxKind.QuotesToken, _position++,"\"",null!);
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null!);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null!);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null!);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null!);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null!);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null!);
                case '{':
                    return new SyntaxToken(SyntaxKind.OpenBraceToken, _position++, "{", null!);
                case '}':
                    return new SyntaxToken(SyntaxKind.CloseBraceToken, _position++, "}", null!);
                case '&':
                    if(LoookaHead == '&')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&", null!);
                    }
                    break;
                case '|':
                    if(LoookaHead == '|')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, start, "||", null!);
                    }
                    break;
                case '=':
                    if(LoookaHead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==", null!);
                    }
                    else
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.EqualsToken, start, "=", null!);
                    } 
                case '<':
                    if(LoookaHead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.LessOrEqualsToken, start, "<=", null!);
                    }
                    else
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.LessToken, start, "<", null!);
                    }  
                case '>':
                    if(LoookaHead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.GreaterOrEqualsToken, start, ">=", null!);
                    }
                    else
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.GreatToken, start, ">", null!);
                    }  
                case '!':
                    if(LoookaHead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, start, "!=", null!);
                    }
                    else
                    {
                        _position ++;
                        return new SyntaxToken(SyntaxKind.BangToken, start, "!", null!);    
                    }
            }

            /// <summary>
            /// Lex a bad token.
            /// </summary>
            _diagnostics.ReportBadCharacter(_position,Current);
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position-1,1), null!);
        }
    }
}