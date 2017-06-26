using System;
using System.IO;

namespace DbfReader
{
    internal class ByteFile : IDisposable
    {
        private byte[] _data;

        public static ByteFile Open(string filepath)
        {
            if (!File.Exists(filepath)) {
                throw new FileNotFoundException($"The specified file could not be located: {filepath}");
            }

            return new ByteFile
            {
                _data = File.ReadAllBytes(filepath)
            };
        }

        public void Dispose()
        {
            _data = null;
        }

        public int Length
        {
            get
            {
                return _data.Length;
            }
        }

        public byte this[int index]
        {
            get
            {
                return _data[index];
            }
        }

        public byte[] GetBytes(Range<byte> r, bool inclusive = true) =>
            GetBytes(r.Start, r.End, inclusive);

        public byte[] GetNBytes(int start, int length) =>
            GetBytes(start, start + length);

        public byte[] GetBytes(int start, int end, bool inclusive = true)
        {
            if (_data == null) {
                Console.WriteLine("File not opened yet");
                return null;
            }
            if (end < 0) {
                Console.WriteLine("Invalid value for end {0}", end);
                return null;
            }
            if (start < 0) {
                Console.WriteLine("Invalid value for start {0}", start);
                return null;
            }
            if (start > end) {
                Console.WriteLine("Start cannot be after end");
                return null;
            }
            if (end >= _data.Length) {
                end = _data.Length - 1;
            }

            int length = end - start + (inclusive ? 1 : 0);
            var result = new byte[length];

            Array.Copy(_data, start, result, 0, length);

            return result;
        }
    }
}
