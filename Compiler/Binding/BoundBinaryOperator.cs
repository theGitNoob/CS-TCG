using System;
using Compiler.Syntax;

namespace Compiler.Binding
{
    /// <summary>
    /// The BoundBinaryOperatorKind enum represents the kind of a binary operator.
    /// </summary>
    internal sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(SyntaxKind syntaxKind,BoundBinaryOperatorKind kind, Type type)
            : this(syntaxKind,kind,type,type,type)
        {
            
        }
         private BoundBinaryOperator(SyntaxKind syntaxKind,BoundBinaryOperatorKind kind, Type operandType,Type resultType)
            : this(syntaxKind,kind,operandType,operandType,resultType)
        {
            
        }
        private BoundBinaryOperator(SyntaxKind syntaxKind,BoundBinaryOperatorKind kind, Type leftType,Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        /// <summary>
        /// The Bind method binds a binary operator to a given operand type.
        /// </summary>
        public static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.PlusToken,BoundBinaryOperatorKind.Addition,typeof(string)),
            new BoundBinaryOperator(SyntaxKind.PlusToken,BoundBinaryOperatorKind.Addition,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken,BoundBinaryOperatorKind.Subtraction,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarToken,BoundBinaryOperatorKind.Multiplication,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken,BoundBinaryOperatorKind.Division,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken,BoundBinaryOperatorKind.LogicalAnd,typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePipeToken,BoundBinaryOperatorKind.LogicalOr,typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken,BoundBinaryOperatorKind.Equals,typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEqualsToken,BoundBinaryOperatorKind.NotEquals,typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken,BoundBinaryOperatorKind.Equals,typeof(int),typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEqualsToken,BoundBinaryOperatorKind.NotEquals,typeof(int),typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LessOrEqualsToken,BoundBinaryOperatorKind.LessOrEquals,typeof(int),typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LessToken,BoundBinaryOperatorKind.Less,typeof(int),typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.GreatToken,BoundBinaryOperatorKind.Great,typeof(int),typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.GreaterOrEqualsToken,BoundBinaryOperatorKind.GreaterOrEquals,typeof(int),typeof(bool))
        };
        /// <summary>
        /// The Bind method binds a binary operator to a given operand type.
        /// </summary>
        /// <param name="syntaxKind">The syntax kind of the operator.</param>
        /// <param name="leftType">The type of the left operand.</param>
        /// <param name="rightType">The type of the right operand.</param>
        /// <returns>The bound binary operator.</returns>
        /// <exception cref="Exception">Thrown when no operator is found.</exception>
        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if(op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType)
                    return op;
            }
            return null;
        }
    }
}