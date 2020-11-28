using DNT.Deskly.Application;
using DNT.Deskly.Domain;
using DNT.Deskly.EFCore.Context;
using DNT.Deskly.EFCore.Context.Extensions;
using DNT.Deskly.Eventing;
using DNT.Deskly.Functional;
using DNT.Deskly.GuardToolkit;
using DNT.Deskly.Transaction;
using DNT.Deskly.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;


namespace DNT.Deskly.EFCore.Services.Application
{
    //public class CrudService<TEntity> : CrudService<int, TEntity>,
    //    ICrudService<TEntity>, IQueryableService<TEntity>
    //    where TEntity : class, IEntity
    //{
    //    protected CrudService(IUnitOfWork uow, IEventBus bus) : base(uow, bus)
    //    {
    //    }
    //}

    public class CrudService <TKey, TEntity> : ApplicationService,
    ICrudService<TKey, TEntity>, IQueryableService<TKey, TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    {
        protected readonly DbSet<TEntity> EntitySet;
        protected readonly IEventBus EventBus;
        protected readonly IUnitOfWork UnitOfWork;

        public CrudService(IUnitOfWork uow, IEventBus bus)
        {
            UnitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            EventBus = bus ?? throw new ArgumentNullException(nameof(bus));
            EntitySet = UnitOfWork.Set<TEntity>();
        }

        public IQueryable<TEntity> Entities => EntitySet;

        private Expression<Func<TEntity, bool>> BuildEqualityExpressionForId(TKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, nameof(IEntity<TKey>.Id)),
                Expression.Constant(id, typeof(TKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }



        protected async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entityList = await BuildFindQuery().Where(predicate).ToListAsync(cancellationToken);


            await AfterFindAsync(entityList, cancellationToken);

            return entityList;
        }


        [SkipValidation]
        public async Task<Maybe<TEntity>> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entities = await FindAsync(BuildEqualityExpressionForId(id), cancellationToken);

            return entities.SingleOrDefault();

        }

        [SkipValidation]
        public Task<IReadOnlyList<TEntity>> FindListAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            return FindAsync(entity => ids.Contains(entity.Id), cancellationToken);

        }

        public Task<IReadOnlyList<TEntity>> FindListAsync(CancellationToken cancellationToken = default)
        {
            return FindAsync(_ => true, cancellationToken);
        }


        [Transactional]
        public Task<Result> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(entity, nameof(entity));

            return CreateAsync(new[] { entity }, cancellationToken);
        }

        [Transactional]
        public async Task<Result> CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {

            var result = await BeforeCreateAsync(entities.ToList(), cancellationToken);
            if (result.Failed) return result;


            result = await EventBus.TriggerCreatingEventAsync<TEntity, TKey>(entities);
            if (result.Failed) return result;

            EntitySet.AddRange(entities);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
            UnitOfWork.MarkUnchanged(entities);


            result = await AfterCreateAsync(entities.ToList(), cancellationToken);
            if (result.Failed) return result;

            result = await EventBus.TriggerCreatedEventAsync<TEntity, TKey>(entities);

            return result;
        }

        [Transactional]
        public Task<Result> EditAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(entity, nameof(entity));

            return EditAsync(new[] { entity }, cancellationToken);
        }

        [Transactional]
        public async Task<Result> EditAsync(IEnumerable<TEntity> newEntities, CancellationToken cancellationToken = default)
        {

            var ids = newEntities.Select(m => m.Id).ToList();
            var oldEntities = await FindListAsync(newEntities.Select(x => x.Id), cancellationToken);

            var modifiedList = newEntities.Select(newEntity => new ModifiedModel<TEntity>
            { NewValue = newEntity, OriginalValue = oldEntities.ToDictionary(x => x.Id).GetValueOrDefault(newEntity.Id) }).ToList();

            var result = await BeforeEditAsync(modifiedList, cancellationToken);
            if (result.Failed) return result;


            result = await EventBus.TriggerEditingEventAsync<TEntity, TKey>(modifiedList, cancellationToken);
            if (result.Failed) return result;

            UnitOfWork.UpdateGraph(newEntities);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
            UnitOfWork.MarkUnchanged(newEntities);


            result = await AfterEditAsync(modifiedList, cancellationToken);
            if (result.Failed) return result;

            result = await EventBus.TriggerEditedEventAsync<TEntity, TKey>(modifiedList, cancellationToken);

            return result;
        }

        [Transactional]
        [SkipValidation]
        public Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(entity, nameof(entity));

            return DeleteAsync(new[] { entity }, cancellationToken);
        }

        [Transactional]
        [SkipValidation]
        public async Task<Result> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {

            var result = await BeforeDeleteAsync(entities.ToList(), cancellationToken);
            if (result.Failed) return result;

            result = await EventBus.TriggerDeletingEventAsync<TEntity, TKey>(entities);
            if (result.Failed) return result;

            EntitySet.RemoveRange(entities);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            result = await AfterDeleteAsync(entities.ToList(), cancellationToken);
            if (result.Failed) return result;

            result = await EventBus.TriggerDeletedEventAsync<TEntity, TKey>(entities);

            return result;
        }

        [Transactional]
        [SkipValidation]
        public async Task<Result> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var model = await FindAsync(id, cancellationToken);
            if (model.HasValue) return await DeleteAsync(model.Value, cancellationToken);

            return Ok();
        }

        [Transactional]
        [SkipValidation]
        public async Task<Result> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            var models = await FindListAsync(ids);
            if (models.Any()) return await DeleteAsync(models);

            return Ok();
        }





        protected virtual IQueryable<TEntity> BuildFindQuery()
        {
            return EntitySet.AsNoTracking();
        }







        protected virtual Task AfterFindAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task<Result> BeforeCreateAsync(IReadOnlyList<TEntity> entities,
    CancellationToken cancellationToken)
        {
            return Task.FromResult(Ok());
        }

        protected virtual Task<Result> AfterCreateAsync(IReadOnlyList<TEntity> entities,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Ok());
        }

        protected virtual Task<Result> BeforeEditAsync(IReadOnlyList<ModifiedModel<TEntity>> entities,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Ok());
        }

        protected virtual Task<Result> AfterEditAsync(IReadOnlyList<ModifiedModel<TEntity>> entities,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Ok());
        }

        protected virtual Task<Result> BeforeDeleteAsync(IReadOnlyList<TEntity> entities,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Ok());
        }

        protected virtual Task<Result> AfterDeleteAsync(IReadOnlyList<TEntity> entities,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Ok());
        }


    }
}