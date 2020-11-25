using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace DNT.Deskly.Exceptions
{
    [Serializable]
    public class DbConcurrencyException : DbException
    {
        public DbConcurrencyException() : base(
            "The record has been modified since it was loaded. The operation was canceled!",
            null)
        {
        }

        public DbConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
