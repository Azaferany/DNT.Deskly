using System;
using DNT.Deskly.EFCore.Services.Application;
using DNT.Deskly.Mvc;
using DNT.Deskly.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace DNT.Deskly.Web
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceFrameworkBuilder AddCrudServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient(typeof(ICrudService<>), typeof(CrudService<>));
            services.AddTransient(typeof(ICrudService<,>), typeof(CrudService<,>));


            return new ServiceFrameworkBuilder(services);
        }
    }

    public class ServiceFrameworkBuilder
    {
        private IServiceCollection Services { get; }

        public ServiceFrameworkBuilder(IServiceCollection services)
        {
            Services = services;
        }


    }
}