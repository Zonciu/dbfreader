namespace DbfReader
{
    public interface IDbfRow
    {
        IDbfColumn this[int index] { get; }
        IDbfColumn this[string name] { get; }
    }
}
