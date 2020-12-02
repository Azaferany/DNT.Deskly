using System;

namespace DNT.Deskly.Validation
{
    /// <summary>
    /// Can be added to a method to enable auto validation if validation is disabled for it's class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class EnableValidationAttribute : Attribute
    {
    }
}