// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNT.Deskly.EFCore.Context.Hooks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.ApiAuthorization.IdentityServer
{
    public abstract class ApiAuthorizationDbContext<TUser> :
        ApiAuthorizationDbContext<TUser, string>
        where TUser : IdentityUser
    {
        public ApiAuthorizationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions, IEnumerable<IHook> hooks)
            : base(options, operationalStoreOptions, hooks) { }
    }
    public abstract class ApiAuthorizationDbContext<TUser, TKey> :
        ApiAuthorizationDbContext<TUser, TKey, IdentityUserClaim<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public ApiAuthorizationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions, IEnumerable<IHook> hooks)
            : base(options, operationalStoreOptions, hooks) { }
    }
    public abstract class ApiAuthorizationDbContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken> :
        ApiAuthorizationDbContext<TUser, IdentityRole<TKey>, TKey, TUserClaim, IdentityUserRole<TKey>, TUserLogin, IdentityRoleClaim<TKey>, TUserToken>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        public ApiAuthorizationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions, IEnumerable<IHook> hooks) 
            : base(options, operationalStoreOptions, hooks) { }
    }
    /// <summary>
    /// Database abstraction for a combined <see cref="DbContext"/> using ASP.NET Identity and Identity Server.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public abstract class ApiAuthorizationDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> :
    IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>,
    IPersistedGrantDbContext
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>
    where TUserRole : IdentityUserRole<TKey>
    where TUserLogin : IdentityUserLogin<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
    where TUserToken : IdentityUserToken<TKey>
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiAuthorizationDbContext{TUser}"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/>.</param>
        /// <param name="operationalStoreOptions">The <see cref="IOptions{OperationalStoreOptions}"/>.</param>
        public ApiAuthorizationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions, IEnumerable<IHook> hooks) : base(options, hooks)
        {
            _operationalStoreOptions = operationalStoreOptions;
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{PersistedGrant}"/>.
        /// </summary>
        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{DeviceFlowCodes}"/>.
        /// </summary>
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
        }
    }
}
