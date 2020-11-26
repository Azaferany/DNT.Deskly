using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DNT.Deskly.Domain;
using DNT.Deskly.EFCore.Services.Application.Events;
using DNT.Deskly.Eventing;
using DNT.Deskly.Functional;

namespace DNT.Deskly.Application
{
    public static class EventBusExtensions
    {
        public static Task<Result> TriggerCreatingEventAsync<TEntity, TKey>(this IEventBus bus,
            IEnumerable<TEntity> models, CancellationToken cancellationToken = default)
            where TEntity : class ,IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return TriggerAsync(bus, typeof(CreatingBusinessEvent<TEntity, TKey>), models, cancellationToken);
        }

        public static Task<Result> TriggerCreatedEventAsync<TEntity, TKey>(this IEventBus bus,
            IEnumerable<TEntity> models, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return TriggerAsync(bus, typeof(CreatedBusinessEvent<TEntity, TKey>), models, cancellationToken);
        }

        public static Task<Result> TriggerEditingEventAsync<TEntity, TKey>(this IEventBus bus,
            IEnumerable<ModifiedModel<TEntity>> models, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return TriggerAsync(bus, typeof(EditingBusinessEvent<TEntity, TKey>), models, cancellationToken);
        }

        public static Task<Result> TriggerEditedEventAsync<TEntity, TKey>(this IEventBus bus,
            IEnumerable<ModifiedModel<TEntity>> models, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return TriggerAsync(bus, typeof(EditedBusinessEvent<TEntity, TKey>), models, cancellationToken);
        }

        public static Task<Result> TriggerDeletingEventAsync<TEntity, TKey>(this IEventBus bus,
            IEnumerable<TEntity> models, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return TriggerAsync(bus, typeof(DeletingBusinessEvent<TEntity, TKey>), models, cancellationToken);
        }

        public static Task<Result> TriggerDeletedEventAsync<TEntity, TKey>(this IEventBus bus,
            IEnumerable<TEntity> models, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return TriggerAsync(bus, typeof(DeletedBusinessEvent<TEntity, TKey>), models, cancellationToken);
        }

        private static Task<Result> TriggerAsync(IEventBus bus, Type eventType, object model,
            CancellationToken cancellationToken = default)
        {
            var businessEvent = (IBusinessEvent) Activator.CreateInstance(eventType, model);

            return bus.TriggerAsync(businessEvent, cancellationToken);
        }
    }
}