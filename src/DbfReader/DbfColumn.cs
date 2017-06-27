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

        private DbfColumn(byte[] data, char fieldType)
        {
            _data = data;
            _length = data.Length;
            _fieldType = fieldType;
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
            string str = ForceString();

            if (!DbfFieldType.IsString(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(string));
            }

            return str;
        }

        public DateTime GetDate(string format = "yyyyMMdd")
        {
            string str = ForceString();
            if (!DbfFieldType.IsDate(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(DateTime));
            }

            DateTime result;
            if (!DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out result)) {
                throw new DbfDateTimeInvalidFormatException(str, format);
            }

            return result;
        }

        public DateTime? GetDateOrNull(string format = "yyyyMMdd")
        {
            string str = ForceString();
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

            return result;
        }

        public int GetInt()
        {
            string str = ForceString();
            if (!DbfFieldType.IsNumeric(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(int));
            }

            int result;
            if (!int.TryParse(str, out result)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(int));
            }

            return result;
        }

        public int? GetIntOrNull()
        {
            string str = ForceString();
            if (!DbfFieldType.IsNumeric(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(int));
            }

            int result;
            if (!int.TryParse(str, out result)) {
                return null;
            }

            return result;
        }

        public decimal GetDecimal()
        {
            string str = ForceString();
            if (!DbfFieldType.IsFloatingPoint(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(decimal));
            }

            decimal result;
            if (!decimal.TryParse(str, NumberStyles.Number, null, out result)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(decimal));
            }

            return result;
        }

        public bool GetBool()
        {
            string str = ForceString();
            if (!DbfFieldType.IsBool(_fieldType)) {
                throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(bool));
            }

            switch ((char) _data[0]) {
                case 'Y':
                case 'y':
                    return true;

                case 'N':
                case 'n':
                    return false;

                default:
                    throw new DbfColumnTypeMismatchException(_fieldType, str, typeof(bool));
            }
        }
    }
}
