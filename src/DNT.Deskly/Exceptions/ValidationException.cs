using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Deskly.Validation;

namespace DNT.Deskly.Exceptions
{
    [Serializable]
    public class ValidationException : FrameworkException
    {
        private readonly List<ValidationFailure> _failures;

        public ValidationException(string message) : this(message, Enumerable.Empty<ValidationFailure>())
        {
        }

        public ValidationException(string message, IEnumerable<ValidationFailure> failures) : base(message)
        {
            _failures = failures.ToList();
        }

        public IReadOnlyList<ValidationFailure> Failures => _failures;

        public ValidationException WithFailure(string memberName, string message)
        {
            _failures.Add(new ValidationFailure(memberName, message));
            return this;
        }
    }
}