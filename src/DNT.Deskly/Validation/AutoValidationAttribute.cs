using System;

namespace DNT.Deskly.Validation
{
    /// <summary>
    /// Can be added to a method to enable auto validation if validation is disabled for it's class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoSkipValidationAttribute : Attribute
    {
        public readonly string[] SkipValidationNames = { "Find", "Remove", "Delete", "Get" };
        public AutoSkipValidationAttribute(params string[] skipValidationNames)
        {
            SkipValidationNames = skipValidationNames;
        }
        public AutoSkipValidationAttribute()
        {

        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoEnableValidationAttribute : Attribute
    {
        public readonly string[] EnableValidationNames = { "Update", "Add", "Create", "Edit" };
        public AutoEnableValidationAttribute(params string[] skipValidationNames)
        {
            EnableValidationNames = skipValidationNames;
        }
        public AutoEnableValidationAttribute()
        {

        }
    }
}