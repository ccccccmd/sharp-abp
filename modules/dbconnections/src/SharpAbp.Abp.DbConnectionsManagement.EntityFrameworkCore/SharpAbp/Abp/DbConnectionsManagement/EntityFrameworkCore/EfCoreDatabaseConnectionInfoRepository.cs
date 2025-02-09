﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace SharpAbp.Abp.DbConnectionsManagement.EntityFrameworkCore
{
    public class EfCoreDatabaseConnectionInfoRepository : EfCoreRepository<IDbConnectionsManagementDbContext, DatabaseConnectionInfo, Guid>, IDatabaseConnectionInfoRepository
    {
        public EfCoreDatabaseConnectionInfoRepository(IDbContextProvider<IDbConnectionsManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        /// <summary>
        /// Find by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<DatabaseConnectionInfo> FindByNameAsync(
            [NotNull] string name,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// Find by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expectedId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<DatabaseConnectionInfo> FindExpectedByNameAsync(
            string name,
            Guid? expectedId = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!name.IsNullOrWhiteSpace(), x => x.Name == name)
                .WhereIf(expectedId.HasValue, x => x.Id != expectedId.Value)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }


        /// <summary>
        /// Get list
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="name"></param>
        /// <param name="databaseProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<DatabaseConnectionInfo>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting = null,
            string name = "",
            string databaseProvider = "",
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!name.IsNullOrWhiteSpace(), item => item.Name == name)
                .WhereIf(!databaseProvider.IsNullOrWhiteSpace(), item => item.DatabaseProvider == databaseProvider)
                .OrderBy(sorting ?? nameof(DatabaseConnectionInfo.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// Get count
        /// </summary>
        /// <param name="name"></param>
        /// <param name="databaseProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> GetCountAsync(
            string name = "", 
            string databaseProvider = "",
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!name.IsNullOrWhiteSpace(), item => item.Name == name)
                .WhereIf(!databaseProvider.IsNullOrWhiteSpace(), item => item.DatabaseProvider == databaseProvider)
                .CountAsync(GetCancellationToken(cancellationToken));
        }
    }
}
