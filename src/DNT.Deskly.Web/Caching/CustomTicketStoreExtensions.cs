using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace DNT.Deskly.Web.Caching
{
    public static class CustomTicketStoreExtensions
    {
        public static IServiceCollection AddCustomDistributSqlServerCache(
            this IServiceCollection services, Action<SqlServerCacheOptions> setupAction)
        {
            // To manage large identity cookies
            services.AddDistributedSqlServerCache(setupAction);
            services.AddScoped<ITicketStore, DistributedCacheTicketStore>();
            return services;
        }
        public static IServiceCollection AddCustomDistributedMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            return services;

        }
    }
}