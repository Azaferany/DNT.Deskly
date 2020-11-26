using System;
using DNT.Deskly.Logging;
using DNT.Deskly.Runtime;
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
        public IServiceCollection Services { get; }

        public WebFrameworkBuilder(IServiceCollection services)
        {
            Services = services;
        }


    }
}