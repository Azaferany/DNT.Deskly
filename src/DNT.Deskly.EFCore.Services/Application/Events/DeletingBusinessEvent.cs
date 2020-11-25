using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Deskly.Domain;
using DNT.Deskly.Eventing;

namespace DNTFrameworkCore.Application
{
    public class DeletingBusinessEvent<TEntity, TKey> : IBusinessEvent
        where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public DeletingBusinessEvent(IEnumerable<TEntity> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<TEntity> Models { get; }
    }
}