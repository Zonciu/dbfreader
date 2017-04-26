using System.Collections.Generic;

namespace DbfReader
{
    internal static class DbfFieldType
    {
        public const char Character = 'C';
        public const char Currency = 'Y';
        public const char Numeric = 'N';
        public const char Float = 'F';
        public const char Date = 'D';
        public const char DateTime = 'T';
        public const char Double = 'B';
        public const char Integer = 'I';
        public const char Logical = 'L';
        public const char Memo = 'M';
        public const char General = 'G';
        public const char Picture = 'P';
        public const char Autoincrement = '+';
        public const char Timestamp = '@';

        private static List<char> _all = new List<char>
        {
            Character,
            Currency,
            Numeric,
            Float,
            Date,
            DateTime,
            Double,
            Integer,
            Logical,
            Memo,
            General,
            Picture,
            Autoincrement,
            Double,
            Timestamp
        };
        private static List<char> _numerics = new List<char>
        {
            Integer
        };
        private static List<char> _decimals = new List<char>
        {
            Currency, Double, Float, Numeric
        };

        public static bool IsValidType(char typ) =>
            _all.Contains(typ);

        public static bool IsNumeric(char typ) =>
            _numerics.Contains(typ);

        public static bool IsFloatingPoint(char typ) =>
            _decimals.Contains(typ);

        public static bool IsBool(char typ) =>
            typ == Logical;

        public static bool IsString(char typ) =>
            typ == Character;

        public static bool IsDate(char typ) =>
            typ == Date || typ == DateTime;
    }
}
