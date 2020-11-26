using System;
using System.Linq;
using DNT.Deskly.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DNT.Deskly.EFCore.Context
{
    public static class EFCoreShadow
    {
        public const string CreatedDateTime = nameof(CreatedDateTime);
        public const string CreatedByUserId = nameof(CreatedByUserId);
        public const string CreatedByBrowserName = nameof(CreatedByBrowserName);
        public const string CreatedByIP = nameof(CreatedByIP);

        public const string ModifiedDateTime = nameof(ModifiedDateTime);
        public const string ModifiedByUserId = nameof(ModifiedByUserId);
        public const string ModifiedByBrowserName = nameof(ModifiedByBrowserName);
        public const string ModifiedByIP = nameof(ModifiedByIP);

        public const string UserId = nameof(UserId);
        public const string TenantId = nameof(TenantId);
        public const string IsDeleted = nameof(IsDeleted);
        public const string Version = nameof(Version);
        public const string Hash = nameof(Hash);

        public static readonly Func<object, byte[]> PropertyVersion =
            entity => Microsoft.EntityFrameworkCore.EF.Property<byte[]>(entity, Version);

        public static readonly Func<object, string> PropertyHash =
            entity => Microsoft.EntityFrameworkCore.EF.Property<string>(entity, Hash);

        public static readonly Func<object, string> PropertyCreatedByBrowserName =
            entity => Microsoft.EntityFrameworkCore.EF.Property<string>(entity, CreatedByBrowserName);

        public static readonly Func<object, string> PropertyCreatedByIP =
            entity => Microsoft.EntityFrameworkCore.EF.Property<string>(entity, CreatedByIP);

        public static TUserId PropertyCreatedByUserId<TUserId>(object entity) =>
            Microsoft.EntityFrameworkCore.EF.Property<TUserId>(entity, CreatedByUserId);

        public static readonly Func<object, DateTime> PropertyCreatedDateTime =
            entity => Microsoft.EntityFrameworkCore.EF.Property<DateTime>(entity, CreatedDateTime);

        public static readonly Func<object, string> PropertyModifiedByBrowserName =
            entity => Microsoft.EntityFrameworkCore.EF.Property<string>(entity, ModifiedByBrowserName);

        public static readonly Func<object, string> PropertyModifiedByIP =
            entity => Microsoft.EntityFrameworkCore.EF.Property<string>(entity, ModifiedByIP);

        public static TUserId PropertyModifiedByUserId<TUserId>(object entity) =>
            Microsoft.EntityFrameworkCore.EF.Property<TUserId>(entity, ModifiedByUserId);

        public static readonly Func<object, DateTime?> PropertyModifiedDateTime =
            entity => Microsoft.EntityFrameworkCore.EF.Property<DateTime?>(entity, ModifiedDateTime);

        public static TTenantId PropertyTenantId<TTenantId>(object entity) =>
            Microsoft.EntityFrameworkCore.EF.Property<TTenantId>(entity, TenantId);

        public static void AddTrackingFields<TUserId>(this ModelBuilder builder) where TUserId : IEquatable<TUserId>
        {
            var types = builder.Model.GetEntityTypes().ToList();

            var propertyType = typeof(TUserId).IsValueType
                ? typeof(Nullable<>).MakeGenericType(typeof(TUserId))
                : typeof(TUserId);

            foreach (var entityType in types.Where(e => typeof(ICreationTracking).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property<DateTime>(CreatedDateTime)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                builder.Entity(entityType.ClrType)
                    .Property<string>(CreatedByBrowserName).HasMaxLength(1024)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                builder.Entity(entityType.ClrType)
                    .Property<string>(CreatedByIP).HasMaxLength(256)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                builder.Entity(entityType.ClrType)
                    .Property(propertyType, CreatedByUserId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }

            foreach (var entityType in types.Where(e => typeof(IModificationTracking).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType.Name)
                    .Property<DateTime?>(ModifiedDateTime);

                builder.Entity(entityType.ClrType)
                    .Property<string>(ModifiedByBrowserName).HasMaxLength(1024);

                builder.Entity(entityType.ClrType)
                    .Property<string>(ModifiedByIP).HasMaxLength(256);

                builder.Entity(entityType.ClrType)
                    .Property(propertyType, ModifiedByUserId);
            }
        }

        public static void AddIsDeletedField(this ModelBuilder builder)
        {
            var types = builder.Model.GetEntityTypes().ToList();

            foreach (var entityType in types.Where(e => typeof(IDeletedEntity).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property<bool>(IsDeleted);
            }
        }

        public static void AddRowLevelSecurityField<TUserId>(this ModelBuilder builder)
            where TUserId : IEquatable<TUserId>
        {
            var types = builder.Model.GetEntityTypes().ToList();

            foreach (var entityType in types.Where(e => typeof(IHasRowLevelSecurity).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property(typeof(TUserId), UserId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }
        }

        public static void AddRowVersionField(this ModelBuilder builder)
        {
            var types = builder.Model.GetEntityTypes().ToList();

            foreach (var entityType in types.Where(e => typeof(IHasRowVersion).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property<byte[]>(Version)
                    .IsRowVersion();
            }
        }

        public static void AddRowIntegrityField(this ModelBuilder builder)
        {
            var types = builder.Model.GetEntityTypes().ToList();

            foreach (var entityType in types.Where(e => typeof(IHasRowIntegrity).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property<string>(Hash)
                    .HasMaxLength(256);
            }
        }
    }
}