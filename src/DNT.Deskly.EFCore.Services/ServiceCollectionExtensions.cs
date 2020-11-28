using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            //services.AddTransient(typeof(ICrudService<>), typeof(CrudService<>));
            services.AddTransient(typeof(ICrudService<,>), typeof(CrudService<,>));


            return new ServiceFrameworkBuilder(services);
        }
        public static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
        {
            var typeInfoList = assembly.DefinedTypes.Where(x => x.IsClass
                                && !x.IsAbstract
                                && x != compareType
                                && x.GetInterfaces()
                                        .Any(i => i.IsGenericType
                                                && i.GetGenericTypeDefinition() == compareType))?.ToList();

            return typeInfoList;
        }

        public static void AddClassesAsImplementedInterface(
                this IServiceCollection services,
                Assembly assembly,
                Type compareType,
                ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            assembly.GetTypesAssignableTo(compareType).ForEach((type) =>
            {
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Scoped:
                            services.AddScoped(implementedInterface, type);
                            break;
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(implementedInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(implementedInterface, type);
                            break;
                    }
                }
            });
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