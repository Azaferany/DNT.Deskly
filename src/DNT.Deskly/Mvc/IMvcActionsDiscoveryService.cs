using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNT.Deskly.Mvc
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2573
    /// </summary>
    public interface IMvcActionsDiscoveryService
    {
        /// <summary>
        /// Returns the list of all of the controllers and action methods of an MVC application.
        /// </summary>
        ICollection<MvcControllerViewModel> MvcControllers { get; }

        /// <summary>
        /// Returns the list of all of the controllers and action methods of an MVC application which have AuthorizeAttribute and the specified policyName.
        /// </summary>
        ICollection<MvcControllerViewModel> GetAllSecuredControllerActionsWithPolicy(string policyName);
    }
    /// <summary>
    /// MvcController ViewModel
    /// </summary>
    public class MvcControllerViewModel
    {
        /// <summary>
        /// Return `AreaAttribute.RouteValue`
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// Returns the list of the Controller's Attributes.
        /// </summary>
        public IList<Attribute> ControllerAttributes { get; set; }

        /// <summary>
        /// Returns the `DisplayNameAttribute` value
        /// </summary>
        public string ControllerDisplayName { get; set; }

        /// <summary>
        /// It's set to `{AreaName}:{ControllerName}`
        /// </summary>
        public string ControllerId => $"{AreaName}:{ControllerName}";

        /// <summary>
        /// Return ControllerActionDescriptor.ControllerName
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Returns the list of the Controller's action methods.
        /// </summary>
        public IList<MvcActionViewModel> MvcActions { get; set; } = new List<MvcActionViewModel>();

        /// <summary>
        /// Returns `[{controllerAttributes}]{AreaName}.{ControllerName}`
        /// </summary>
        public override string ToString()
        {
            const string attribute = "Attribute";
            var controllerAttributes = string.Join(",", ControllerAttributes.Select(a => a.GetType().Name.Replace(attribute, "")));
            return $"[{controllerAttributes}]{AreaName}.{ControllerName}";
        }
    }
    /// <summary>
    /// MvcAction ViewModel
    /// </summary>
    public class MvcActionViewModel
    {
        /// <summary>
        /// Returns the list of Attributes of the action method.
        /// </summary>
        public IList<Attribute> ActionAttributes { get; set; }

        /// <summary>
        /// Returns `DisplayNameAttribute` value of the action method.
        /// </summary>
        public string ActionDisplayName { get; set; }

        /// <summary>
        /// It's set to `{ControllerId}:{ActionName}`
        /// </summary>
        public string ActionId => $"{ControllerId}:{ActionName}";

        /// <summary>
        /// Return ControllerActionDescriptor.ActionName
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// It's set to `{AreaName}:{ControllerName}`
        /// </summary>
        public string ControllerId { get; set; }

        /// <summary>
        /// Returns true if the action method has an `AuthorizeAttribute`.
        /// </summary>
        public bool IsSecuredAction { get; set; }


        /// <summary>
        /// Returns `[{actionAttributes}]{ActionName}`
        /// </summary>
        public override string ToString()
        {
            const string attribute = "Attribute";
            var actionAttributes = string.Join(",", ActionAttributes.Select(a => a.GetType().Name.Replace(attribute, "")));
            return $"[{actionAttributes}]{ActionName}";
        }
    }

}
