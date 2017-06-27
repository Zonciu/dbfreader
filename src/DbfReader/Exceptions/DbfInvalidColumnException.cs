using System;

namespace DbfReader.Exceptions
{
    public class DbfInvalidColumnException : Exception
    {
        public DbfInvalidColumnException(int index)
            : base($"Column name not found at index {index}.")
        {
        }
        public DbfInvalidColumnException(string columnName)
            : base($"Column name not found: {columnName}.")
        {
        }
    }
}
