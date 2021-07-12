using ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.SubscriberInfo;
using ConsumerMarketplace.AdminFlexTool.Data.SubscriberInfo.DbContexts;
using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.Factories
{
    internal class DbContextFactory : IDbContextFactory
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IConnectionStringFactory _connectionStringFactory;

        public DbContextFactory(
            IDbConnectionFactory dbConnectionFactory,
            IConnectionStringFactory connectionStringFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _connectionStringFactory = connectionStringFactory;
        }

        public IDbContext CreateForWsMaster()
        {
            return CreateForWsMaster(null);
        }

        public IDbContext CreateForWsMaster(ConnectionConfig connectionConfig)
        {
            return CreateByName("wsMaster", connectionConfig);
        }

        public IDbContext CreateByName(string connectionStringName)
        {
            return CreateByName(connectionStringName, null);
        }

        public IDbContext CreateByName(string connectionStringName, ConnectionConfig connectionConfig)
        {
            if (string.IsNullOrWhiteSpace(connectionStringName))
            {
                throw new ArgumentException("Connection String Name not set");
            }

            var connectionString = _connectionStringFactory.Create(connectionStringName, connectionConfig);
            return Create(connectionString, null);
        }

        public IDbContext Create(string connectionString)
        {
            return Create(connectionString, null);
        }

        public IDbContext Create(string connectionString, ConnectionConfig connectionConfig)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection String not set");
            }

            return new GenericDbContext(_dbConnectionFactory, _connectionStringFactory, connectionString, connectionConfig);
        }

        public IDbContext CreateForSubscriber(int subscriberId,
            string databaseName, string databaseServerName, ConnectionConfig connectionConfig)
        {          
            var subscriberDatabase = new SubscriberDatabase
            {
                Id = subscriberId,
                DatabaseName = databaseName,
                ServerName = databaseServerName
            };
            return new SubscriberContext(_dbConnectionFactory, _connectionStringFactory, subscriberDatabase, connectionConfig);
        }
    }
}