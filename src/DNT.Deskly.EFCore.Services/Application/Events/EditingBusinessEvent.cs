using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Deskly.Domain;
using DNT.Deskly.Eventing;

namespace DNTFrameworkCore.Application
{
    public class EditingBusinessEvent<TEntity, TKey> : IBusinessEvent
        where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public EditingBusinessEvent(IEnumerable<ModifiedModel<TEntity>> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<ModifiedModel<TEntity>> Models { get; }
    }
}