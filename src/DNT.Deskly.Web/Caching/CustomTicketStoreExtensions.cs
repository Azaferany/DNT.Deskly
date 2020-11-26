using System;
using DNT.Deskly.Web.Caching;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNETCoreIdentitySample.IocConfig
{
    public static class CustomTicketStoreExtensions
    {
        public static IServiceCollection AddDistributSqlServerCache(
            this IServiceCollection services, Action<SqlServerCacheOptions> setupAction)
        {
            // To manage large identity cookies
            services.AddDistributedSqlServerCache(setupAction);
            services.AddScoped<ITicketStore, DistributedCacheTicketStore>();
            return services;
        }
        public static IServiceCollection AddDistributedMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            return services;

        }
    }
}