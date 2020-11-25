using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DNT.Deskly.Dependency;
using DNT.Deskly.Functional;
using DNT.Deskly.Validation;

namespace DNT.Deskly.Eventing
{
    public interface IBusinessEventHandler<in T> : ITransientDependency
        where T : IBusinessEvent
    {
        Task<Result> Handle(T businessEvent, CancellationToken cancellationToken = default);
    }

    public abstract class BusinessEventHandler<T> : IBusinessEventHandler<T> where T : IBusinessEvent
    {
        public abstract Task<Result> Handle(T businessEvent, CancellationToken cancellationToken = default);
        protected Result Fail(string message) => Result.Fail(message);

        protected Result Fail(string message, IEnumerable<ValidationFailure> failures) =>
            Result.Fail(message, failures);

        protected Result Ok() => Result.Ok();
    }
}