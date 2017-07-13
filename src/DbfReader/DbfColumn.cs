using DbfReader.Exceptions;
using System;
using System.Globalization;

namespace DbfReader
{
    public interface IDbfColumn
    {
        string ForceString();
        /// <summary>
        /// Returns the string value in the column. Will not return null
        /// </summary>
        /// <returns></returns>
        string GetString();
        DateTime GetDate(string format = "yyyyMMdd");
        DateTime? GetDateOrNull(string format = "yyyyMMdd");
        int GetInt();
        int? GetIntOrNull();
        decimal GetDecimal();
        bool GetBool();
        object GetValue();
    }

    public class DbfColumn : IDbfColumn
    {
        private readonly byte[] _data;
        private readonly int _length;
        private readonly char _fieldType;

        private readonly string _dataAsString;
        private bool _cacheSet;
        private object _dataCache;

        private DbfColumn(byte[] data, char fieldType)
        {
            _data = data;
            _length = data.Length;
            _fieldType = fieldType;

            _cacheSet = false;
            _dataCache = null;
            _dataAsString = ForceString();
        }

        public static IDbfColumn GetColumn(byte[] data, char fieldType)
        {
            if (IsBooleanField(data, fieldType)) {
                fieldType = DbfFieldType.Logical;
            }

            return new DbfColumn(data, fieldType);
        }

        private static bool IsBooleanField(byte[] data, char fieldType)
        {
            if (fieldType != DbfFieldType.Character || data.Length > 1) {
                return false;
            }

            switch ((char) data[0]) {
                case 'Y':
                case 'y':
                case 'N':
                case 'n':
                    return true;
            }
            return false;
        }

        public object GetValue()
        {
            switch (_fieldType) {
                case DbfFieldType.Character:
                    return GetString();

                case DbfFieldType.Integer:
                    return GetInt();

                case DbfFieldType.Numeric:
                case DbfFieldType.Float:
                case DbfFieldType.Currency:
                case DbfFieldType.Double:
                    return GetDecimal();

                case DbfFieldType.Date:
                case DbfFieldType.DateTime:
                    return GetDate();

                case DbfFieldType.Logical:
                    return GetBool();

                default:
                    return "No method found for type " + _fieldType;
            }
        }

        public string ForceString()
        {
            string result = "";
            foreach (var byt in _data) {
                result += (char) byt;
            }

            return result;
        }

        public string GetString()
        {
            if (_cacheSet) {
                return _dataCache as string;
            }

            string str = _dataAsString;

            if (!DbfFieldType.IsString(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(string));
            }

            _cacheSet = true;
            _dataCache = str;

            return str;
        }

        public DateTime GetDate(string format = "yyyyMMdd")
        {
            if (_cacheSet) {
                return (DateTime) _dataCache;
            }

            string str = _dataAsString;
            if (!DbfFieldType.IsDate(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(DateTime));
            }

            DateTime result;
            if (!DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out result)) {
                throw new DbfDateTimeInvalidFormatException(str, format);
            }

            _cacheSet = true;
            _dataCache = result;

            return result;
        }

        public DateTime? GetDateOrNull(string format = "yyyyMMdd")
        {
            if (_cacheSet) {
                return _dataCache as DateTime?;
            }

            string str = _dataAsString;
            if (!DbfFieldType.IsDate(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(DateTime));
            }

            if (string.IsNullOrWhiteSpace(str)) {
                return null;
            }

            DateTime result;
            if (!DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out result)) {
                throw new DbfDateTimeInvalidFormatException(str, format);
            }

            _cacheSet = true;
            _dataCache = result;

            return result;
        }

        public int GetInt()
        {
            if (_cacheSet) {
                return (int) _dataCache;
            }

            string str = _dataAsString;
            if (!DbfFieldType.IsNumeric(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(int));
            }

            int result;
            if (!int.TryParse(str, out result)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(int));
            }

            _cacheSet = true;
            _dataCache = result;

            return result;
        }

        public int? GetIntOrNull()
        {
            if (_cacheSet) {
                return _dataCache as int?;
            }

            string str = _dataAsString;
            if (!DbfFieldType.IsNumeric(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(int));
            }

            int result;
            if (!int.TryParse(str, out result)) {
                return null;
            }

            _cacheSet = true;
            _dataCache = result;

            return result;
        }

        public decimal GetDecimal()
        {
            if (_cacheSet) {
                return (decimal) _dataCache;
            }

            string str = _dataAsString;
            if (!DbfFieldType.IsFloatingPoint(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(decimal));
            }

            decimal result;
            if (!decimal.TryParse(str, NumberStyles.Number, null, out result)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(decimal));
            }

            _cacheSet = true;
            _dataCache = result;

            return result;
        }

        public bool GetBool()
        {
            if (_cacheSet) {
                return (bool) _dataCache;
            }

            string str = _dataAsString;
            if (!DbfFieldType.IsBool(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(bool));
            }

            switch ((char) _data[0]) {
                case 'Y':
                case 'y':
                    _cacheSet = true;
                    _dataCache = true;

                    return true;

                case 'N':
                case 'n':
                    _cacheSet = true;
                    _dataCache = false;

                    return false;

                default:
                    throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(bool));
            }
        }
    }
}
