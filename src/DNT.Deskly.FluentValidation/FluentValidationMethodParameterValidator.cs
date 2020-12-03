using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNT.Deskly.Domain;
using DNT.Deskly.ReflectionToolkit;
using DNT.Deskly.Validation;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;
using fvValidationFailure = FluentValidation.Results.ValidationFailure;
using DNT.Deskly.Validation.Interception;

namespace DNT.Deskly.FluentValidation
{
    internal class FluentValidationMethodParameterValidator : IMethodParameterValidator
    {
        private readonly IValidatorFactory _factory;

        public FluentValidationMethodParameterValidator(IValidatorFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object validatingObject)
        {
            var fvValidator = _factory.GetType().GetMethod("GetValidator")
                .MakeGenericMethod(validatingObject.GetType()).Invoke(_factory, null);

            if (fvValidator == null) return Enumerable.Empty<ValidationFailure>();


            if (fvValidator.GetType().IsGenericType)
            {

                if (fvValidator.GetType() == typeof(FluentModelValidator<>).MakeGenericType(fvValidator.GetType().GenericTypeArguments))
                    fvValidator.GetType().GetProperty("ValidatorCaller").SetValue(fvValidator, validatorCaller);

                else if (fvValidator.GetType() == typeof(FluentModelValidator<,>).MakeGenericType(fvValidator.GetType().GenericTypeArguments))
                    fvValidator.GetType().GetProperty("ValidatorCaller").SetValue(fvValidator, validatorCaller.CastToReflected(validatorCaller.GetType()));

            }



            var Validator = await fvValidator.GetType().GetMethod("ValidateAsync").InvokeAsync(fvValidator, validatingObject);

            ValidationResult validationResult = (ValidationResult)Validator;
            var failures = validationResult.Errors
                .Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage));

            return failures;
        }
    }
}