using System.Collections.Generic;

namespace DbfReader
{
    public interface IDbfTable : IEnumerable<IDbfRow>
    {
        IDbfRow this[int index] { get; }
        bool HasHeader(string name);
    }
}
