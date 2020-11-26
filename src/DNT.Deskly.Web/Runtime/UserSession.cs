using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DNT.Deskly.Extensions;
using DNT.Deskly.GuardToolkit;
using DNT.Deskly.Runtime;
using DNT.Deskly.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DNT.Deskly.Web.Runtime
{
    internal sealed class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserSessionOptions _options;


        public UserSession(IHttpContextAccessor context, IOptions<UserSessionOptions> options)
        {
            _context = Ensure.IsNotNull(context, nameof(context));
            _options = Ensure.IsNotNull(options.Value, nameof(options));
        }

        private HttpContext HttpContext => _context.HttpContext;
        private ClaimsPrincipal Principal => HttpContext?.User;

        public bool IsAuthenticated => Principal?.Identity.IsAuthenticated ?? false;
        public string UserId => Principal?.FindUserId();
        public string UserName => Principal?.FindUserName();

        //public IReadOnlyList<string> Permissions =>   TODO : add some inject 
        // TODO : add some inject
        public IReadOnlyList<string> Permissions => Principal?.FindPermissions(_options.PermissionClaimName,_options.IsPackedPermissionAvailable ,_options.PackedPermissionClaimName, _options.PackingSymbol);
        public IReadOnlyList<string> Roles => Principal?.FindRoles();
        public IReadOnlyList<Claim> Claims => Principal?.Claims.ToList();
        public string UserDisplayName => Principal?.FindUserDisplayName();
        public string UserBrowserName => HttpContext?.FindUserAgent();
        public string UserIP => HttpContext?.FindUserIP();
        public string ImpersonatorUserId => Principal?.FindImpersonatorUserId();
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public bool IsInRole(string role)
        {
            ThrowIfUnauthenticated();

            return Principal.IsInRole(role);
        }

        public bool IsGranted(string permission)
        {
            ThrowIfUnauthenticated();

            return Permissions.Any(p => p.Equals(permission, StringComparison.OrdinalIgnoreCase));
        }

        private void ThrowIfUnauthenticated()
        {
            if (!IsAuthenticated) throw new InvalidOperationException("This operation need user authenticated");
        }
    }
}