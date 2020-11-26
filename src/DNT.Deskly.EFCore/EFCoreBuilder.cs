using System;
using DNT.Deskly.Configuration;
using DNT.Deskly.EFCore.Configuration;
using DNT.Deskly.EFCore.Context;
using DNT.Deskly.EFCore.Context.Hooks;
using DNT.Deskly.EFCore.Cryptography;
using DNT.Deskly.EFCore.Transaction;
using DNT.Deskly.Transaction;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace DNT.Deskly.EFCore
{
    /// <summary>
    ///     Nice method to create the EFCore builder
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Add the services (application specific tenant class)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static EFCoreBuilder AddEFCore<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IUnitOfWork
        {
            services.AddScoped(provider => (IUnitOfWork)provider.GetRequiredService(typeof(TDbContext)));
            services.AddTransient<TransactionInterceptor>();
            services.AddScoped<IKeyValueService, KeyValueService>();
            services.AddTransient<IHook, PreUpdateRowVersionHook>();

            return new EFCoreBuilder(services, typeof(TDbContext));
        }
    }

    public class EFCoreBuilder
    {
        public EFCoreBuilder(IServiceCollection services, Type contextType)
        {
            Services = services;
            ContextType = contextType;
        }

        public IServiceCollection Services { get; }
        public Type ContextType { get; }

        public EFCoreBuilder WithTransactionOptions(Action<TransactionOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        public EFCoreBuilder WithRowLevelSecurityHook<TUserId>() where TUserId : IEquatable<TUserId>
        {
            Services.AddTransient<IHook, PreInsertRowLevelSecurityHook<TUserId>>();
            Services.AddTransient<IHook, PreUpdateRowLevelSecurityHook<TUserId>>();
            return this;
        }

        public EFCoreBuilder WithTrackingHook<TUserId>() where TUserId : IEquatable<TUserId>
        {
            Services.AddTransient<IHook, PreInsertCreationTrackingHook<TUserId>>();
            Services.AddTransient<IHook, PreUpdateModificationTrackingHook<TUserId>>();
            return this;
        }

        public EFCoreBuilder WithRowIntegrityHook()
        {
            Services.AddTransient<IHook, RowIntegrityHook>();
            return this;
        }

        public EFCoreBuilder WithDeletedEntityHook()
        {
            Services.AddTransient<IHook, PreDeleteDeletedEntityHook>();
            return this;
        }
    }
    public static class DataProtectionExtensions
    {
        /// <summary>
        /// Configures the data protection system to persist keys to an EntityFrameworkCore store
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/> instance to modify.</param>
        /// <returns>The value <paramref name="builder"/>.</returns>
        public static IDataProtectionBuilder PersistKeysToDbContext<TContext>(this IDataProtectionBuilder builder)
            where TContext : DbContext
        {
            builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(provider =>
            {
                var loggerFactory = provider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new XmlRepository<TContext>(provider, loggerFactory);
                });
            });

            return builder;
        }
    }
}