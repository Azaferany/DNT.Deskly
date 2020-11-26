using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DNT.Deskly.Runtime;

namespace DNT.Deskly.Extensions
{
    public static class PrincipalExtensions
    {
        /// <summary>
        /// Returns the value for the first claim of the specified type otherwise null the claim is not present.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance this method extends.</param>
        /// <param name="claimType">The claim type whose first value should be returned.</param>
        /// <returns>The value of the first instance of the specified claim type, or null if the claim is not present.</returns>        
        public static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claim = principal.FindFirst(claimType);
            return claim?.Value;
        }

        public static IReadOnlyList<string> FindRoles(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var roles = principal.Claims
                .Where(c => c.Type.Equals(UserClaimTypes.Role, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value).ToList();

            return roles.AsReadOnly();
        }

        public static string FindUserId(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirstValue(UserClaimTypes.UserId);
            return value;
        }
        public static string FindClaimsByType(this ClaimsPrincipal principal, string claimType)
        {
            return principal.FindFirstValue(claimType);
        }

        public static string FindTenantId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(UserClaimTypes.TenantId);
        }

        public static string FindTenantName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(UserClaimTypes.TenantName);
        }

        public static bool IsHeadTenant(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(UserClaimTypes.IsHeadTenant, "true");
        }
        public static IReadOnlyList<string> FindPermissions(this ClaimsPrincipal principal,
            string permissionClaimName,bool isPackedPermissionAvailable,
            string packedPermissionClaimName, string packingSymbol)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var permissions = principal.Claims
                .Where(c => c.Type.Equals(permissionClaimName, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value)
                .ToList();
            if (isPackedPermissionAvailable && !packedPermissionClaimName.IsNullOrEmpty())
            {
                var packedPermissions = principal.Claims.Where(c =>
                        c.Type.Equals(packedPermissionClaimName, StringComparison.OrdinalIgnoreCase))
                    .SelectMany(c => c.Value.UnpackFromString(packingSymbol));

                permissions.AddRange(packedPermissions);
            }

            return permissions.AsReadOnly();
        }
        public static string FindImpersonatorTenantId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(UserClaimTypes.ImpersonatorTenantId);
        }

        public static string FindImpersonatorUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(UserClaimTypes.ImpersonatorUserId);
        }

        public static string FindUserDisplayName(this ClaimsPrincipal principal)
        {
            var displayName = principal.FindFirstValue(UserClaimTypes.UserName);
            return string.IsNullOrWhiteSpace(displayName) ? principal.FindUserName() : displayName;
        }

        public static string FindUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(UserClaimTypes.UserName);
        }
    }
}