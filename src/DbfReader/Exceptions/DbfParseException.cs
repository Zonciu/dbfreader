using System;

namespace DbfReader.Exceptions
{
    public class DbfParseException : Exception
    {
        public DbfParseException(string message)
            : base(message)
        {
        }
    }
}
