using System;
using System.Threading;
using System.Threading.Tasks;
using DNT.Deskly.Dependency;
using DNT.Deskly.Functional;
using Microsoft.Extensions.DependencyInjection;

namespace DNT.Deskly.Eventing
{
    public interface IEventBus : IScopedDependency
    {
        Task<Result> TriggerAsync(IBusinessEvent businessEvent, CancellationToken cancellationToken = default);
    }

    internal sealed class EventBus : IEventBus
    {
        private const string MethodName = "Handle";
        private readonly IServiceProvider _provider;

        public EventBus(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<Result> TriggerAsync(IBusinessEvent businessEvent,
            CancellationToken cancellationToken = default)
        {
            var eventType = businessEvent.GetType();
            var handlerType = typeof(IBusinessEventHandler<>).MakeGenericType(eventType);

            var method = handlerType.GetMethod(MethodName, new[] { eventType, typeof(CancellationToken) });
            if (method == null) throw new InvalidOperationException();

            var handlers = _provider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var result =
                    await (Task<Result>)method.Invoke(handler, new object[] { businessEvent, cancellationToken });

                if (result.Failed) return result;
            }

            return Result.Ok();
        }

    }
}