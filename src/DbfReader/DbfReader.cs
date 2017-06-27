using DbfReader.Exceptions;
using System;

namespace DbfReader
{
    // http://www.dbf2002.com/dbf-file-format.html
    public class DbfReader : IDisposable
    {
        private const int SubrecordLength = 32;
        private const byte _headerRecordTerminator = 0x0d;

        private ByteFile _data;
        private int _columnOffset;

        public static DbfTable OpenTable(string filepath, bool includeDisplacement = false)
        {
            ByteFile data = ByteFile.Open(filepath);

            using (var result = new DbfReader(data)) {
                return result.ParseTable(includeDisplacement);
            }
        }

        private DbfReader(ByteFile data)
        {
            _data = data;
        }

        public void Dispose()
        {
            _data.Dispose();
        }

        private DbfTable ParseTable(bool includeDisplacement)
        {
            var table = new DbfTable();
            int numRecords = ByteHelper.GetInt(_data.GetBytes(DbfByteOffset.NumRecords));
            int rowLength = ByteHelper.GetInt(_data.GetBytes(DbfByteOffset.LengthOfDataRecord));

            // Create headers
            int headerIndex = 0;
            DbfHeader header = null;
            int dataPointer = DbfByteOffset.FieldSubrecords.Start;

            _columnOffset = 0;

            while (true) {
                byte[] row = _data.GetBytes(dataPointer, dataPointer + SubrecordLength, false);
                header = ParseHeader(row, headerIndex++, includeDisplacement);

                if (header == null) {
                    break;
                }
                table.AddHeader(header);
                dataPointer += SubrecordLength;
            }

            rowLength = table.GetRowLength();

            // Add subrecords (rows)
            for (int i = dataPointer + 2; i < _data.Length; i += rowLength) {
                byte[] row = _data.GetBytes(i, i + rowLength);
                table.AddRow(row);
            }

            return table;
        }

        private DbfHeader ParseHeader(byte[] data, int index, bool includeDisplacement)
        {
            if (data[0] == _headerRecordTerminator) {
                return null;
            }

            if (data.Length != SubrecordLength) {
                throw new DbfParseException($"Subrecord length ({data.Length}) should be equal to {SubrecordLength}");
            }
            var header = new DbfHeader();

            header.Index = index;
            header.Name = ByteHelper.GetString(data, DbfByteOffset.FieldName).ToLower();
            header.FieldType = GetFieldType(ByteHelper.GetBytes(data, DbfByteOffset.FieldType));
            header.Length = ByteHelper.GetInt(data, DbfByteOffset.LengthOfField);
            header.NumDecimalPlaces = ByteHelper.GetInt(data, DbfByteOffset.NumDecimalPlaces);
            header.ColumnOffset = _columnOffset;

            if (includeDisplacement) {
                header.ColumnDisplacement = ByteHelper.GetInt(data, DbfByteOffset.ColumnDisplacement);
            }

            _columnOffset += header.Length + header.ColumnDisplacement;

            return header;
        }

        private char GetFieldType(byte[] data)
        {
            if (data.Length != 1) {
                throw new DbfParseException("Field type must consist of only 1 byte");
            }

            if (!DbfFieldType.IsValidType((char) data[0])) {
                throw new DbfParseException($"Field is not a valid field type: {(char) data[0]} / {(int) data[0]}");
            }

            return (char) data[0];
        }
    }
}
