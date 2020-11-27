using System;
using DNT.Deskly.Logging;
using DNT.Deskly.Mvc;
using DNT.Deskly.Runtime;
using DNT.Deskly.Web.Mvc;
using DNT.Deskly.Web.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace DNT.Deskly.Web
{
    public static class ServiceCollectionExtensions
    {
        public static WebFrameworkBuilder AddWebFramework(this IServiceCollection services, Action<UserSessionOptions> configure)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            services.AddHttpContextAccessor();
            services.AddScoped<IUserSession, UserSession>();

            services.Configure(configure);


            return new WebFrameworkBuilder(services);
        }
    }

    /// <summary>
    /// Configure DNT.Deskly.Web services
    /// </summary>
    public class WebFrameworkBuilder
    {
        private IServiceCollection Services { get; }

        public WebFrameworkBuilder(IServiceCollection services)
        {
            Services = services;
        }
        /// <summary>
        /// Adds IMvcActionsDiscoveryService to IServiceCollection.
        /// </summary>
        public WebFrameworkBuilder AddMvcActionsDiscoveryService(WebFrameworkBuilder builder)
        {
            builder.Services.AddSingleton<IMvcActionsDiscoveryService, MvcActionsDiscoveryService>();
            return builder;
        }

    }
}