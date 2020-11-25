using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;

namespace DNT.Deskly.Validation
{
    public class ValidationOptions
    {
        public ISet<Type> IgnoredTypes { get; } = new HashSet<Type> { typeof(Type), typeof(Stream), typeof(Expression), typeof(CancellationToken) };
    }
}