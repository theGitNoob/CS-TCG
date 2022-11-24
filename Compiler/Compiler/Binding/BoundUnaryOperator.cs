using System;
using Compiler.Syntax;

namespace Compiler.Binding
{
    internal sealed class BoundUnaryOperator
    {
        private BoundBinaryExpression(SyntaxKind syntaxKind,BoundUnaryOperatorKind kind, Type operandType)
            : this(sintaxKind,kind,operandType,operandType)
        {
            
        }
        private BoundBinaryExpression(SyntaxKind syntaxKind,BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        public BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken,BoundUnaryOperatorKind.LogicalNegation,typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.PlusToken,BoundUnaryOperatorKind.Identity,typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken,BoundUnaryOperatorKind.Negation,typeof(int)),
        };
        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operatorType)
        {
            foreach (var op in _operators)
            {
                if(op.SyntaxKind == syntaxKind && op.OperandType == operatorType)
                    return op;
            }
            return null;
        }
    }
}