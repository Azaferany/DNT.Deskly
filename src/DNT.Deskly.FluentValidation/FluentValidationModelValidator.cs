using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Deskly.Validation;
using FluentValidation;

namespace DNT.Deskly.FluentValidation
{
    internal class FluentValidationModelValidator<T> : ModelValidator<T>
    {
        private readonly IValidatorFactory _factory;

        public FluentValidationModelValidator(IValidatorFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public override IEnumerable<ValidationFailure> Validate(T model)
        {
            var fvValidator = _factory.GetValidator<T>();

            if (fvValidator == null) return Enumerable.Empty<ValidationFailure>();

            var validationResult = fvValidator.Validate(model);
            var failures = validationResult.Errors
                .Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage))
                .ToList();

            return failures;
        }
    }
}