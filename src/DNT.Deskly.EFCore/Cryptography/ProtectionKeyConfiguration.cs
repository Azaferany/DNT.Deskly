using System;
using DNT.Deskly.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNT.Deskly.EFCore.Cryptography
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyProtectionKeyConfiguration(this ModelBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ApplyConfiguration(new ProtectionKeyConfiguration());
        }
    }

    public class ProtectionKeyConfiguration : IEntityTypeConfiguration<ProtectionKey>
    {
        public void Configure(EntityTypeBuilder<ProtectionKey> builder)
        {
            builder.ToTable(nameof(ProtectionKey), "dbo");

            builder.Property(a => a.FriendlyName).IsRequired();
            builder.HasIndex(a => a.FriendlyName).IsUnique().HasDatabaseName("IX_ProtectionKey_FriendlyName");
        }
    }
}