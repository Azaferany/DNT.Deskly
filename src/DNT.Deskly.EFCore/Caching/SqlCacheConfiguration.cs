using System;
using DNT.Deskly.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;

namespace DNT.Deskly.EFCore.Caching
{
    public static class ModelBuilderExtensions
    {
        public static void ApplySqlCacheConfiguration(this ModelBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.ApplyConfigurationsFromAssembly(typeof(CacheConfiguration).Assembly, x => x == typeof(CacheConfiguration));

        }
    }

    public class CacheConfiguration : IEntityTypeConfiguration<Cache>
    {
        private readonly SqlServerCacheOptions _options;
        public CacheConfiguration(IOptions<SqlServerCacheOptions> options)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options), "Should SqlServerCacheOptions Injected");
            _options = options.Value ?? throw new ArgumentNullException(nameof(SqlServerCacheOptions), "Should SqlServerCacheOptions Injected");
        }
        public void Configure(EntityTypeBuilder<Cache> builder)
        {
            builder.Property(e => e.Id).HasMaxLength(449);
            builder.Property(e => e.Value).IsRequired();

            builder.HasIndex(e => e.ExpiresAtTime).HasDatabaseName("IX_Cache_ExpiresAtTime");

            builder.ToTable(name: _options.TableName, schema: _options.SchemaName);
        }
    }
}