using System;
using System.Collections.Generic;
using System.Text;

namespace DNT.Deskly.Runtime
{
    public class UserSessionOptions
    {
        public string PermissionClaimName { get; set; }
        /// <summary>
        /// false by default
        /// </summary>
        public bool IsPackedPermissionAvailable { get; set; } = false;
        public string PackedPermissionClaimName { get; set; }
        /// <summary>
        ///  default value  = ","
        /// </summary>
        public string PackingSymbol { get; set; } = ",";


    }
}
