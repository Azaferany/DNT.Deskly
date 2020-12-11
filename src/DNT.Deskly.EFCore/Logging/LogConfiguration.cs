using System;
using DNT.Deskly.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNT.Deskly.EFCore.Logging
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyLogConfiguration(this ModelBuilder modelBuilder, string tableName = nameof(Log), string schema = "dbo")
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            var builder = modelBuilder.Entity<Log>();

            builder.ToTable(tableName, schema);

            builder.HasIndex(e => e.LoggerName).HasDatabaseName("IX_Log_LoggerName");
            builder.HasIndex(e => e.Level).HasDatabaseName("IX_Log_Level");

            builder.Property(a => a.Level).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Message).IsRequired();
            builder.Property(a => a.LoggerName).HasMaxLength(256).IsRequired();
            builder.Property(a => a.UserDisplayName).HasMaxLength(50);
            builder.Property(a => a.UserName).HasMaxLength(50);
            builder.Property(a => a.UserBrowserName).HasMaxLength(1024);
            builder.Property(a => a.UserIP).HasMaxLength(256);
            builder.Property(a => a.UserId).HasMaxLength(256);
            builder.Property(a => a.ImpersonatorUserId).HasMaxLength(256);
            builder.Property(a => a.TenantId).HasMaxLength(256);
            builder.Property(a => a.TenantName).HasMaxLength(256);
            builder.Property(a => a.ImpersonatorTenantId).HasMaxLength(256);
        }

    }
}