﻿namespace Infrastructure.Exceptions.AppExceptions
{
    public class SqlClientImplementationException : Exception
    {
        public SqlClientImplementationException(string? message) : base(message)
        {
        }
    }
}