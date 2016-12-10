using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
            if (fieldType != DbfFieldType.Character || data.Length > 1) return false;
            switch ((char)data[0]) {
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
                result += (char)byt;
            }

            return result;
        }

        public string GetString()
        {
            if (!DbfFieldType.IsString(_fieldType)) ThrowTypeMismatch<string>();

            return ForceString();
        }

        public DateTime GetDate(string format = "yyyyMMdd")
        {
            if (!DbfFieldType.IsDate(_fieldType)) ThrowTypeMismatch<DateTime>();

            string str = ForceString();

            DateTime result;
            if (!DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out result)) {
                throw new ArgumentException(string.Format(
                    "Date was not in format provided and could not be parsed\nProvided {0}\nValue {1}",
                    format,
                    str));
            }

            return result;
        }

        public DateTime? GetDateOrNull(string format = "yyyyMMdd")
        {
            if (!DbfFieldType.IsDate(_fieldType)) ThrowTypeMismatch<DateTime>();

            string str = ForceString();

            if (string.IsNullOrWhiteSpace(str)) {
                return null;
            }

            DateTime result;
            if (!DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out result)) {
                throw new ArgumentException(string.Format(
                    "Date was not in format provided and could not be parsed\nProvided {0}\nValue {1}",
                    format,
                    str));
            }

            return result;
        }

        public int GetInt()
        {
            if (!DbfFieldType.IsNumeric(_fieldType)) ThrowTypeMismatch<int>();
            string str = ForceString();

            int result;
            if (!int.TryParse(str, out result)) {
                throw new InvalidCastException(string.Format(
                    "ColType {0} failed to parse integer {1}",
                    _fieldType,
                    str));
            }

            return result;
        }

        public int? GetIntOrNull()
        {
            if (!DbfFieldType.IsNumeric(_fieldType)) ThrowTypeMismatch<int>();
            string str = ForceString();

            int result;
            if (!int.TryParse(str, out result)) {
                return null;
            }

            return result;
        }

        public decimal GetDecimal()
        {
            if (!DbfFieldType.IsFloatingPoint(_fieldType)) ThrowTypeMismatch<decimal>();
            string str = ForceString();

            decimal result;
            if (!decimal.TryParse(str, NumberStyles.Number, null, out result)) {
                throw new InvalidCastException(string.Format(
                    "Failed to parse decimal {0} -> {1}",
                    _fieldType,
                    str));
            }

            return result;
        }

        public bool GetBool()
        {
            if (!DbfFieldType.IsBool(_fieldType)) ThrowTypeMismatch<bool>();
            string str = ForceString();

            switch ((char)_data[0]) {
                case 'Y':
                case 'y':
                    return true;

                case 'N':
                case 'n':
                    return false;

                default:
                    throw new InvalidCastException(string.Format(
                        "Failed to parse bool {0} -> {1}",
                        _fieldType,
                        str));
            }
        }

        private void ThrowTypeMismatch<T>()
        {
            throw new ArgumentException(string.Format(
                "Column of type {0} is not of type {1}",
                _fieldType,
                typeof(T).FullName));
        }
    }
}
