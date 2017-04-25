using System;
using System.Collections.Generic;

namespace DbfReader
{
    public class DbfRow : IDbfRow
    {
        private byte[] _data;
        private DbfTable _table;
        private Dictionary<int, IDbfColumn> _columnCache = new Dictionary<int, IDbfColumn>();

        public IDbfColumn this[int index]
        {
            get
            {
                DbfHeader header = _table.GetHeader(index);
                return GetColumn(header);
            }
        }
        public IDbfColumn this[string name]
        {
            get
            {
                DbfHeader header = _table.GetHeader(name);
                return GetColumn(header);
            }
        }

        public DbfRow(byte[] data, DbfTable table)
        {
            _data = data;
            _table = table;
        }

        private IDbfColumn GetColumn(DbfHeader header)
        {
            IDbfColumn result;
            if (_columnCache.TryGetValue(header.Index, out result)) {
                return result;
            }

            int offs = header.ColumnOffset;
            char type = header.FieldType;
            int length = header.Length;
            var column = new byte[length];

            Array.Copy(_data, offs, column, 0, length);

            result = DbfColumn.GetColumn(column, type);

            return _columnCache[header.Index] = result;
        }
    }
}
