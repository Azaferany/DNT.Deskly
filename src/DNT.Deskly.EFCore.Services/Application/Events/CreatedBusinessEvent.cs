using DNT.Deskly.Domain;
using DNT.Deskly.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNT.Deskly.EFCore.Services.Application.Events
{
    public class CreatedBusinessEvent<TEntity, TKey> : IBusinessEvent
        where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public CreatedBusinessEvent(IEnumerable<TEntity> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<TEntity> Models { get; }
    }
}