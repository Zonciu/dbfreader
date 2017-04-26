using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DbfReader
{
    public class DbfTable : IDbfTable
    {
        private List<DbfHeader> _headers = new List<DbfHeader>();
        private List<IDbfRow> _rows = new List<IDbfRow>();
        private int _rowLength = int.MinValue;

        public IDbfRow this[int index]
        {
            get
            {
                return _rows[index];
            }
        }

        internal void AddHeader(DbfHeader header)
        {
            if (header == null) return;
            if (DbfFieldType.IsFloatingPoint(header.FieldType) && header.NumDecimalPlaces == 0) {
                header.FieldType = DbfFieldType.Integer;
            }
            _headers.Add(header);
        }

        internal void AddRow(byte[] row)
        {
            _rows.Add(new DbfRow(row, this));
        }

        internal int GetRowLength()
        {
            if (_rowLength != int.MinValue) return _rowLength;

            var lastHeader = _headers.Last();
            _rowLength = lastHeader.ColumnOffset + lastHeader.Length + 1;

            return _rowLength;
        }

        internal DbfHeader GetHeader(string name)
        {
            DbfHeader result = _headers
                .SingleOrDefault(t => t.Name == name.ToLower());

            if (result == null) {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Invalid value for column index {0}", name));
            }

            return result;
        }

        internal DbfHeader GetHeader(int index)
        {
            DbfHeader result = _headers
                .SingleOrDefault(t => t.Index == index);

            if (result == null) {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Invalid value for column index {0}", index));
            }

            return result;
        }

        public bool HasHeader(string name)
        {
            return _headers.Any(t => t.Name == name.ToLower());
        }

        public IEnumerator<IDbfRow> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _rows.GetEnumerator();
        }
    }
}
