﻿using Volo.Abp.Data;

namespace SharpAbp.Abp.DbConnectionsManagement
{
    public class DbConnectionsManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpDbConnectionsManagement";
    }
}
