using System;

namespace DbfReader.Exceptions
{
    public class DbfColumnTypeMismatchException : Exception
    {
        public DbfColumnTypeMismatchException(char columnType, string value, Type t) 
            : base($"Value \"{value}\" in column of type \"{columnType}\" was invalid. Expected type: {t.FullName}.")
        {
        }
    }
}
