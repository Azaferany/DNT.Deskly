using System;
using System.Data;

namespace DNT.Deskly.Transaction
{
    public sealed class TransactionOptions
    {
        public TimeSpan? Timeout { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }
    }
}