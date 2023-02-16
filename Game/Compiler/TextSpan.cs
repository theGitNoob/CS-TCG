namespace Compiler
{
    /// <summary>
    /// The TextSpan class represents a span of text in the source code.
    /// </summary>
    public struct TextSpan
    {
        public TextSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
    }
}