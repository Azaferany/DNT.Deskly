using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DNT.Deskly.Exceptions
{
    [Serializable]
    public class FrameworkException : Exception
    {
        public FrameworkException()
        {
        }

        public FrameworkException(string message)
            : base(message)
        {
        }

        public FrameworkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FrameworkException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
