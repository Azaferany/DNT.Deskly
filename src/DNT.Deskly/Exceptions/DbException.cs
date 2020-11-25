using System;
using System.Collections.Generic;
using System.Text;

namespace DNT.Deskly.Exceptions
{
    [Serializable]
    public class DbException : FrameworkException
    {
        public DbException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}
