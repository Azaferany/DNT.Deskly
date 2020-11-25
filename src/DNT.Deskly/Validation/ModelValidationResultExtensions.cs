using System.Collections.Generic;
using System.Linq;
using DNT.Deskly.Functional;

namespace DNT.Deskly.Validation
{
    public static class ModelValidationResultExtensions
    {
        public static Result ToResult(this IEnumerable<ValidationFailure> failures)
        {
            failures = failures as ValidationFailure[] ?? failures.ToArray();
            return !failures.Any() ? Result.Ok() : Result.Fail(string.Empty, failures);
        }
    }
}