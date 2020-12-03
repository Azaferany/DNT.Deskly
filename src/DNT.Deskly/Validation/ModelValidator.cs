using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DNT.Deskly.GuardToolkit;
using DNT.Deskly.ReflectionToolkit;

namespace DNT.Deskly.Validation
{
    public abstract class ModelValidator<TModel> : IModelValidator<TModel>
    {
        public virtual async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object model)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            Guard.ArgumentNotNull(validatorCaller, nameof(validatorCaller));

            if (!((IModelValidator)this).CanValidateInstancesOfType(model.GetType()))
            {
                throw new InvalidOperationException(
                    $"Cannot validate instances of type '{model.GetType().GetTypeInfo().Name}'. This validator can only validate instances of type '{typeof(TModel).Name}'.");
            }

            var failures = new List<ValidationFailure>();

            if (this.GetType().IsAssignableFrom(typeof(IModelValidator<>).MakeGenericType(typeof(TModel), validatorCaller.GetType())))
            {
                var validatorCallerMethod = this.GetType().GetMethods().Where(x => x.Name == "Validate")
                .Where(x => x.GetParameters().Where(x => x.ParameterType == validatorCaller.GetType()).FirstOrDefault() != null)
                ?.FirstOrDefault();
                var result = (IEnumerable<ValidationFailure>)await validatorCallerMethod.InvokeAsync(
                    this,
                    new object[] {
                        validatorCaller.CastToReflected(validatorCaller.GetType()),
                        model.CastToReflected(model.GetType())
                    });

                failures.AddRange(result);

            }

            failures.AddRange(await Validate(validatorCaller, (TModel)model));

            return failures;
        }

        public virtual bool CanValidateInstancesOfType(Type type)
        {
            return typeof(TModel).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

        public abstract Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, TModel model);
    }

}