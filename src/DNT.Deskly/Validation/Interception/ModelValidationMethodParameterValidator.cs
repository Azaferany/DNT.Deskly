using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNT.Deskly.Validation.Interception
{
    internal sealed class ModelValidationMethodParameterValidator : IMethodParameterValidator
    {
        private readonly IServiceProvider _provider;

        public ModelValidationMethodParameterValidator(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object validatingObject)
        {
            var validatorType = typeof(IModelValidator<>).MakeGenericType(validatingObject.GetType());

            if (_provider.GetService(validatorType) is not IModelValidator validator)
                return Enumerable.Empty<ValidationFailure>();

            var failures = await validator.Validate(validatorCaller, validatingObject);

            validatorType = typeof(IModelValidator<,>).MakeGenericType(validatingObject.GetType(), validatorCaller.GetType());

            return failures;
        }
    }
}