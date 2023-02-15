using System.Linq.Expressions;
using Compiler.Syntax;
namespace Compiler.Binding
{
    internal class BoundMethodExpression : BoundExpression
    {
        public BoundMethodExpression(SyntaxToken identifierToken,
            BoundExpression cantToken, SyntaxToken variable)
        {
            IdentifierToken = identifierToken;
            CantToken = cantToken;
            Variable = variable;
        }

        public SyntaxToken IdentifierToken { get; }
        public BoundExpression CantToken { get; }
        public SyntaxToken Variable { get; }

        public override Type Type => throw new System.NotImplementedException();

        public override BoundNodeKind Kind => BoundNodeKind.MethodExpression;

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return CantToken;
        }
    }
}