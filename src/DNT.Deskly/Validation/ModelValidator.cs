using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DNT.Deskly.GuardToolkit;

namespace DNT.Deskly.Validation
{
    public abstract class ModelValidator<TModel, TValidatorCaller> : IModelValidator<TModel, TValidatorCaller>
        where TValidatorCaller : class
    {
        async Task<IEnumerable<ValidationFailure>> IModelValidator.Validate(object validatorCaller, object model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            if (!((IModelValidator) this).CanValidateInstancesOfType(model.GetType()))
            {
                throw new InvalidOperationException(
                    $"Cannot validate instances of type '{model.GetType().GetTypeInfo().Name}'. This validator can only validate instances of type '{typeof(TModel).Name}'.");
            }

            return await Validate((TValidatorCaller) validatorCaller, (TModel) model);
        }

        bool IModelValidator.CanValidateInstancesOfType(Type type)
        {
            return typeof(TModel).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
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

            return await Validate(validatorCaller, (TModel)model);
        }

        bool IModelValidator.CanValidateInstancesOfType(Type type)
        {
            return typeof(TModel).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

        public abstract Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, TModel model);
    }
}