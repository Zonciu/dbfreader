namespace DbfReader
{
    internal class Range<T>
    {
        public T Start;
        public T End;

        public Range()
        {
        }
        public Range(T start, T end)
        {
            Start = start;
            End = end;
        }
        public Range(T start)
        {
            Start = start;
            End = start;
        }
    }
}
