using DNT.Deskly.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNT.Deskly.EFCore.Services.Application
{
    public interface IQueryableService<TEntity>
    where TEntity : class, IEntity<int>
    {
    }
    public interface IQueryableService<TKey, TEntity> 
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        IQueryable<TEntity> Entities { get; }
    }
}