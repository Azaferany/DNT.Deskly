using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNT.Deskly.Web.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Make sure you add this code BEFORE app.UseStaticFiles();,
        /// otherwise the headers will not be applied to your static files.
        /// </summary>
        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContentSecurityPolicyMiddleware>();
        }

    }
}
