using System;
using DNT.Deskly.Caching;
using DNT.Deskly.Eventing;
using DNT.Deskly.Timing;
using DNT.Deskly.Validation;
using DNT.Deskly.Validation.Interception;
using Microsoft.Extensions.DependencyInjection;

namespace DNT.Deskly
{
    public static class ServiceCollectionExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static FrameworkBuilder AddFramework(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IEventBus, EventBus>();
            services.AddTransient<IClock, Clock>();

            return new FrameworkBuilder(services);
        }
    }

    /// <summary>
    /// Configure  DNT.Deskly services
    /// </summary>
    public sealed class FrameworkBuilder
    {
        public IServiceCollection Services { get; }

        public FrameworkBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Register the ICacheService
        /// </summary>
        public FrameworkBuilder WithMemoryCache()
        {
            Services.AddMemoryCache();
            Services.AddSingleton<ICacheService, MemoryCacheService>();
            return this;
        }




        /// <summary>
        /// Register the validation infrastructure's services
        /// </summary>
        public FrameworkBuilder WithModelValidation(Action<ValidationOptions> setupAction = null)
        {
            //TODO: auto regester IModelValidation class
            Services.AddTransient<ValidationInterceptor>();
            Services.AddTransient<MethodInvocationValidator>();
            Services.AddTransient<IMethodParameterValidator, DataAnnotationMethodParameterValidator>();
            Services.AddTransient<IMethodParameterValidator, ValidatableObjectMethodParameterValidator>();
            Services.AddTransient<IMethodParameterValidator, ModelValidationMethodParameterValidator>();

            if (setupAction != null)
            {
                Services.Configure(setupAction);
            }

            return this;
        }
    }
}