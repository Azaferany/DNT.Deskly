using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Deskly.Domain;
using DNT.Deskly.Eventing;

namespace DNT.Deskly.Application
{
    public class EditedBusinessEvent<TEntity, TKey> : IBusinessEvent
        where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public EditedBusinessEvent(IEnumerable<ModifiedModel<TEntity>> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<ModifiedModel<TEntity>> Models { get; }
    }
}