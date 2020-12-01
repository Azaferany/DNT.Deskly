using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DNT.Deskly.GuardToolkit;

namespace DNT.Deskly.Validation
{
    public abstract class ModelValidator<TModel, TValidatorCaller> : IModelValidator<TModel, TValidatorCaller>, IModelValidator<TModel>
    {
        public virtual async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            if (!((IModelValidator)this).CanValidateInstancesOfType(model.GetType()))
            {
                throw new InvalidOperationException(
                    $"Cannot validate instances of type '{model.GetType().GetTypeInfo().Name}'. This validator can only validate instances of type '{typeof(TModel).Name}'.");
            }
            var failures = new List<ValidationFailure>();
            if(validatorCaller.GetType()==typeof(TValidatorCaller))
                failures.AddRange(await Validate((TValidatorCaller) validatorCaller, (TModel) model));

            failures.AddRange((await Validate(validatorCaller, (TModel) model)));
            return failures;
        }

        bool IModelValidator.CanValidateInstancesOfType(Type type)
        {
            return typeof(TModel).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async virtual Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, TModel model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return Enumerable.Empty<ValidationFailure>();
        }

        public abstract Task<IEnumerable<ValidationFailure>> Validate(TValidatorCaller validatorCaller, TModel model);
    }
    public abstract class ModelValidator<TModel> : IModelValidator<TModel>
    {
        async Task<IEnumerable<ValidationFailure>> IModelValidator.Validate(object validatorCaller, object model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            if (!((IModelValidator)this).CanValidateInstancesOfType(model.GetType()))
            {
                throw new InvalidOperationException(
                    $"Cannot validate instances of type '{model.GetType().GetTypeInfo().Name}'. This validator can only validate instances of type '{typeof(TModel).Name}'.");
            }

            return await Validate(validatorCaller, (TModel) model);
        }

        bool IModelValidator.CanValidateInstancesOfType(Type type)
        {
            return typeof(TModel).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

        public abstract Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, TModel model);
    }
}