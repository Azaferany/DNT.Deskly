﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNT.Deskly.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
// ReSharper disable InconsistentNaming

namespace DNT.Deskly.EFCore.Logging
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggingBuilder AddEFCore<TContext>(this ILoggingBuilder builder)
         where TContext : DbContext
        {
            builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider<TContext>>();

            return builder;
        }
        public static ILoggingBuilder AddEFCore<TContext>(this ILoggingBuilder builder, Action<DbLoggerOptions>
            configuration)
            where TContext : DbContext
        {
            builder.AddEFCore<TContext>();
            builder.Services.Configure(configuration);

            return builder;
        }
    }

    [ProviderAlias("EFCore")]
    internal class DbLoggerProvider<TContext> : BatchingLoggerProvider
        where TContext : DbContext
    {
        private readonly IServiceProvider _provider;

        public DbLoggerProvider(IOptions<DbLoggerOptions> options, IServiceProvider provider) : base(options, provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        protected override async Task WriteLogsAsync(IEnumerable<LogItem> items, CancellationToken token)
        {
            var logs = items.Where(m => !string.IsNullOrEmpty(m.Message))
                .Select(m => new Log
                {
                    Level = m.Level.ToString(),
                    LoggerName = m.LoggerName,
                    EventId = m.EventId.Id,
                    Message = m.Message,
                    UserIP = m.UserIP,
                    UserId = m.UserId,
                    UserBrowserName = m.UserBrowserName,
                    UserDisplayName = m.UserDisplayName,
                    UserName = m.UserName,
                    CreationTime = m.CreationTime,
                    ImpersonatorUserId = m.ImpersonatorUserId,
                    TenantId = m.TenantId,
                    TenantName = m.TenantName,
                    ImpersonatorTenantId = m.ImpersonatorTenantId
                });

            using var scope = _provider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<TContext>();
            context.Set<Log>().AddRange(logs);
            await context.SaveChangesAsync(token);
        }
    }
}