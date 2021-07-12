using Microsoft.Extensions.Logging;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Engines;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Monitoring;
using ConsumerMarketplace.AdminFlexTool.Data.SubscriberInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ConsumerMarketplace.AdminFlexTool.Data.Repositories
{
    /// <summary>
    /// A basic repository which provides subscriber database information
    /// </summary>
    internal class SubscriberDbSqlRepository : ISubscriberDbProvider
    {
        /// <summary>
        /// Encourage reuse of parameter instead of having to return to garbage collection
        /// </summary>
        private static readonly ConnectionConfig DefaultReadonlyConfig = ConnectionFlags.ReadOnly;

        private readonly IDataProvider _dataProvider;

        public SubscriberDbSqlRepository(
            IDbContextFactory dbContextFactory,
            ISqlEngine sqlEngine,
            ISqlCommand sqlCommand,
            ILogger<IDataProvider> logger
        )
        {
            var wsMasterContext = dbContextFactory.CreateForWsMaster(DefaultReadonlyConfig);
            _dataProvider = new SqlDataProvider(wsMasterContext, sqlEngine, sqlCommand, logger);
        }

        #region Implementation of ISubscriberDbProvider

        private const string StudioBase = @"SELECT [StudioID], [DatabaseName], [DatabaseServerFQDN] FROM [dbo].[FQDNStudios]";
        private const string SubscriberSelect = @" WHERE [StudioId] = @SubscriberId";

        private static Func<DataRow, SubscriberDatabase> MappingFunc()
        {
            return dataRow =>
            {
                var id = dataRow.Field<int>("StudioID");
                var dbServerFQDN = dataRow.Field<string>("DatabaseServerFQDN");
                var dbName = dataRow.Field<string>("DatabaseName");
                return new SubscriberDatabase
                {
                    Id = id,
                    ServerName = dbServerFQDN,
                    DatabaseName = dbName
                };
            };
        }

        /// <summary>
        /// Fetch a single subscriber's information
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public SubscriberDatabase GetSubscriberDatabaseInfo(int subscriberId)
        {
            const string sql = StudioBase + SubscriberSelect;
            var parameters = new List<SqlParameter>
            {
                SqlParameterFactory.Create("@SubscriberId", SqlDbType.Int, subscriberId)
            };

            var results = _dataProvider.Read(sql, MappingFunc(), parameters);
            var infoIfAvailable = results.FirstOrDefault();

            var wasAvailable = infoIfAvailable != null ? "Found" : "NotFound";
            SubscriberInfoTracer.AddSubscriberInfoMiss($"DP:FetchNoCache:{wasAvailable}");

            return infoIfAvailable;
        }

        #endregion
    }
}