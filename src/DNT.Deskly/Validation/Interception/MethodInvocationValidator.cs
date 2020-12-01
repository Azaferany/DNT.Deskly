using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DNT.Deskly.Dependency;
using DNT.Deskly.GuardToolkit;
using DNT.Deskly.Extensions;
using Microsoft.Extensions.Options;
using DNT.Deskly.ReflectionToolkit;
using System.Threading.Tasks;

namespace DNT.Deskly.Validation.Interception
{
    /// <summary>
    /// This class is used to validate a method call (invocation) for method arguments.
    /// </summary>
    public sealed class MethodInvocationValidator : ITransientDependency
    {
        private const int MaxRecursiveParameterValidationDepth = 8;

        private readonly IOptions<ValidationOptions> _options;
        private readonly IEnumerable<IMethodParameterValidator> _validators;
        private readonly IList<ValidationFailure> _failures;

        public MethodInvocationValidator(
            IOptions<ValidationOptions> options,
            IEnumerable<IMethodParameterValidator> validators)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));

            _failures = new List<ValidationFailure>();
        }

        public async Task<IEnumerable<ValidationFailure>> ValidateAsync(object validatorCaller, MethodInfo method, object[] parameterValues)
        {
            Guard.ArgumentNotNull(method, nameof(method));
            Guard.ArgumentNotNull(parameterValues, nameof(parameterValues));

            if (method.ValidationIgnored()) return Enumerable.Empty<ValidationFailure>();

            var parameters = method.GetParameters();

            if (parameters.Length != parameterValues.Length)
            {
                throw new Exception("Method parameter count does not match with argument count!");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                await ValidateMethodParameterAsync(validatorCaller, parameters[i], parameterValues[i]);
            }

            return _failures;
        }

        /// <summary>
        /// Validates given parameter for given value.
        /// </summary>
        /// <param name="parameterInfo">Parameter of the method to validate</param>
        /// <param name="parameterValue">Value to validate</param>
        private async Task ValidateMethodParameterAsync(object validatorCaller, ParameterInfo parameterInfo, object parameterValue)
        {
            if (parameterValue == null)
            {
                if (!parameterInfo.IsOptional &&
                    !parameterInfo.IsOut &&
                    !TypeHelper.IsPrimitiveExtendedIncludingNullable(parameterInfo.ParameterType, true))
                {
                    _failures.Add(new ValidationFailure(parameterInfo.Name, parameterInfo.Name + " is null!"));
                }

                return;
            }

            await ValidateObjectRecursivelyAsync(validatorCaller, parameterValue, 1);
        }

        private async Task ValidateObjectRecursivelyAsync(object validatorCaller, object validatingObject, int depth)
        {
            if (depth > MaxRecursiveParameterValidationDepth)
            {
                return;
            }

            if (validatingObject == null)
            {
                return;
            }

            if (_options.Value.IgnoredTypes.Any(t => t.IsInstanceOfType(validatingObject)))
            {
                return;
            }

            if (TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObject.GetType()))
            {
                return;
            }

            await SetValidationErrorsAsync(validatorCaller, validatingObject);

            // Validate items of enumerable
            if (IsEnumerable(validatingObject))
            {
                foreach (var item in (IEnumerable) validatingObject)
                {
                    await ValidateObjectRecursivelyAsync(validatorCaller,
                                                         item,
                                                         depth + 1);
                }
            }

            if (!ShouldMakeDeepValidation(validatingObject)) return;

            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
            foreach (var property in properties)
            {
                if (property.Attributes.OfType<SkipValidationAttribute>().Any())
                {
                    continue;
                }

                await ValidateObjectRecursivelyAsync(validatorCaller, property.GetValue(validatingObject), depth + 1);
            }
        }

        private async Task SetValidationErrorsAsync(object validatorCaller, object validatingObject)
        {
            foreach (var validator in _validators)
            {
                var failures = await validator.Validate(validatorCaller, validatingObject);
                _failures.AddRange(failures);
            }
        }

        private static bool ShouldMakeDeepValidation(object validatingObject)
        {
            // Do not recursively validate for enumerable objects
            if (validatingObject is IEnumerable)
            {
                return false;
            }

            var validatingObjectType = validatingObject.GetType();

            // Do not recursively validate for primitive objects
            return !TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObjectType);
        }

        private static bool IsEnumerable(object validatingObject)
        {
            return
                validatingObject is IEnumerable &&
                !(validatingObject is IQueryable) &&
                !TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObject.GetType());
        }
    }
}