using System;

namespace DNT.Deskly.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class SkipNormalizationAttribute : Attribute
    {
    }
}