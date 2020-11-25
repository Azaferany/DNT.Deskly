using DNT.Deskly.Application;
using DNT.Deskly.Domain;
using DNT.Deskly.Functional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DNT.Deskly.EFCore.Services.Application
{
    public interface ICrudService<TEntity> : ICrudService<int, TEntity>, IApplicationService
    where TEntity : class, IEntity<int>
    { }
    public interface ICrudService<in TKey, TEntity> : IApplicationService
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    {
        //TODO: implement find methods with paging and sorting and filtering

        Task<Maybe<TEntity>> FindAsync(TKey id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> FindListAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> FindListAsync(CancellationToken cancellationToken = default);


        Task<Result> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<Result> CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task<Result> EditAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<Result> EditAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
    }
}