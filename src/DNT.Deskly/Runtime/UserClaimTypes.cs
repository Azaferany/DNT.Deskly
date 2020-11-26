using System.Security.Claims;

namespace DNT.Deskly.Runtime
{
    public static class UserClaimTypes
    {
        public const string UserName = ClaimTypes.Name;
        public const string UserId = ClaimTypes.NameIdentifier;
        public const string SecurityStamp = nameof(SecurityStamp);
        public const string Role = ClaimTypes.Role;
        public const string DisplayName = nameof(DisplayName);
        public const string TenantId = nameof(TenantId);
        public const string TenantName = nameof(TenantName);
        public const string IsHeadTenant = nameof(IsHeadTenant);
        public const string ImpersonatorUserId = nameof(ImpersonatorUserId);
        public const string ImpersonatorTenantId = nameof(ImpersonatorTenantId);
    }
}