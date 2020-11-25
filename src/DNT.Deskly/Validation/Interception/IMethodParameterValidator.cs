using System.Collections.Generic;
using DNT.Deskly.Dependency;

namespace DNT.Deskly.Validation.Interception
{
    /// <summary>
    /// This interface is used to validate method parameters.
    /// </summary>
    public interface IMethodParameterValidator : ITransientDependency
    {
        IEnumerable<ValidationFailure> Validate(object validatingObject);
    }
}