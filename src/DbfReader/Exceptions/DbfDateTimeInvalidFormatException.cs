using System;

namespace DbfReader.Exceptions
{
    public class DbfDateTimeInvalidFormatException : Exception
    {
        public DbfDateTimeInvalidFormatException(string value, string formatSpecified)
            : base($"Date (\"{value}\") was not in format provided (\"{formatSpecified}\") and could not be parsed.")
        {
        }
    }
}
