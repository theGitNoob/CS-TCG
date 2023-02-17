using System;
using Compiler.Syntax;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundUnaryOperator class represents a unary operator in the bound tree.
    /// </summary>
    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(SyntaxKind syntaxKind,BoundUnaryOperatorKind kind, Type operandType)
            : this(syntaxKind,kind,operandType,operandType)
        {
            
        }
        private BoundUnaryOperator(SyntaxKind syntaxKind,BoundUnaryOperatorKind kind, Type operandType, Type resultType)
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

        /// <summary>
        /// The _operators array contains all the unary operators.
        /// </summary>
        public static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken,BoundUnaryOperatorKind.LogicalNegation,typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.PlusToken,BoundUnaryOperatorKind.Identity,typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken,BoundUnaryOperatorKind.Negation,typeof(int)),
        };
        /// <summary>
        /// The Bind method binds a unary operator to a given operand type.
        /// </summary>
        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operatorType)
        {
            foreach (var op in _operators)
            {
                if(op.SyntaxKind == syntaxKind && op.OperandType == operatorType)
                    return op;
            }
            return null!;
        }
    }
}