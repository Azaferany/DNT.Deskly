using System.Collections.Generic;
using System.Threading.Tasks;
using DNT.Deskly.Dependency;

namespace DNT.Deskly.Validation.Interception
{
    /// <summary>
    /// This interface is used to validate method parameters.
    /// </summary>
    public interface IMethodParameterValidator : ITransientDependency
    {
        Task<IEnumerable<ValidationFailure>> Validate(object validatorCaller, object validatingObject);
    }
}