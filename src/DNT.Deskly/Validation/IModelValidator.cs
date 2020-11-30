using System;
using System.Collections.Generic;
using DNT.Deskly.Dependency;

namespace DNT.Deskly.Validation
{
    public interface IModelValidator<in TModel, TValidatorCaller> : IModelValidator
    {
        /// <summary>
        /// Validate the specified instance synchronously.
        /// contains validation logic and business rules validation
        /// </summary>
        /// <param name="model">model to validate</param>
        /// <returns>
        /// A list of <see cref="ValidationFailure"/> indicating the results of validating the model value.
        /// </returns>
        IEnumerable<ValidationFailure> Validate(TValidatorCaller validatorCaller, TModel model);
    }
    public interface IModelValidator<in TModel> : IModelValidator
    {
        /// <summary>
        /// Validate the specified instance synchronously.
        /// contains validation logic and business rules validation
        /// </summary>
        /// <param name="model">model to validate</param>
        /// <returns>
        /// A list of <see cref="ValidationFailure"/> indicating the results of validating the model value.
        /// </returns>
        IEnumerable<ValidationFailure> Validate(object validatorCaller, TModel model);
    }

    public interface IModelValidator : ITransientDependency
    {
        /// <summary>
        /// Validate the specified instance synchronously.
        /// contains validation logic and business rules validation
        /// </summary>
        /// <param name="model">model to validate</param>
        /// <returns>
        /// A list of <see cref="ValidationFailure"/> indicating the results of validating the model value.
        /// </returns>
        IEnumerable<ValidationFailure> Validate(object validatorCaller, object model);

        bool CanValidateInstancesOfType(Type type);
    }
}