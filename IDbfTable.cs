using System.Collections.Generic;

namespace DbfReader
{
    public interface IDbfTable : IEnumerable<DbfRow>
    {
        DbfRow this[int index] { get; }
        bool HasHeader(string name);
    }
}
