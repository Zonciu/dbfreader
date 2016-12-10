using System;

namespace DbfReader
{
    internal static class ByteHelper
    {
        public static byte[] GetBytes(byte[] data, Range<byte> range, bool inclusive = true) =>
            GetBytes(data, range.Start, range.End, inclusive);

        public static byte[] GetBytes(byte[] data, int start, int end, bool inclusive = true)
        {
            int length = end - start + (inclusive ? 1 : 0);
            var result = new byte[length];
            Array.Copy(data, start, result, 0, length);
            return result;
        }

        public static string GetString(byte[] data, Range<byte> range, bool inclusive = true) =>
            GetString(GetBytes(data, range, inclusive));

        public static string GetString(byte[] data, int start, int end, bool inclusive = true) =>
            GetString(GetBytes(data, start, end, inclusive));

        public static string GetString(byte[] data)
        {
            string result = "";

            for (int i = 0; i < data.Length; ++i) {
                if (data[i] == '\0') {
                    return result;
                }
                result += (char)data[i];
            }

            return result;
        }

        public static int GetInt(byte[] data, Range<byte> range, bool inclusive = true) =>
            GetInt(GetBytes(data, range, inclusive));

        public static int GetInt(byte[] data)
        {
            var cast = new byte[4];
            // big endian (LSB)
            for (int i = data.Length - 1, j = 3; i >= 0; --i) {
                if (data[i] != '\0') {
                    cast[j--] = data[i];
                }
            }

            string hexValue = "";

            foreach (byte byt in cast) {
                if (byt != '\0') {
                    hexValue += byt.ToString("x2");
                }
            }

            if (string.IsNullOrWhiteSpace(hexValue)) {
                return 0;
            }

            return Convert.ToInt32(hexValue, 16);
        }
    }
}
