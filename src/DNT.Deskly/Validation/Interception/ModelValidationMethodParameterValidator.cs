using System;
using System.Collections.Generic;
using System.Linq;

namespace DNT.Deskly.Validation.Interception
{
    internal sealed class ModelValidationMethodParameterValidator : IMethodParameterValidator
    {
        private readonly IServiceProvider _provider;

        public ModelValidationMethodParameterValidator(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IEnumerable<ValidationFailure> Validate(object validatingObject)
        {
            var validatorType = typeof(IModelValidator<>).MakeGenericType(validatingObject.GetType());

            if (!(_provider.GetService(validatorType) is IModelValidator validator))
                return Enumerable.Empty<ValidationFailure>();

            var failures = validator.Validate(validatingObject);

            return failures;
        }
    }
}