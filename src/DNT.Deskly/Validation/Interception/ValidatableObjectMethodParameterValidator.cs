using DNT.Deskly.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DNT.Deskly.Validation.Interception
{
    internal sealed class ValidatableObjectMethodParameterValidator : IMethodParameterValidator
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object validatingObject)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (validatingObject is not IValidatableObject validatable)
            {
                return Enumerable.Empty<ValidationFailure>();
            }

            var results = validatable.Validate(new ValidationContext(validatingObject));
            var failures = new List<ValidationFailure>();

            foreach (var result in results)
            {
                if (result == ValidationResult.Success) continue;

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

            return failures;
        }
    }
}