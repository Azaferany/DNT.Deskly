using System;

namespace DNT.Deskly.Validation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class SkipValidationAttribute : Attribute
    {
    }
}