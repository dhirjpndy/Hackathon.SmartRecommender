using Microsoft.Extensions.Logging;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Engines;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.Repositories;
using System;
using System.Diagnostics;
using Hackathon.SmartRecommender.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Factories
{
    internal class CoreDataProviderFactory : ICoreDataProviderFactory
    {
        protected readonly IDbContextFactory DbContextFactory;
        protected readonly ISqlEngine SqlEngine;
        protected readonly ISqlCommand SqlCommand;
        private readonly ILogger<IDataProvider> Logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextFactory"></param>
        /// <param name="sqlEngine"></param>
        /// <param name="sqlCommand"></param>
        /// <param name="logger"></param>
        public CoreDataProviderFactory(
            IDbContextFactory dbContextFactory,
            ISqlEngine sqlEngine,
            ISqlCommand sqlCommand,
            ILogger<IDataProvider> logger)
        {
            DbContextFactory = dbContextFactory;
            SqlEngine = sqlEngine;
            SqlCommand = sqlCommand;
            Logger = logger;
        }

        /// <inheritdoc />
        public IDataProvider CreateForContext(IDbContext dbContext)=> _CreateForContext(dbContext);
        

        protected IDataProvider _CreateForContext(IDbContext dbContext)
        {
            try
            {
                Debug.WriteLine("Created " + dbContext.ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _Create(dbContext);
        }

        protected IDataProvider _Create(IDbContext dbContext)
        {
            return new SqlDataProvider(dbContext, SqlEngine, SqlCommand, Logger);
        }

        /// <inheritdoc />
        public IDataProvider CreateForWsMaster()
        {
            return CreateForWsMaster(null);
        }

        /// <inheritdoc />
        public IDataProvider CreateForWsMaster(ConnectionConfig scope)
        {
            var context = DbContextFactory.CreateForWsMaster(scope);
            return CreateForContext(context);
        }

        /// <inheritdoc />
        public IDataProvider CreateForConnectionString(string connectionString)
        {
            return CreateForConnectionString(connectionString, null);
        }

        /// <inheritdoc />
        public IDataProvider CreateForConnectionString(string connectionString, ConnectionConfig scope)
        {
            var context = DbContextFactory.Create(connectionString, scope);
            return CreateForContext(context);
        }

        /// <inheritdoc />
        public IDataProvider CreateForConnectionStringName(string connectionStringName)
        {
            return CreateForConnectionStringName(connectionStringName, null);

        }

        /// <inheritdoc />
        public IDataProvider CreateForConnectionStringName(string connectionStringName, ConnectionConfig scope)
        {
            var context = DbContextFactory.CreateByName(connectionStringName, scope);
            return CreateForContext(context);
        }
    }
}