using DNT.Deskly.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DNT.Deskly.Validation.Interception
{
    internal sealed class DataAnnotationMethodParameterValidator : IMethodParameterValidator
    {
        private readonly IServiceProvider _provider;

        public DataAnnotationMethodParameterValidator(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object validatingObject)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var failures = new List<ValidationFailure>();

            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
            foreach (var property in properties)
            {
                var attributes = property.Attributes.OfType<ValidationAttribute>().ToArray();
                if (attributes.IsNullOrEmpty())
                {
                    continue;
                }

                var context = new ValidationContext(validatingObject, _provider, null)
                {
                    DisplayName = property.DisplayName,
                    MemberName = property.Name
                };

                foreach (var attribute in attributes)
                {
                    var result = attribute.GetValidationResult(property.GetValue(validatingObject), context);

                    if (result == null) continue;

                    if (!result.MemberNames.IsNullOrEmpty())
                    {
                        failures.AddRange(result.MemberNames.Select(memberName =>
                            new ValidationFailure(memberName, result.ErrorMessage)));
                    }
                    else
                    {
                        failures.Add(new ValidationFailure(string.Empty, result.ErrorMessage));
                    }
                }
            }

            return failures;
        }
    }
}