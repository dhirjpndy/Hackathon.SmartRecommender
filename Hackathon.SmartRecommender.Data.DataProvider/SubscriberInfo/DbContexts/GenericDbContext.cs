using ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.UnitOfWork;
using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.SubscriberInfo.DbContexts
{
    internal class GenericDbContext : DbContext
    {
        private readonly string _connectionString;

        /// <summary>
        /// Creates a generic db context with a connection string
        /// </summary>
        /// <param name="dbConnectionFactory">factory for db connections</param>
        /// <param name="connectionStringFactory"></param>
        /// <param name="connectionString">the connection string being used for this context</param>
        /// <param name="scope"></param>
        /// <exception cref="ArgumentNullException">connection string is null</exception>
        /// <inheritdocs/>
        public GenericDbContext(
            IDbConnectionFactory dbConnectionFactory,
            IConnectionStringFactory connectionStringFactory,
            string connectionString, ConnectionConfig scope)
            : base(dbConnectionFactory, scope)
        {
            _connectionString = connectionString;

            // Amend application info before its handed along
            _connectionString += ";Application Name=" + ConnectionStringInformation.AppName;

            if (scope != null)
            {
                _connectionString = connectionStringFactory.HandleOverrides(_connectionString, ConnectionConfig);
            }
        }

        /// <summary>
        /// The connection string this dbContext will use
        /// </summary>
        public override string ConnectionString => _connectionString;

        /// <summary>
        /// The database this dbContext will use
        /// </summary>
        public override string Database => null;
    }
}
