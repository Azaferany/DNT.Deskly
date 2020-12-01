using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNT.Deskly.Validation
{
    public class NullModelValidator<TModel> : IModelValidator<TModel>
    {
        public bool CanValidateInstancesOfType(Type type)
        {
            return true;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, TModel model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return Enumerable.Empty<ValidationFailure>();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return Enumerable.Empty<ValidationFailure>();
        }
    }
}
