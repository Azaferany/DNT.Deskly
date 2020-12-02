using DNT.Deskly.ReflectionToolkit;
using DNT.Deskly.Validation;
using System.Linq;
using System.Reflection;

namespace DNT.Deskly.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool ValidationIgnored(this MethodInfo method)
        {
            var parameters = method.GetParameters();
            return !method.IsPublic || parameters.IsNullOrEmpty() || IsValidationSkipped(method);
        }


        //TODO: do more config
        private static bool IsValidationSkipped(MemberInfo method)
        {
            if (method.IsDefined(typeof(AutoSkipValidationAttribute), true) || method.IsDefined(typeof(AutoEnableValidationAttribute), true) )
            {
                bool autoSkip = false;

                foreach (var str in method.GetCustomAttribute<AutoSkipValidationAttribute>().SkipValidationNames)
                    autoSkip = method.Name.Contains(str);

                foreach (var str in method.GetCustomAttribute<AutoEnableValidationAttribute>(false).EnableValidationNames)
                {
                    if (!autoSkip)
                    {
                        if (method.Name.Contains(str)) autoSkip = false;
                    }
                }
                if (method.GetCustomAttributes<SkipValidationAttribute>(false).Any())
                    return true;

                if (method.GetCustomAttributes<EnableValidationAttribute>(false).Any())
                    return false;

                if (method.IsDefined(typeof(EnableValidationAttribute), true))
                    return autoSkip;
                if (method.IsDefined(typeof(SkipValidationAttribute), true))
                    return autoSkip;




                return autoSkip;

            }
            else 
            {
                if (method.IsDefined(typeof(EnableValidationAttribute), true) && !method.GetCustomAttributes<SkipValidationAttribute>(false).Any())
                    return false;

                else if (method.IsDefined(typeof(EnableValidationAttribute), true) && method.GetCustomAttributes<SkipValidationAttribute>(false).Any())
                    return true;

                else if (method.IsDefined(typeof(SkipValidationAttribute), true) && !method.GetCustomAttributes<EnableValidationAttribute>(false).Any())
                    return true;

                else if (method.IsDefined(typeof(SkipValidationAttribute), true) && method.GetCustomAttributes<EnableValidationAttribute>(false).Any())
                    return false;
                else if (method.GetCustomAttributes<EnableValidationAttribute>(false).Any())
                    return false;
                else if (method.GetCustomAttributes<SkipValidationAttribute>(false).Any())
                    return true;

            }


            return ReflectionHelper
                       .GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<SkipValidationAttribute>(method) != null;
        }
    }
}