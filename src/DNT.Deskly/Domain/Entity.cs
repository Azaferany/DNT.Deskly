using System;
using System.Collections.Generic;

namespace DNT.Deskly.Domain
{
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IEntity : IEntity<int>
    {
    }
}