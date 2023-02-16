namespace Compiler.Syntax
{
    /// <summary>
    /// The IfStatementSyntax class represents an if statement in the syntax tree.
    /// </summary>
    public sealed class IfStatementSyntax : StatementSyntax
    {
        public IfStatementSyntax(SyntaxToken ifToken,ParenthesisExpressionSyntax parenthesisExpression, 
        BlockStatementSyntax blockIfStatement, ElseClauseSyntax elseClauseStatement)
        {
            IfToken = ifToken;
            if(!Aux(parenthesisExpression.GetChildren().ToList()))
            {
                throw new Exception("Error: No se puede asignar un valor a una variable en la condición del if");
            }
            ParenthesisExpression = parenthesisExpression;
            BlockIfStatement = blockIfStatement;
            ElseClauseStatement = elseClauseStatement;
        }
        public bool Aux(List<SyntaxNode> list)
        {
            foreach (var nodo in list)
            {
                if(nodo.Kind == SyntaxKind.EqualsToken)
                return false;
            }
            return true;
        }
        public SyntaxToken IfToken { get; }
        public ParenthesisExpressionSyntax ParenthesisExpression { get; }
        public BlockStatementSyntax BlockIfStatement { get; }
        public ElseClauseSyntax ElseClauseStatement { get; }

        public override SyntaxKind Kind => SyntaxKind.IfStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IfToken;
            yield return ParenthesisExpression;
            yield return BlockIfStatement;
            yield return ElseClauseStatement;
        }
    }
}