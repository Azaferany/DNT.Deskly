using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Deskly.Domain;
using DNT.Deskly.Eventing;

namespace DNT.Deskly.EFCore.Services.Application.Events
{
    public class CreatingBusinessEvent<TEntity, TKey> : IBusinessEvent
        where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public CreatingBusinessEvent(IEnumerable<TEntity> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<TEntity> Models { get; }
    }
}