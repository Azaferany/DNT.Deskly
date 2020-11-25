using DNT.Deskly.Extensions;
using DNT.Deskly.ReflectionToolkit;
using DNT.Deskly.Validation;
using System.Reflection;

namespace DNTFrameworkCore.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool ValidationIgnored(this MethodInfo method)
        {
            var parameters = method.GetParameters();
            return !method.IsPublic || parameters.IsNullOrEmpty() || IsValidationSkipped(method);
        }

        private static bool IsValidationSkipped(MemberInfo method)
        {
            if (method.IsDefined(typeof(EnableValidationAttribute), true))
            {
                return false;
            }

            return ReflectionHelper
                       .GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<SkipValidationAttribute>(method) != null;
        }
    }
}