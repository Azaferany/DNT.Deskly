using System;
using DNT.Deskly.Domain;
using DNT.Deskly.EFCore.Context.Hooks;
using DNT.Deskly.Extensions;
using DNT.Deskly.Runtime;
using DNT.Deskly.Timing;
using Microsoft.EntityFrameworkCore;

namespace DNT.Deskly.EFCore.Context
{
    public static class HookNames
    {
        public const string CreationTracking = nameof(CreationTracking);
        public const string ModificationTracking = nameof(ModificationTracking);
        public const string RowLevelSecurity = nameof(RowLevelSecurity);
        public const string DeletedEntity = nameof(DeletedEntity);
        public const string RowVersion = nameof(RowVersion);
        public const string RowIntegrity = nameof(RowIntegrity);
        public const string Numbering = nameof(Numbering);
        public const string Tenancy = nameof(Tenancy);
        public const string DomainEvent = nameof(DomainEvent);
    }

    internal sealed class PreInsertCreationTrackingHook<TUserId> : PreInsertHook<ICreationTracking>
        where TUserId : IEquatable<TUserId>
    {
        private readonly IUserSession _session;
        private readonly IClock _clock;

        public PreInsertCreationTrackingHook(IUserSession session, IClock clock)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public override string Name => HookNames.CreationTracking;

        protected override void Hook(ICreationTracking entity, HookEntityMetadata metadata, IUnitOfWork uow)
        {
            metadata.Entry.Property(EFCoreShadow.CreatedDateTime).CurrentValue = _clock.Now;
            metadata.Entry.Property(EFCoreShadow.CreatedByBrowserName).CurrentValue = _session.UserBrowserName;
            metadata.Entry.Property(EFCoreShadow.CreatedByIP).CurrentValue = _session.UserIP;
            metadata.Entry.Property(EFCoreShadow.CreatedByUserId).CurrentValue = _session.UserId.To<TUserId>();
        }
    }

    internal sealed class PreUpdateModificationTrackingHook<TUserId> : PreUpdateHook<IModificationTracking>
        where TUserId : IEquatable<TUserId>
    {
        private readonly IUserSession _session;
        private readonly IClock _clock;

        public PreUpdateModificationTrackingHook(IUserSession session, IClock clock)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public override string Name => HookNames.ModificationTracking;

        protected override void Hook(IModificationTracking entity, HookEntityMetadata metadata, IUnitOfWork uow)
        {
            metadata.Entry.Property(EFCoreShadow.ModifiedDateTime).CurrentValue = _clock.Now;
            metadata.Entry.Property(EFCoreShadow.ModifiedByBrowserName).CurrentValue = _session.UserBrowserName;
            metadata.Entry.Property(EFCoreShadow.ModifiedByIP).CurrentValue = _session.UserIP;
            metadata.Entry.Property(EFCoreShadow.ModifiedByUserId).CurrentValue = _session.UserId.To<TUserId>();
        }
    }
    internal sealed class PreInsertRowLevelSecurityHook<TUserId> : PreInsertHook<IHasRowLevelSecurity>
        where TUserId : IEquatable<TUserId>
    {
        private readonly IUserSession _session;

        public PreInsertRowLevelSecurityHook(IUserSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public override string Name => HookNames.RowLevelSecurity;

        protected override void Hook(IHasRowLevelSecurity entity, HookEntityMetadata metadata, IUnitOfWork uow)
        {
            metadata.Entry.Property(EFCoreShadow.UserId).CurrentValue = _session.UserId.To<TUserId>();
        }
    }

    internal sealed class PreDeleteDeletedEntityHook : PreDeleteHook<IDeletedEntity>
    {
        public override string Name => HookNames.DeletedEntity;

        protected override void Hook(IDeletedEntity entity, HookEntityMetadata metadata, IUnitOfWork uow)
        {
            metadata.Entry.State = EntityState.Modified;
            metadata.Entry.Property(EFCoreShadow.IsDeleted).CurrentValue = true;
        }
    }

    internal sealed class PreUpdateRowVersionHook : PreUpdateHook<IHasRowVersion>
    {
        public override string Name => HookNames.RowVersion;

        protected override void Hook(IHasRowVersion entity, HookEntityMetadata metadata, IUnitOfWork uow)
        {
            metadata.Entry.Property(EFCoreShadow.Version).OriginalValue =
                metadata.Entry.Property(EFCoreShadow.Version).CurrentValue;
        }
    }

    internal sealed class RowIntegrityHook : PostActionHook<IHasRowIntegrity>
    {
        public override string Name => HookNames.RowIntegrity;
        public override int Order => int.MaxValue;
        public override EntityState HookState => EntityState.Unchanged;

        protected override void Hook(IHasRowIntegrity entity, HookEntityMetadata metadata, IUnitOfWork uow)
        {
            metadata.Entry.Property(EFCoreShadow.Hash).CurrentValue = uow.EntityHash(entity);
        }
    }
}