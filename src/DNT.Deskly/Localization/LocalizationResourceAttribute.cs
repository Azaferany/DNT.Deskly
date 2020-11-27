using System;

namespace DNT.Deskly.Localization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LocalizationResourceAttribute : Attribute
    {
        /// <summary>
        /// gets or sets name of resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets location of resource
        /// </summary>
        public string Location { get; set; }
    }
}